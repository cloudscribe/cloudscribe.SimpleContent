// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-11-21
// Last Modified:           2017-11-21
//

using cloudscribe.SimpleContent.Models;
using NoDb;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PageCompositeSerializer : IStringSerializer<Page>
    {
        public PageCompositeSerializer(
            PageJsonSerializer jsonSerializer,
            PageMarkdownSerializer markdownSerializer
            )
        {
            _jsonSerializer = jsonSerializer;
            _markdownSerializer = markdownSerializer;
        }

        private PageJsonSerializer _jsonSerializer;
        private PageMarkdownSerializer _markdownSerializer;

        public string ExpectedFileExtension { get; } = "";

        public string Serialize(Page page)
        {
            if (page.ContentType == "markdown")
            {
                return _markdownSerializer.Serialize(page);
            }

            return _jsonSerializer.Serialize(page);
        }

        public Page Deserialize(string input, string key)
        {
            if (input.StartsWith("---"))
            {
                return _markdownSerializer.Deserialize(input, key);
            }

            return _jsonSerializer.Deserialize(input, key);
        }
    }
}
