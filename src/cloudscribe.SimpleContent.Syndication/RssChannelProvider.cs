// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-02
// Last Modified:           2018-06-30
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.Syndication.Models.Rss;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Syndication
{
    public class RssChannelProvider : IChannelProvider
    {
        public RssChannelProvider(
            IProjectService projectService,
            IBlogService blogService,
            IBlogUrlResolver blogUrlResolver,
            IBlogRoutes blogRoutes,
            IHttpContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IContentProcessor contentProcessor,
            IStringLocalizer<RssChannelProvider> localizer
            )
        {
            ProjectService = projectService;
            BlogService = blogService;
            BlogUrlResolver = blogUrlResolver;
            ContextAccessor = contextAccessor;
            UrlHelperFactory = urlHelperFactory;
            ActionContextAccesor = actionContextAccesor;
            ContentProcessor = contentProcessor;
            BlogRoutes = blogRoutes;
            sr = localizer;
        }

        protected IUrlHelperFactory UrlHelperFactory { get; private set; }
        protected IActionContextAccessor ActionContextAccesor { get; private set; }
        protected IHttpContextAccessor ContextAccessor { get; private set; }
        protected IProjectService ProjectService { get; private set; }
        protected IBlogService BlogService { get; private set; }
        protected IBlogRoutes BlogRoutes { get; private set; }
        protected IContentProcessor ContentProcessor { get; private set; }
        protected IBlogUrlResolver BlogUrlResolver { get; private set; }
        protected IStringLocalizer sr { get; private set; }

        public string Name { get; } = "cloudscribe.SimpleContent.Syndication.RssChannelProvider";

        public virtual async Task<RssChannel> GetChannel(CancellationToken cancellationToken = default(CancellationToken))
        {

            var project = await ProjectService.GetCurrentProjectSettings();
            if(project == null) { return null; }

            var itemsToGet = project.DefaultFeedItems;
            var requestedFeedItems = ContextAccessor.HttpContext?.Request.Query["maxItems"].ToString();
            if(!string.IsNullOrWhiteSpace(requestedFeedItems))
            {
                int.TryParse(requestedFeedItems, out itemsToGet);
                if(itemsToGet > project.MaxFeedItems) { itemsToGet = project.MaxFeedItems; }
            }
            
            var posts = await BlogService.GetRecentPosts(itemsToGet);
            if(posts == null) { return null; }

            var channel = new RssChannel
            {
                Title = project.Title,
                Copyright = project.CopyrightNotice,
                Generator = Name,
                RemoteFeedUrl = project.RemoteFeedUrl,
                RemoteFeedProcessorUseAgentFragment = project.RemoteFeedProcessorUseAgentFragment
            };
            if (!string.IsNullOrEmpty(project.Description))
            {
                channel.Description = project.Description;
            }
            else
            {
                // prevent error, channel desc cannot be empty
                channel.Description = sr["Welcome to my blog"];
            }
            
            if(!string.IsNullOrEmpty(project.ChannelCategoriesCsv))
            {
                var channelCats = project.ChannelCategoriesCsv.Split(',');
                foreach(var cat in channelCats)
                {
                    channel.Categories.Add(new RssCategory(cat));
                }
            }
            
            var urlHelper = UrlHelperFactory.GetUrlHelper(ActionContextAccesor.ActionContext);

            if (!string.IsNullOrEmpty(project.Image))
            {
                channel.Image.Url = new Uri(urlHelper.Content(project.Image));
            }
            if(!string.IsNullOrEmpty(project.LanguageCode))
            {
                channel.Language = new CultureInfo(project.LanguageCode);
            }

            var baseUrl = string.Concat(
                        ContextAccessor.HttpContext.Request.Scheme,
                        "://",
                        ContextAccessor.HttpContext.Request.Host.ToUriComponent()
                        );

            // asp.net bug? the comments for this method say it returns an absolute fully qualified url but it returns relative
            // looking at latest code seems ok so maybe just a bug in rc1
            //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/UrlHelperExtensions.cs
            //https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/Routing/UrlHelper.cs

            var indexUrl = urlHelper.RouteUrl(BlogRoutes.BlogIndexRouteName);
            if(indexUrl == null)
            {
                indexUrl = urlHelper.Action("Index", "Blog");
            }
            
            if (indexUrl.StartsWith("/"))
            {
                indexUrl = string.Concat(baseUrl, indexUrl);
            }
            channel.Link = new Uri(indexUrl);
            if(!string.IsNullOrEmpty(project.ManagingEditorEmail))
            {
                channel.ManagingEditor = project.ManagingEditorEmail;
            }

            if(!string.IsNullOrEmpty(project.ChannelRating))
            {
                channel.Rating = project.ChannelRating;
            }
            
            var feedUrl = string.Concat(
                        ContextAccessor.HttpContext.Request.Scheme,
                        "://",
                        ContextAccessor.HttpContext.Request.Host.ToUriComponent(),
                        ContextAccessor.HttpContext.Request.PathBase.ToUriComponent(),
                        ContextAccessor.HttpContext.Request.Path.ToUriComponent(),
                        ContextAccessor.HttpContext.Request.QueryString.ToUriComponent());
            channel.SelfLink = new Uri(feedUrl);

            channel.TimeToLive = project.ChannelTimeToLive;
            if(!string.IsNullOrEmpty(project.WebmasterEmail))
            {
                channel.Webmaster = project.WebmasterEmail;
            }
           
            DateTime mostRecentPubDate = DateTime.MinValue;
            var items = new List<RssItem>();
            foreach(var post in posts)
            {
                if(!post.PubDate.HasValue) { continue; }

                if(post.PubDate.Value > mostRecentPubDate) { mostRecentPubDate = post.PubDate.Value; }
                var rssItem = new RssItem
                {
                    Author = post.Author
                };

                if (post.Categories.Count > 0)
                {
                    foreach(var c in post.Categories)
                    {
                        rssItem.Categories.Add(new RssCategory(c));
                    }
                }

                var postUrl = await BlogUrlResolver.ResolvePostUrl(post, project).ConfigureAwait(false);

                if (string.IsNullOrEmpty(postUrl))
                {
                    //TODO: log 
                    continue;
                }

                if (postUrl.StartsWith("/"))
                {
                    //postUrl = urlHelper.Content(postUrl);
                    postUrl = string.Concat(
                        ContextAccessor.HttpContext.Request.Scheme,
                        "://",
                        ContextAccessor.HttpContext.Request.Host.ToUriComponent(),
                        postUrl);
                }

                var filteredResult = ContentProcessor.FilterHtmlForRss(post, project, baseUrl);
                rssItem.Description = filteredResult.FilteredContent;
                if(!filteredResult.IsFullContent)
                {
                    var readMore = " <a href='" + postUrl + "'>" + sr["...read more"] + "</a>";
                    rssItem.Description += readMore;
                }
                
                //rssItem.Enclosures
                
                rssItem.Guid = new RssGuid(postUrl,true);
                rssItem.Link = new Uri(postUrl);
                rssItem.PublicationDate = post.PubDate.Value;
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
