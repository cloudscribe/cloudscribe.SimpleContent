// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-11-20
// Last Modified:           2017-11-20
// 


using cloudscribe.SimpleContent.Models;
using NoDb;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PostCompositeSerializer : IStringSerializer<Post>
    {
        public PostCompositeSerializer(
            PostXmlSerializer xmlSerializer,
            PostMarkdownSerializer markdownSerializer
            )
        {
            _xmlSerializer = xmlSerializer;
            _markdownSerializer = markdownSerializer;
        }

        private PostXmlSerializer _xmlSerializer;
        private PostMarkdownSerializer _markdownSerializer;

        public string ExpectedFileExtension { get; } = "";

        public string Serialize(Post post)
        {
            if(post.ContentType == "markdown")
            {
                return _markdownSerializer.Serialize(post);
            }

            return _xmlSerializer.Serialize(post);
        }

        public Post Deserialize(string input, string key)
        {
            if(input.StartsWith("---"))
            {
                return _markdownSerializer.Deserialize(input, key);
            }

            return _xmlSerializer.Deserialize(input, key);
        }

    }
}
