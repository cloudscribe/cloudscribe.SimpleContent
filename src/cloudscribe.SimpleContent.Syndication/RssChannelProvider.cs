// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-02
// Last Modified:           2016-08-23
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Syndication.Models.Rss;
using cloudscribe.SimpleContent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace cloudscribe.SimpleContent.Syndication
{
    public class RssChannelProvider : IChannelProvider
    {
        public RssChannelProvider(
            IProjectService projectService,
            IBlogService blogService,
            IHttpContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            HtmlProcessor htmlProcessor)
        {
            this.projectService = projectService;
            this.blogService = blogService;
            this.contextAccessor = contextAccessor;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccesor;
            this.htmlProcessor = htmlProcessor;
        }

        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionContextAccesor;
        private IHttpContextAccessor contextAccessor;
        private IProjectService projectService;
        private IBlogService blogService;
        private HtmlProcessor htmlProcessor;
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
            if(!string.IsNullOrEmpty(project.Description))
            {
                channel.Description = project.Description;
            }
            else
            {
                // prevent error, channel desc cannot be empty
                channel.Description = "Welcome to my blog";
            }
            
            channel.Copyright = project.CopyrightNotice;
            if(!string.IsNullOrEmpty(project.ChannelCategoriesCsv))
            {
                var channelCats = project.ChannelCategoriesCsv.Split(',');
                foreach(var cat in channelCats)
                {
                    channel.Categories.Add(new RssCategory(cat));
                }
            }
            
            channel.Generator = Name;
            channel.RemoteFeedUrl = project.RemoteFeedUrl;
            channel.RemoteFeedProcessorUseAgentFragment = project.RemoteFeedProcessorUseAgentFragment;


            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);

            if (project.Image.Length > 0)
            {
                channel.Image.Url = new Uri(urlHelper.Content(project.Image));
            }
            if(project.LanguageCode.Length > 0)
            {
                channel.Language = new CultureInfo(project.LanguageCode);
            }

            var baseUrl = string.Concat(
                        contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        contextAccessor.HttpContext.Request.Host.ToUriComponent()
                        );

            // asp.net bug? the comments for this method say it returns an absolute fully qualified url but it returns relative
            // looking at latest code seems ok so maybe just a bug in rc1
            //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/UrlHelperExtensions.cs
            //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/Routing/UrlHelper.cs

            var indexUrl = urlHelper.RouteUrl(ProjectConstants.BlogIndexRouteName);
            
            if (indexUrl.StartsWith("/"))
            {
                indexUrl = string.Concat(baseUrl, indexUrl);
            }
            channel.Link = new Uri(indexUrl);
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
                if(project.UseMetaDescriptionInFeed)
                {
                    rssItem.Description = post.MetaDescription;
                }
                else
                {
                    // change relative urls in content to absolute
                    rssItem.Description = htmlProcessor.ConvertUrlsToAbsolute(baseUrl, post.Content);
                }
                
                //rssItem.Enclosures
               
                var postUrl = await blogService.ResolvePostUrl(post);
                
                if(string.IsNullOrEmpty(postUrl))
                {
                    //TODO: log 
                    continue;
                }

                
                if (postUrl.StartsWith("/"))
                {
                    //postUrl = urlHelper.Content(postUrl);
                    postUrl = string.Concat(
                        contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        contextAccessor.HttpContext.Request.Host.ToUriComponent(),
                        postUrl);
                }

                rssItem.Guid = new RssGuid(postUrl,true);
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
