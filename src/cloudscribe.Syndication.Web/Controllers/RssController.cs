// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-01
// Last Modified:           2016-04-02
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
using cloudscribe.Syndication.Models.Rss;

namespace cloudscribe.Syndication.Web.Controllers
{
    [Route("api/[controller]")]
    public class RssController : Controller
    {
        public RssController(
            ILogger<RssController> logger,
            IEnumerable<IChannelProvider> channelProviders = null,
            IChannelProviderResolver channelResolver = null,
            IXmlFormatter xmlFormatter = null
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
            this.xmlFormatter = xmlFormatter ?? new DefaultXmlFormatter();

        }

        private ILogger log;
        private IChannelProviderResolver channelResolver;
        private IEnumerable<IChannelProvider> channelProviders;
        private IChannelProvider currentChannelProvider;
        private IXmlFormatter xmlFormatter;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            currentChannelProvider = channelResolver.GetCurrentChannelProvider(channelProviders);

            if (currentChannelProvider == null)
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            var currentChannel = await currentChannelProvider.GetChannel();

            if (currentChannel == null)
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }

            var xml = xmlFormatter.BuildXml(currentChannel, Url);

            return new XmlResult(xml);

        }

    }
}
