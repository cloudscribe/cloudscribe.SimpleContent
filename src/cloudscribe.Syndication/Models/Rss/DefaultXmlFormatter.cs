// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-02
// Last Modified:           2016-04-07
// 



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNet.Mvc;

namespace cloudscribe.Syndication.Models.Rss
{
    public class DefaultXmlFormatter : IXmlFormatter
    {
        public DefaultXmlFormatter()
        {

        }

        public XDocument BuildXml(RssChannel channel, IUrlHelper urlHelper)
        {
            //TODO: improve and complete this
            //http://cyber.law.harvard.edu/rss/rss.html
            //http://cyber.law.harvard.edu/rss/examples/rss2sample.xml
            //http://www.mikesdotnetting.com/article/174/generating-rss-and-atom-feeds-in-webmatrix
            //http://www.rssboard.org/rss-profile

            var rssChannel = new XElement(Rss20Constants.ChannelTag,
                      new XElement(Rss20Constants.TitleTag, channel.Title),
                      new XElement(Rss20Constants.LinkTag, urlHelper.Content(channel.Link.ToString())),
                      new XElement(Rss20Constants.DescriptionTag, channel.Description),
                      new XElement(Rss20Constants.LanguageTag, channel.Language),
                      new XElement(Rss20Constants.PubDateTag, channel.PublicationDate.ToString("R")),
                      new XElement(Rss20Constants.DocsTag, "http://blogs.law.harvard.edu/tech/rss"),
                      new XElement(Rss20Constants.TtlTag, channel.TimeToLive)
                        );

            if(!string.IsNullOrEmpty(channel.ManagingEditor))
            {
                rssChannel.Add(new XElement(Rss20Constants.ManagingEditorTag, channel.ManagingEditor));
            }

            if (!string.IsNullOrEmpty(channel.Webmaster))
            {
                rssChannel.Add(new XElement(Rss20Constants.WebMasterTag, channel.Webmaster));
            }

            if (!(channel.Image == null))
            {
                rssChannel.Add(new XElement(Rss20Constants.ImageTag, channel.Image));
            }

            if (!string.IsNullOrEmpty(channel.Copyright))
            {
                rssChannel.Add(new XElement(Rss20Constants.CopyrightTag, channel.Copyright));
            }

            XNamespace ns = Rss20Constants.AtomNamespace;

            if (channel.SelfLink != null)
            {
                rssChannel.Add(
                    new XElement(ns + Rss20Constants.LinkTag, 
                    new XAttribute("href", channel.SelfLink),
                    new XAttribute("rel", "self"),
                    new XAttribute("type", "application/rss+xml")
                    ));
            }

            foreach (var item in channel.Items)
            {
                AddItem(rssChannel, item, urlHelper);
            }

            

            var rss = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(Rss20Constants.RssTag,
                  new XAttribute(Rss20Constants.VersionTag, Rss20Constants.Version),
                   new XAttribute(ns + "atom", Rss20Constants.AtomNamespace)
                  //new XAttribute("xmlns",ns)
                  ,
                  rssChannel
                )
              );

            return rss;

        }

        private void AddItem(
            XElement channel, 
            RssItem item, 
            IUrlHelper urlHelper
            )
        {
            var rssItem = new XElement(Rss20Constants.ItemTag,
                            new XElement(Rss20Constants.TitleTag, item.Title),
                            new XElement(Rss20Constants.DescriptionTag, item.Description),
                            new XElement(Rss20Constants.LinkTag, urlHelper.Content(item.Link.ToString())),
                            new XElement(Rss20Constants.GuidTag, urlHelper.Content(item.Link.ToString())),
                            new XElement(Rss20Constants.PubDateTag, item.PublicationDate.ToString("R"))
                           );

            if (item.Categories.Count > 0)
            {
                foreach (var cat in item.Categories)
                {
                    var ele = new XElement(Rss20Constants.CategoryTag, cat.Value);
                    rssItem.Add(ele);
                }
            }

            channel.Add(rssItem);
        }

    }
}
