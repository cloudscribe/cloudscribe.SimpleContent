// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-02
// Last Modified:           2016-04-04
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Syndication.Models.Rss;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;

namespace cloudscribe.SimpleContent.Syndication
{
    public class RssChannelProvider
    {
        public RssChannelProvider(
            IProjectService projectService,
            IBlogService blogService,
            IHttpContextAccessor contextAccessor,
            IUrlHelper urlHelper)
        {
            this.projectService = projectService;
            this.blogService = blogService;
            this.urlHelper = urlHelper;
        }

        private IUrlHelper urlHelper;
        private IHttpContextAccessor contextAccessor;
        private IProjectService projectService;
        private IBlogService blogService;
        private int maxFeedItems = 20;

        public string Name { get; } = "cloudscribe.SimpleContent.Syndication.RssChannelProvider";

        public async Task<RssChannel> GetChannel(CancellationToken cancellationToken = default(CancellationToken))
        {
            var project = await projectService.GetCurrentProjectSettings();
            if(project == null) { return null; }
            var posts = await blogService.GetRecentPosts(maxFeedItems);
            if(posts == null) { return null; }

            var channel = new RssChannel();
            channel.Title = project.Title;
            channel.Description = project.Description;
            channel.Copyright = project.CopyrightNotice;
            if(project.ChannelCategoriesCsv.Length > 0)
            {
                var channelCats = project.ChannelCategoriesCsv.Split(',');
                foreach(var cat in channelCats)
                {
                    channel.Categories.Add(new RssCategory(cat));
                }
            }
            
            channel.Generator = Name;
            if(project.Image.Length > 0)
            {
                channel.Image.Url = new Uri(urlHelper.Content(project.Image));
            }
            if(project.LanguageCode.Length > 0)
            {
                channel.Language = new CultureInfo(project.LanguageCode);
            }
             
            channel.Link = new Uri(urlHelper.RouteUrl(ProjectConstants.BlogIndexRouteName));
            if(project.ManagingEditorEmail.Length > 0)
            {
                channel.ManagingEditor = project.ManagingEditorEmail;
            }

            if(project.ChannelRating.Length > 0)
            {
                channel.Rating = project.ChannelRating;
            }
            
            
            var feedUrl = string.Concat(
                        contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        contextAccessor.HttpContext.Request.Host.ToUriComponent(),
                        contextAccessor.HttpContext.Request.PathBase.ToUriComponent(),
                        contextAccessor.HttpContext.Request.Path.ToUriComponent(),
                        contextAccessor.HttpContext.Request.QueryString.ToUriComponent());
            channel.SelfLink = new Uri(feedUrl);
            //channel.TextInput = 
            channel.TimeToLive = project.ChannelTimeToLive;
            if(project.WebmasterEmail.Length > 0)
            {
                channel.Webmaster = project.WebmasterEmail;
            }
           
            DateTime mostRecentPubDate = DateTime.MinValue;
            var items = new List<RssItem>();
            foreach(var post in posts)
            {
                if(post.PubDate > mostRecentPubDate) { mostRecentPubDate = post.PubDate; }
                var rssItem = new RssItem();
                rssItem.Author = post.Author;

                if(post.Categories.Count > 0)
                {
                    foreach(var c in post.Categories)
                    {
                        rssItem.Categories.Add(new RssCategory(c));
                    }
                }

                
                //rssItem.Comments
                rssItem.Description = post.Content;
                //rssItem.Enclosures
                rssItem.Guid.Value = post.Id;
                string postUrl;
                if(project.IncludePubDateInPostUrls)
                {
                    postUrl = urlHelper.RouteUrl(ProjectConstants.PostWithDateRouteName,
                        new
                        {
                            year = post.PubDate.Year,
                            month = post.PubDate.Month,
                            day = post.PubDate.Day,
                            slug = post.Slug
                        });
                }
                else
                {
                    postUrl = urlHelper.RouteUrl(ProjectConstants.PostWithoutDateRouteName, 
                        new { slug = post.Slug });
                }
                rssItem.Link = new Uri(postUrl);
                rssItem.PublicationDate = post.PubDate;
                //rssItem.Source
                rssItem.Title = post.Title;
               


                items.Add(rssItem);

            }

            channel.PublicationDate = mostRecentPubDate;
            channel.Items = items;

            return channel;
        }


    }
}
