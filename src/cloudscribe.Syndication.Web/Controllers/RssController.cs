// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-01
// Last Modified:           2016-04-01
// 


using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.Xml.Linq;
using cloudscribe.SimpleContent.Syndication;
using cloudscribe.Syndication.Models.Rss;

namespace cloudscribe.Syndication.Web.Controllers
{
    [Route("api/[controller]")]
    public class RssController : Controller
    {
        public RssController(
            ILogger<RssController> logger,
            IEnumerable<IChannelProvider> channelProviders = null,
            IChannelProviderResolver channelResolver = null
            )
        {
            log = logger;
            this.channelProviders = channelProviders ?? new List<IChannelProvider>();
            if (channelProviders is List<IChannelProvider>)
            {
                var list = channelProviders as List<IChannelProvider>;
                if (list.Count == 0)
                {
                    list.Add(new NullChannelProvider());
                }
            }

            this.channelResolver = channelResolver ?? new DefaultChannelProviderResolver();

        }

        private ILogger log;
        private IChannelProviderResolver channelResolver;
        private IEnumerable<IChannelProvider> channelProviders;
        private IChannelProvider currentChannelProvider;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            currentChannelProvider = channelResolver.GetCurrentChannelProvider(channelProviders);

            if (currentChannelProvider == null)
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            RssChannel currentChannel = await currentChannelProvider.GetChannel();

            if (currentChannel == null)
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            var xml = BuildXml(currentChannel);

            return new XmlResult(xml);

        }

        private XDocument BuildXml(RssChannel channel)
        {
            //TODO: is this sufficient or incomplete?

            var rss = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(Rss20Constants.RssTag,
                  new XAttribute(Rss20Constants.VersionTag, Rss20Constants.Version),
                    new XElement(Rss20Constants.ChannelTag,
                      new XElement(Rss20Constants.TitleTag, channel.Title),
                      new XElement(Rss20Constants.LinkTag, Url.Content(channel.Link.ToString()),
                      new XElement(Rss20Constants.DescriptionTag, channel.Description),
                      new XElement(Rss20Constants.CopyrightTag, channel.Copyright),
                        channel.Items.Select(item =>
                            new XElement(Rss20Constants.ItemTag,
                            new XElement(Rss20Constants.TitleTag, item.Title),
                            new XElement(Rss20Constants.DescriptionTag, item.Description),
                            new XElement(Rss20Constants.LinkTag, Url.Content(item.Link.ToString())),
                            new XElement(Rss20Constants.PubDateTag, item.PublicationDate.ToString("R"))
                           //,item.Categories.Select(cat => 
                           //  new XElement(Rss20Constants.CategoryTag, cat.Value)
                           // )
                           )
                        )
                      )
                    )
                )
              );

            return rss;



        }

    }
}
