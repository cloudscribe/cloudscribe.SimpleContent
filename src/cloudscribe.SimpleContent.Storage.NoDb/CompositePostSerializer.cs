// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-11-20
// Last Modified:           2017-11-20
// 


using cloudscribe.SimpleContent.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class CompositePostSerializer : IStringSerializer<Post>
    {
        public CompositePostSerializer(
            PostXmlSerializer xmlSerializer,
            PostMarkdownSerializer mardownSerializer
            )
        {
            _xmlSerializer = xmlSerializer;
            _mardownSerializer = mardownSerializer;
        }

        private PostXmlSerializer _xmlSerializer;
        private PostMarkdownSerializer _mardownSerializer;

        public string ExpectedFileExtension { get; } = "";

        public string Serialize(Post post)
        {
            if(post.ContentType == "markdown")
            {
                return _mardownSerializer.Serialize(post);
            }

            return _xmlSerializer.Serialize(post);
        }

        public Post Deserialize(string input, string key)
        {
            if(input.StartsWith("---"))
            {
                return _mardownSerializer.Deserialize(input, key);
            }

            return _xmlSerializer.Deserialize(input, key);
        }

    }
}
