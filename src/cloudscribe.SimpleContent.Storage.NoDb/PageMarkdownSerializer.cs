// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-11-21
// Last Modified:           2017-11-21
//

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PageMarkdownSerializer : IStringSerializer<Page>
    {
        public PageMarkdownSerializer(
            ILogger<PageMarkdownSerializer> logger
            )
        {
            _log = logger;
        }

        private ILogger _log;

        public string ExpectedFileExtension { get; } = ".md";

        public string Serialize(Page page)
        {
            var yamlPage = new YamlPage();
            page.CopyTo(yamlPage);
            //foreach (var c in page.Comments)
            //{
            //    if (c is Comment comment)
            //    {
            //        yamlPage.TheComments.Add(comment);
            //    }
            //}

            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            var yaml = serializer.Serialize(yamlPage);

            var sb = new StringBuilder();
            sb.Append("---\r\n");
            sb.Append(yaml);
            sb.Append("---\r\n");
            sb.Append(page.Content);

            return sb.ToString();

        }

        public Page Deserialize(string markdownWithYaml, string key)
        {
            var yamlHelper = new YamlHelper();
            string extractedYaml = null;
            var match = yamlHelper.MatchFrontMatter(markdownWithYaml);

            var page = new Page();
            if (match.Success)
            {
                extractedYaml = match.Value;

                var yaml = yamlHelper.RemoveFrontMatterDelimiters(extractedYaml).Trim();
                var input = new StringReader(yaml);

                var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

                var yamlPost = deserializer.Deserialize<YamlPage>(input);
                yamlPost.CopyTo(page);

                //post.Id = key;
                //post.Comments.AddRange(yamlPost.TheComments);
                page.Content = markdownWithYaml.Replace(extractedYaml, string.Empty).TrimStart('\r', '\n');
            }
            else
            {
                _log.LogError($"failed to deserialize yamlBlock from page with key {key}");
            }

            return page;

        }

    }
}
