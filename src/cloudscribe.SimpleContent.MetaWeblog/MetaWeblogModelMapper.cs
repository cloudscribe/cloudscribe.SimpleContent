// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2018-07-09
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.MetaWeblog.Models;
using System;
using System.Globalization;

namespace cloudscribe.SimpleContent.MetaWeblog
{
    public class MetaWeblogModelMapper
    {
        public MetaWeblogModelMapper()
        {
           
        }
        
        public Post GetPostFromStruct(PostStruct postStruct)
        {
            Post p = new Post();
            p.Author = postStruct.author;
            p.Categories = postStruct.categories;
            p.Content = postStruct.description;
            p.MetaDescription = postStruct.excerpt;
            p.Id = postStruct.postId;
            p.IsPublished = postStruct.publish;
            if(postStruct.postDate != null)
            {
                p.PubDate = postStruct.postDate;
            }
            
            p.Slug = postStruct.slug;
            p.Title = postStruct.title;
          
            return p;
        }

        public PostStruct GetStructFromPost(
            IPost post, 
            string postUrl,
            bool commentsOpen
            )
        {
            PostStruct p = new PostStruct();

            p.author = post.Author;
            p.categories = post.Categories;
            p.commentPolicy = commentsOpen ? "1" : "0";
            if(!string.IsNullOrWhiteSpace(post.DraftContent))
            {
                p.description = post.DraftContent;
            }
            else
            {
                p.description = post.Content;
            }
            
            p.excerpt = post.MetaDescription;
            p.link = postUrl;
            if(post.PubDate.HasValue)
            {
                p.postDate = post.PubDate.Value;
            }
            
            p.postId = post.Id;
            p.publish = post.IsPublished;
            p.slug = post.Slug;
            p.title = post.Title;
           
            
            return p;
        }

        public BlogInfoStruct GetStructFromBlog(IProjectSettings blog, string blogUrl)
        {
            BlogInfoStruct b = new BlogInfoStruct
            {
                blogId = blog.Id,
                blogName = blog.Title,
                url = blogUrl
            };

            return b;

        }

        public Page GetPageFromStruct(PageStruct pageStruct)
        {
            var p = new Page();
            p.Content = pageStruct.description;
            p.Title = pageStruct.title;
            p.Id = pageStruct.pageId;
            p.ParentId = pageStruct.pageParentId;
           // p.IsPublished = pageStruct.published;
            if (pageStruct.pageUtcDate != null)
            {
                p.PubDate = pageStruct.pageUtcDate;
            }

            p.Slug = pageStruct.link;
            if(!string.IsNullOrEmpty(pageStruct.pageOrder))
            {
                p.PageOrder = Convert.ToInt32(pageStruct.pageOrder);
            }

            return p;

        }

        public PageStruct GetStructFromPage(
            IPage page,
            string postUrl,
            bool commentsOpen
            )
        {
            var p = new PageStruct();

            p.commentPolicy = commentsOpen ? "1" : "0";

            if(!string.IsNullOrWhiteSpace(page.DraftContent))
            {
                p.description = page.DraftContent;
            }
            else
            {
                p.description = page.Content;
            }
            
            p.link = postUrl;
            
            if(page.PubDate.HasValue)
            {
                p.pageUtcDate = page.PubDate.Value;
                p.pageDate = page.PubDate.Value;
            }
            
            p.pageId = page.Id;
            p.pageOrder = page.PageOrder.ToString(CultureInfo.InvariantCulture);
            p.pageParentId = page.ParentId;
            p.title = page.Title;
            p.parentTitle = page.ParentSlug;
            
            return p;
        }

    }
}
