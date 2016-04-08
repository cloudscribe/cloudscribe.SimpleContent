// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-02
// Last Modified:           2016-04-08
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

        public XDocument BuildXml(RssChannel channel
            //, IUrlHelper urlHelper
            )
        {
            //http://cyber.law.harvard.edu/rss/rss.html
            //http://cyber.law.harvard.edu/rss/examples/rss2sample.xml
            //http://www.mikesdotnetting.com/article/174/generating-rss-and-atom-feeds-in-webmatrix
            //http://www.rssboard.org/rss-profile

            var rssChannel = new XElement(Rss20Constants.ChannelTag,
                      new XElement(Rss20Constants.TitleTag, channel.Title),
                      new XElement(Rss20Constants.LinkTag, channel.Link.ToString()),
                      new XElement(Rss20Constants.DescriptionTag, channel.Description),
                      new XElement(Rss20Constants.LanguageTag, channel.Language),
                      new XElement(Rss20Constants.PubDateTag, channel.PublicationDate.ToString("R")),
                      new XElement(Rss20Constants.DocsTag, Rss20Constants.SpecificationLink),
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

            // image is optional
            // The image MUST contain three child elements: link, title and url. It also MAY contain three 
            // OPTIONAL elements: description, height and width.
            if (
                !(channel.Image == null) 
                && (channel.Image.Url != null)
                && (channel.Image.Link != null)
                && (!string.IsNullOrEmpty(channel.Image.Title))
                )
            {
                var imageElement = new XElement(Rss20Constants.ImageTag,
                    new XElement(Rss20Constants.LinkTag, channel.Image.Link),
                    new XElement(Rss20Constants.TitleTag, channel.Image.Title),
                    new XElement(Rss20Constants.UrlTag, channel.Image.Url)

                    );
                
                if (!string.IsNullOrEmpty(channel.Image.Description))
                {
                    imageElement.Add(new XElement(Rss20Constants.DescriptionTag, channel.Image.Description));
                }

                // The image's height element contains the height, in pixels, of the image (OPTIONAL). 
                // The image MUST be no taller than 400 pixels. If this element is omitted, the image is 
                // assumed to be 31 pixels tall.
                if (channel.Image.Height > int.MinValue)
                {
                    imageElement.Add(new XElement(Rss20Constants.HeightTag, channel.Image.Height));
                }

                //The image's width element contains the width, in pixels, of the image (OPTIONAL). The image MUST be no wider than 144 pixels. 
                // If this element is omitted, the image is assumed to be 88 pixels wide.
                if (channel.Image.Width > int.MinValue)
                {
                    imageElement.Add(new XElement(Rss20Constants.WidthTag, channel.Image.Width));
                }

                rssChannel.Add(imageElement);
            }

            if (!string.IsNullOrEmpty(channel.Copyright))
            {
                rssChannel.Add(new XElement(Rss20Constants.CopyrightTag, channel.Copyright));
            }

            if (!string.IsNullOrEmpty(channel.Generator))
            {
                rssChannel.Add(new XElement(Rss20Constants.GeneratorTag, channel.Generator));
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
                AddItem(rssChannel, item);
            }
            
            var rss = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(Rss20Constants.RssTag,
                  new XAttribute(Rss20Constants.VersionTag, Rss20Constants.Version),
                  new XAttribute(XNamespace.Xmlns + "atom", ns)   
                  ,
                  rssChannel
                )
              );

            return rss;

        }

        private void AddItem(
            XElement channel, 
            RssItem item
            )
        {
            var rssItem = new XElement(Rss20Constants.ItemTag,
                            new XElement(Rss20Constants.TitleTag, item.Title),
                            new XElement(Rss20Constants.DescriptionTag, item.Description),
                            new XElement(Rss20Constants.LinkTag, item.Link.ToString()),
                            new XElement(Rss20Constants.GuidTag, item.Link.ToString()),
                            new XElement(Rss20Constants.PubDateTag, item.PublicationDate.ToString("R"))
                           );

           

            // author: A feed published by an individual SHOULD omit this element and use the managingEditor or 
            // webMaster channel elements to provide contact information.

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
