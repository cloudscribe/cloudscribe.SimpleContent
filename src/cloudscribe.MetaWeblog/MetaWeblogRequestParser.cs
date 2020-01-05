// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-04
// Last Modified:           2016-02-17
// 

using cloudscribe.MetaWeblog.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace cloudscribe.MetaWeblog
{
    public class MetaWeblogRequestParser : IMetaWeblogRequestParser
    {
        private ILogger log = null;
        private XDocument postedXml;
        private List<XElement> inputParams = null;

        public MetaWeblogRequestParser(ILogger logger = null)
        {
            log = logger;
           
        }

        public MetaWeblogRequest ParseRequest(XDocument postedDocument)
        {
            //postedXml = XDocument.Load(context.Request.Body, LoadOptions.None);
            postedXml = postedDocument;

            MetaWeblogRequest result = new MetaWeblogRequest();

            var methodCallElement = postedXml.Document.Element("methodCall");
            //if (rootElement == null) throw new HttpException(400, @"The ""methodCall"" element is missing from the XML-RPC request body.");
            var methodNameElement = methodCallElement.Element("methodName");
            result.MethodName = methodNameElement.Value;
            var paramsElement = methodCallElement.Elements("params");
            inputParams = paramsElement.Descendants("param").ToList<XElement>();
            //inputParams.

            // Determine what params are what by method name
            switch (result.MethodName)
            {
                case "system.listMethods":
                case "wpcom.getFeatures":
                    //do nothing
                    break;

                case "metaWeblog.newPost":
                    result.BlogId = inputParams[0].Descendants("string").SingleOrDefault().Value;
                    result.UserName = inputParams[1].Descendants("string").SingleOrDefault().Value;
                    result.Password = inputParams[2].Descendants("string").SingleOrDefault().Value;
                    result.Post = GetPost(inputParams[3]);
                    if (inputParams.Count > 4)
                    {
                        var p = inputParams[4].Descendants("boolean").SingleOrDefault().Value;
                        result.Publish = p != "0" && p != "false";
                    }
                    else
                    {
                        result.Publish = GetIsPublished(inputParams[3]);
                    }

                    break;
                case "metaWeblog.editPost":
                    result.PostId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    result.Post = GetPost(inputParams[3]);
                    //this.Publish = this.inputParams[4].InnerText != "0" && this.inputParams[4].InnerText != "false";
                    if (inputParams.Count > 4)
                    {
                        var p = inputParams[4].Descendants("boolean").SingleOrDefault().Value;
                        result.Publish = p != "0" && p != "false";
                    }
                    else
                    {
                        result.Publish = GetIsPublished(inputParams[3]);
                    }

                    break;
                case "metaWeblog.getPost":
                    result.PostId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    break;
                case "metaWeblog.newMediaObject":
                case "wp.uploadFile":
                    result.BlogId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    result.MediaObject = GetMediaObject(inputParams[3]);
                    break;
                case "metaWeblog.getCategories":
                case "wp.getCategories":
                case "wp.getAuthors":
                case "wp.getPageList":
                case "wp.getPages":
                case "wp.getTags":
                    result.BlogId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    break;
                case "wp.newCategory":
                    result.BlogId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    result.Category = GetCategory(inputParams[3]);
                    break;
                case "metaWeblog.getRecentPosts":
                    result.BlogId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    result.NumberOfPosts = Int32.Parse(inputParams[3].Value, CultureInfo.InvariantCulture);
                    break;
                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                    result.AppKey = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    break;
                case "wp.getUsersBlogs":
                    result.UserName = inputParams[0].Value;
                    result.Password = inputParams[1].Value;
                    break;
                case "blogger.deletePost":
                    result.AppKey = inputParams[0].Descendants("string").SingleOrDefault().Value;
                    result.PostId = inputParams[1].Descendants("string").SingleOrDefault().Value;
                    result.UserName = inputParams[2].Descendants("string").SingleOrDefault().Value;
                    result.Password = inputParams[3].Descendants("string").SingleOrDefault().Value;
                    result.Publish = inputParams[4].Descendants("boolean").SingleOrDefault().Value != "0" 
                        && inputParams[4].Descendants("boolean").SingleOrDefault().Value != "false";

                    break;
                case "blogger.getUserInfo":
                    result.AppKey = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    break;
                case "wp.newPage":
                    result.BlogId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    result.Page = GetPage(inputParams[3]);
                    if (inputParams.Count > 4)
                    {
                        var p = inputParams[4].Descendants("boolean").SingleOrDefault().Value;
                        result.Publish = p != "0" && p != "false";
                        //result.Publish = inputParams[4].Value != "0" && inputParams[4].Value != "false";
                    }


                    break;
                case "wp.getPage":
                    result.BlogId = inputParams[0].Value;
                    result.PageId = inputParams[1].Value;
                    result.UserName = inputParams[2].Value;
                    result.Password = inputParams[3].Value;
                    break;
                case "wp.editPage":
                    result.BlogId = inputParams[0].Value;
                    result.PageId = inputParams[1].Value;
                    result.UserName = inputParams[2].Value;
                    result.Password = inputParams[3].Value;
                    result.Page = GetPage(inputParams[4]);
                    if (inputParams.Count > 5)
                    {
                        var p = inputParams[5].Descendants("boolean").SingleOrDefault().Value;
                        result.Publish = p != "0" && p != "false";
                        
                    }
                    else
                    {
                        result.Publish = GetPublish(inputParams[4]);
                    }
                    //result. = inputParams[5].Value;

                    break;
                case "wp.deletePage":
                    result.BlogId = inputParams[0].Value;
                    result.UserName = inputParams[1].Value;
                    result.Password = inputParams[2].Value;
                    result.PageId = inputParams[3].Value;
                    break;
                default:
                    throw new MetaWeblogException("02", string.Format("Unknown Method. ({0})", result.MethodName));
            }
            
            return result;

        }

        private static bool GetIsPublished(XElement paramNode)
        {
            //XmlNode status = node.SelectSingleNode("value/struct/member[name='post_status']");
            //var status = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "post_status");
            var statusMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "post_status"
                     );


            if ((statusMember != null) && ((statusMember.LastNode as XElement).Value == "publish")) { return true; }

            return false;
        }

        private static bool GetPublish(XElement paramNode)
        {

            //var statusNode = node.SelectSingleNode("value/struct/member[name='page_status']");
            //var statusNode = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "page_status");
            var statusMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "page_status"
                     );

            if (statusMember == null)
            {
                throw new MetaWeblogException("06", "Page Struct Element, page_status, not Sent.");
            }

            string result = (statusMember.LastNode as XElement).Value;

            if (result == "publish") { return true; }

            if (result == "true") { return true; }

            if (result == "1") { return true; }

            return false;

        }

        private static string GetCategory(XElement paramNode)
        {
            //var categoryNode = node.SelectSingleNode("value/struct/member[name='name']");
            //var categoryNode = node.Descendants("member")
            //   .First(i => (string)i.Attribute("name") == "name");

            var categoryMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "name"
                     );

            if (categoryMember == null)
            {
                throw new MetaWeblogException("06", "Category Struct Element, name, not Sent.");
            }

            return (categoryMember.LastNode as XElement).Value;
        }

        /// <summary>
        /// Creates a Metaweblog Post object from the XML struct
        /// </summary>
        /// <param name="paramNode">
        /// XML contains a Metaweblog Post Struct
        /// </param>
        /// <returns>
        /// Metaweblog Post Struct Obejct
        /// </returns>
        private static PostStruct GetPost(XElement paramNode)
        {
            var temp = new PostStruct();

            // Require Title and Description
            //var title = node.SelectSingleNode("value/struct/member[name='title']");
            
            var titleMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "title"
                      );

            if (titleMember == null)
            {
                throw new MetaWeblogException("05", "Page Struct Element, Title, not Sent.");
            }



            //temp.title = title.LastChild.InnerText;
            temp.title = titleMember.Descendants("string").FirstOrDefault().Value;

            //var description = node.SelectSingleNode("value/struct/member[name='description']");
           
            var descriptionMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "description"
                      );

            if (descriptionMember == null)
            {
                throw new MetaWeblogException("05", "Page Struct Element, Description, not Sent.");
            }

            //temp.description = description.LastChild.InnerText;
            temp.description = descriptionMember.Descendants("string").FirstOrDefault().Value;

            //var link = node.SelectSingleNode("value/struct/member[name='link']");
            
            var linkMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "link"
                      );

            //temp.link = link == null ? string.Empty : link.LastChild.InnerText;
            temp.link = linkMember == null ? string.Empty : linkMember.Descendants("string").FirstOrDefault().Value;

            //var allowComments = node.SelectSingleNode("value/struct/member[name='mt_allow_comments']");
            //temp.commentPolicy = allowComments == null ? string.Empty : allowComments.LastChild.InnerText;
            
            var allowCommentsMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "mt_allow_comments"
                      );

            //temp.commentPolicy = allowComments == null ? string.Empty : (allowComments.LastNode as XElement).Value;
            temp.commentPolicy = allowCommentsMember == null ? string.Empty : (allowCommentsMember.LastNode as XElement).Value;

            //var excerpt = node.SelectSingleNode("value/struct/member[name='mt_excerpt']");
            //temp.excerpt = excerpt == null ? string.Empty : excerpt.LastChild.InnerText;

           
            var excerptMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "mt_excerpt"
                      );

            temp.excerpt = excerptMember == null ? string.Empty : (excerptMember.LastNode as XElement).Value;

            //var slug = node.SelectSingleNode("value/struct/member[name='wp_slug']");
            //temp.slug = slug == null ? string.Empty : slug.LastChild.InnerText;

            //var slug = paramNode.Descendants("member")
            //   .First(i => (string)i.Attribute("name") == "wp_slug");

            var slugMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "wp_slug"
                      );

            temp.slug = slugMember == null ? string.Empty : (slugMember.LastNode as XElement).Value;

            //var authorId = node.SelectSingleNode("value/struct/member[name='wp_author_id']");
            //temp.author = authorId == null ? string.Empty : authorId.LastChild.InnerText;
            
            var authorMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "wp_author_id"
                      );

            temp.author = authorMember == null ? string.Empty : (authorMember.LastNode as XElement).Value;

            var cats = new List<string>();
            //var categories = node.SelectSingleNode("value/struct/member[name='categories']");
            
            var categoriesMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "categories"
                      );

            if (categoriesMember != null)
            {
                var categoryArray = categoriesMember.LastNode as XElement;
                //var categoryArrayNodes = categoryArray.SelectNodes("array/data/value/string");
                var categoryArrayNodes = categoryArray.Descendants("string");

                if (categoryArrayNodes != null)
                {
                    cats.AddRange(categoryArrayNodes.Cast<XElement>().Select(
                        catnode => catnode.Value));
                }
            }

            temp.categories = cats;

            // postDate has a few different names to worry about
            //var dateCreated = node.SelectSingleNode("value/struct/member[name='dateCreated']");
            
            var dateCreatedMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "dateCreated"
                      );

            //var pubDate = node.SelectSingleNode("value/struct/member[name='pubDate']");
            
            var pubDateMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "pubDate"
                      );

            if (dateCreatedMember != null)
            {
                try
                {
                    var tempDate = (dateCreatedMember.LastNode as XElement).Value;
                    temp.postDate = DateTime.ParseExact(
                        tempDate,
                        "yyyyMMdd'T'HH':'mm':'ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal);
                }
                catch (Exception ex)
                {
                    // Ignore PubDate Error
                    Debug.WriteLine(ex.Message);
                }
            }

            else if (pubDateMember != null)
            {
                try
                {
                    var tempPubDate = (pubDateMember.LastNode as XElement).Value;
                    temp.postDate = DateTime.ParseExact(
                        tempPubDate,
                        "yyyyMMdd'T'HH':'mm':'ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal);
                }
                catch (Exception ex)
                {
                    // Ignore PubDate Error
                    Debug.WriteLine(ex.Message);
                }
            }

            // WLW tags implementation using mt_keywords
            var tags = new List<string>();
            
            var keyWordsMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "mt_keywords"
                      );

            if (keyWordsMember != null)
            {
                var tagsList = (keyWordsMember.LastNode as XElement).Value;
                foreach (var item in
                    tagsList.Split(',').Where(item => string.IsNullOrEmpty(tags.Find(t => t.Equals(item.Trim(), StringComparison.OrdinalIgnoreCase)))))
                {
                    tags.Add(item.Trim());
                }
            }

            temp.tags = tags;

            return temp;
        }

        private PageStruct GetPage(XElement paramNode)
        {
            var temp = new PageStruct();

            // Require Title and Description
            //var title = node.SelectSingleNode("value/struct/member[name='title']");
            //var title = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "title");
            var titleMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "title"
                     );

            if (titleMember == null)
            {
                throw new MetaWeblogException("06", "Page Struct Element, Title, not Sent.");
            }

            temp.title = (titleMember.LastNode as XElement).Value;

            //var description = node.SelectSingleNode("value/struct/member[name='description']");
            //var description = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "description");
            var descMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "description"
                     );

            if (descMember == null)
            {
                throw new MetaWeblogException("06", "Page Struct Element, Description, not Sent.");
            }

            temp.description = (descMember.LastNode as XElement).Value;

            //var link = node.SelectSingleNode("value/struct/member[name='link']");
            //if (link != null)
            //{
            //    temp.link = node.SelectSingleNode("value/struct/member[name='link']") == null ? null : link.LastChild.InnerText;
            //}

            //var link = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "link");
            var linkMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "link"
                     );

            temp.link = linkMember == null ? string.Empty : (linkMember.LastNode as XElement).Value;

            var slugMember = paramNode.Descendants("member")
             .FirstOrDefault(p =>
                    p.Element("name").Value == "wp_slug"
                    );

            temp.slug = slugMember == null ? string.Empty : (slugMember.LastNode as XElement).Value;

            //var dateCreated = node.SelectSingleNode("value/struct/member[name='dateCreated']");
            //var dateCreated = node.Descendants("member")
            //   .First(i => (string)i.Attribute("name") == "dateCreated");
            var dateCreatedMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "dateCreated"
                     );


            if (dateCreatedMember != null)
            {
                try
                {
                    var tempDate = (dateCreatedMember.LastNode as XElement).Value;
                    temp.pageDate = DateTime.ParseExact(
                        tempDate,
                        "yyyyMMdd'T'HH':'mm':'ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal);
                }
                catch (Exception ex)
                {
                    // Ignore PubDate Error
                    if (log != null)
                    {
                        log.LogError("swallowed pub date error", ex);
                    }
                    
                    
                }
            }

            // Keywords
            //var keywords = node.SelectSingleNode("value/struct/member[name='mt_keywords']");
            //temp.mt_keywords = keywords == null ? string.Empty : keywords.LastChild.InnerText;

            //var keywords = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "mt_keywords");
            var keywordsMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "mt_keywords"
                     );

            temp.mt_keywords = keywordsMember == null ? string.Empty : (keywordsMember.LastNode as XElement).Value;

            //var pageParentId = node.SelectSingleNode("value/struct/member[name='wp_page_parent_id']");
            //temp.pageParentID = pageParentId == null ? null : pageParentId.LastChild.InnerText;

            //var pageParentId = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "wp_page_parent_id");
            var parentIdMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "wp_page_parent_id"
                     );

            temp.pageParentId = parentIdMember == null ? null : (parentIdMember.LastNode as XElement).Value;

            //var pageOrder = node.SelectSingleNode("value/struct/member[name='wp_page_order']");
            //temp.pageOrder = pageOrder == null ? null : pageOrder.LastChild.InnerText;

            //var pageOrder = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "wp_page_order");

            var pageOrderMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "wp_page_order"
                     );

            temp.pageOrder = pageOrderMember == null ? null : (pageOrderMember.LastNode as XElement).Value;

            //var allowComments = node.SelectSingleNode("value/struct/member[name='mt_allow_comments']");
            //temp.commentPolicy = allowComments == null ? string.Empty : allowComments.LastChild.InnerText;
            //var allowComments = node.Descendants("member")
            //    .First(i => (string)i.Attribute("name") == "mt_allow_comments");

            var allowCommentsMember = paramNode.Descendants("member")
              .FirstOrDefault(p =>
                     p.Element("name").Value == "mt_allow_comments"
                     );

            temp.commentPolicy = allowCommentsMember == null ? string.Empty : (allowCommentsMember.LastNode as XElement).Value;

            return temp;
        }

        private static MediaObjectStruct GetMediaObject(XElement paramNode)
        {
            //var name = node.SelectSingleNode("value/struct/member[name='name']");
            //var type = node.SelectSingleNode("value/struct/member[name='type']");
            //var bits = node.SelectSingleNode("value/struct/member[name='bits']");
            
            var nameMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "name"
                      );
            
            var typeMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "type"
                      );

            var bitsMember = paramNode.Descendants("member")
               .FirstOrDefault(p =>
                      p.Element("name").Value == "bits"
                      );

            var temp = new MediaObjectStruct
            {
                name = nameMember == null ? string.Empty : nameMember.Descendants("string").FirstOrDefault().Value,
                type = typeMember == null ? "notsent" : typeMember.Descendants("string").FirstOrDefault().Value,
                bytes = Convert.FromBase64String(bitsMember == null ? string.Empty : bitsMember.Descendants("base64").FirstOrDefault().Value)
            };

            return temp;
        }




        


    }
}
