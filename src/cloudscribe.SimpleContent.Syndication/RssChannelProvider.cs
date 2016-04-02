// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-02
// Last Modified:           2016-04-02
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Syndication.Models.Rss;

namespace cloudscribe.SimpleContent.Syndication
{
    public class RssChannelProvider
    {
        public RssChannelProvider(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        private IBlogService blogService;
        private int maxFeedItems = 20;

        public string Name { get; } = "cloudscribe.SimpleContent.Syndication.RssChannelProvider";

        public async Task<RssChannel> GetChannel(CancellationToken cancellationToken = default(CancellationToken))
        {
            var posts = await blogService.GetRecentPosts(maxFeedItems);
            if(posts == null) { return null; }

            var channel = new RssChannel();


            return channel;
        }


    }
}
