// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-11-20
// Last Modified:           2017-11-20
//

using cloudscribe.SimpleContent.Models;
using Markdig;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
        private MarkdownPipeline _mdPipeline = null;

        static readonly Regex YamlExtractionRegex = new Regex("^---[\n,\r\n].*?^(---[\n,\r\n]|...[\n,\r\n])", RegexOptions.Singleline | RegexOptions.Multiline);

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

        public Post Deserialize(string markdownWithYamlString, string key)
        {
            
            string extractedYaml = null;
            var match = YamlExtractionRegex.Match(markdownWithYamlString);

            var post = new Post();
            if(match.Success)
            {
                extractedYaml = match.Value;

                var yaml = ExtractString(extractedYaml, "---", "\n---", returnDelimiters: false).Trim();
                var input = new StringReader(yaml);

                var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

                var yamlPost = deserializer.Deserialize<YamlPost>(input);
                yamlPost.CopyTo(post);
               
                //post.Id = key;
                post.Comments.AddRange(yamlPost.TheComments);
                post.Content = markdownWithYamlString.Replace(extractedYaml, string.Empty).TrimStart('\r', '\n');
            }
            else
            {
                _log.LogError($"failed to deserialize yamlBlock from post with key {key}");
            }
            
            return post;

        }

        private static string ExtractString(string source,
            string beginDelim,
            string endDelim,
            bool caseSensitive = false,
            bool allowMissingEndDelimiter = false,
            bool returnDelimiters = false)
        {
            int at1, at2;

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (caseSensitive)
            {
                at1 = source.IndexOf(beginDelim);
                if (at1 == -1)
                    return string.Empty;

                at2 = source.IndexOf(endDelim, at1 + beginDelim.Length);
            }
            else
            {
                //string Lower = source.ToLower();
                at1 = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (at1 == -1)
                    return string.Empty;

                at2 = source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.OrdinalIgnoreCase);
            }

            if (allowMissingEndDelimiter && at2 < 0)
            {
                if (!returnDelimiters)
                    return source.Substring(at1 + beginDelim.Length);

                return source.Substring(at1);
            }

            if (at1 > -1 && at2 > 1)
            {
                if (!returnDelimiters)
                    return source.Substring(at1 + beginDelim.Length, at2 - at1 - beginDelim.Length);

                return source.Substring(at1, at2 - at1 + endDelim.Length);
            }

            return string.Empty;
        }


    }
}
