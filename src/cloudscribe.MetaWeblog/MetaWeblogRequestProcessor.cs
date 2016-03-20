// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-04
// Last Modified:           2016-02-24
// 

using cloudscribe.MetaWeblog.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public class MetaWeblogRequestProcessor : IMetaWeblogRequestProcessor
    {
        
        public MetaWeblogRequestProcessor(
            IMetaWeblogService metaWeblogService)
        {
            service = metaWeblogService;
            
        }

        private IMetaWeblogService service;
        
        public async Task<MetaWeblogResult> ProcessRequest(
            MetaWeblogRequest input,
            MetaWeblogSecurityResult permission,
            CancellationToken cancellationToken)
        {
            
            MetaWeblogResult output = new MetaWeblogResult();
            output.Method = input.MethodName;
            
            switch (input.MethodName)
            {
                case "metaWeblog.newPost":
                    output.PostId = await service.NewPost(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.Post, 
                        input.Publish,
                        permission.DisplayName
                        );
                    break;
                case "metaWeblog.editPost":
                    output.Completed = await service.EditPost(
                        input.BlogId,
                        input.PostId,
                        input.UserName,
                        input.Password,
                        input.Post, 
                        input.Publish);
                    break;
                case "metaWeblog.getPost":

                    var postStruct = await service.GetPost(
                        input.BlogId,
                        input.PostId,
                        input.UserName,
                        input.Password,
                        cancellationToken
                        );

                    output.Post = postStruct;
                    if(string.IsNullOrEmpty(output.Post.postId))
                    {
                        output.Fault = new FaultStruct { faultCode = "404", faultString = "post not found" };
                    }
                    break;
                case "metaWeblog.newMediaObject":
                case "wp.uploadFile":
                    output.MediaInfo = await service.NewMediaObject(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.MediaObject);
                    break;
                case "metaWeblog.getCategories":
                    output.Categories = await service.GetCategories(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        permission.IsBlogOwner,
                        cancellationToken);
                    break;
                case "wp.newCategory":
                    output.CategoryId = await service.NewCategory(
                        input.BlogId, 
                        input.Category,
                        input.UserName,
                        input.Password
                        );
                    break;
                case "metaWeblog.getRecentPosts":
                    var posts = await service.GetRecentPosts(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.NumberOfPosts,
                        cancellationToken);

                    output.Posts = posts;
                    break;
                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                case "wp.getUsersBlogs":
                    output.Blogs = await service.GetUserBlogs(
                        input.AppKey, 
                        input.UserName,
                        input.Password,
                        permission,
                        cancellationToken);
                    break;
                case "blogger.deletePost":
                    output.Completed = await service.DeletePost(
                        input.BlogId,
                        input.PostId,
                        input.UserName,
                        input.Password
                        );
                    break;
                case "blogger.getUserInfo":
                    // Not implemented.  Not planned.
                    throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");
                case "wp.newPage":
                    

                    //throw new MetaWeblogException("10", "The method newPage is not implemented.");
                    output.PageId = await service.NewPage(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.Page, 
                        input.Publish);
                    break;
                case "wp.getPageList":
                    
                    output.Pages = await service.GetPageList(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    break;
                case "wp.getPages":
                    
                    output.Pages = await service.GetPages(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    break;
                case "wp.getPage":
                   
                    output.Page = await service.GetPage(
                        input.BlogId, 
                        input.PageId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    break;
                case "wp.editPage":
                    
                    output.Completed = await service.EditPage(
                        input.BlogId, 
                        input.PageId,
                        input.UserName,
                        input.Password,
                        input.Page, 
                        input.Publish);
                    break;
                case "wp.deletePage":
                    
                    // Not implemented. 
                    //throw new MetaWeblogException("10", "The method deletePage is not implemented.");
                    output.Completed = await service.DeletePage(
                        input.BlogId, 
                        input.PageId,
                        input.UserName,
                        input.Password
                        );
                    break;
                case "wp.getAuthors":
                    // Not implemented. 
                    throw new MetaWeblogException("10", "The method getAuthors is not implemented.");
                //output.Authors = this.GetAuthors(input.BlogID, input.UserName, input.Password);
                //break;
                case "wp.getTags":
                    // Not implemented. 
                    throw new MetaWeblogException("10", "The method getTags is not implemented.");
                    //output.Keywords = this.GetKeywords(input.BlogID, input.UserName, input.Password);
                    //break;
            }


            return output;

        }
    }
}
