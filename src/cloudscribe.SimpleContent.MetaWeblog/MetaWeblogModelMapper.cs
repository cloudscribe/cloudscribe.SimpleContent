// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-03-08
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
            //this.commentPolicy = commentPolicy;
           // this.urlResolver = urlResolver;
        }

        //private IPostCommentPolicyResolver commentPolicy;
        //private IPostUrlResolver urlResolver;
        
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
            Post post, 
            string postUrl,
            bool commentsOpen
            )
        {
            PostStruct p = new PostStruct();

            p.author = post.Author;
            p.categories = post.Categories;
            p.commentPolicy = commentsOpen ? "1" : "0";
            p.description = post.Content;
            p.excerpt = post.MetaDescription;
            p.link = postUrl;
            p.postDate = post.PubDate;
            p.postId = post.Id;
            p.publish = post.IsPublished;
            p.slug = post.Slug;
            p.title = post.Title;
           
            
            return p;
        }

        public BlogInfoStruct GetStructFromBlog(ProjectSettings blog, string blogUrl)
        {
            BlogInfoStruct b = new BlogInfoStruct();
            b.blogId = blog.Id;
            b.blogName = blog.Title;
            b.url = blogUrl;

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
            Page page,
            string postUrl,
            bool commentsOpen
            )
        {
            var p = new PageStruct();

            p.commentPolicy = commentsOpen ? "1" : "0";
            p.description = page.Content;
            p.link = postUrl;
            
            p.pageUtcDate = page.PubDate;
            p.pageDate = page.PubDate;
            p.pageId = page.Id;
            p.pageOrder = page.PageOrder.ToString(CultureInfo.InvariantCulture);
            p.pageParentId = page.ParentId;
            p.title = page.Title;
            p.parentTitle = page.ParentTitle;


            return p;
        }

    }
}
