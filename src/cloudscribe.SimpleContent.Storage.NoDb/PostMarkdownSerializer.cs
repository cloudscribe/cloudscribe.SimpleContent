// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-11-20
// Last Modified:           2017-11-21
//

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PostMarkdownSerializer : IStringSerializer<Post>
    {
        public PostMarkdownSerializer(
            ILogger<PostMarkdownSerializer> logger)
        {
            _log = logger;
        }

        private ILogger _log;
        
        public string ExpectedFileExtension { get; } = ".md";

        public string Serialize(Post post)
        {
            var yamlPost = new YamlPost();
            post.CopyTo(yamlPost);
            foreach(var c in post.Comments)
            {
                if (c is Comment comment)
                {
                    yamlPost.TheComments.Add(comment);
                }
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            var yaml = serializer.Serialize(yamlPost);

            var sb = new StringBuilder();
            sb.Append("---\r\n");
            sb.Append(yaml);
            sb.Append("---\r\n");
            sb.Append(post.Content);

            return sb.ToString();

        }

        public Post Deserialize(string markdownWithYaml, string key)
        {
            var yamlHelper = new YamlHelper();
            string extractedYaml = null;
            var match = yamlHelper.MatchFrontMatter(markdownWithYaml);

            var post = new Post();
            if(match.Success)
            {
                extractedYaml = match.Value;

                var yaml = yamlHelper.RemoveFrontMatterDelimiters(extractedYaml).Trim();
                var input = new StringReader(yaml);

                var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

                try
                {
                    var yamlPost = deserializer.Deserialize<YamlPost>(input);
                    yamlPost.CopyTo(post);
                    post.Comments.AddRange(yamlPost.TheComments);
                    post.Content = markdownWithYaml.Replace(extractedYaml, string.Empty).TrimStart('\r', '\n');
                }
                catch(YamlException ex)
                {
                    _log.LogError($"failed to deserialize post with key {key}, error message {ex.Message} stacktrace {ex.StackTrace}");
                    return null;
                }
                
            }
            else
            {
                _log.LogError($"failed to deserialize yamlBlock from post with key {key}");
                return null;
            }
            
            return post;

        }

       
    }
}
