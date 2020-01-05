// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-04
// Last Modified:           2016-08-10
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
                    if(permission.CanEditPosts)
                    {
                        output.PostId = await service.NewPost(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.Post,
                        input.Publish,
                        permission.DisplayName
                        );
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }
                    
                    break;
                case "metaWeblog.editPost":
                    if (permission.CanEditPosts)
                    {
                        output.Completed = await service.EditPost(
                        input.BlogId,
                        input.PostId,
                        input.UserName,
                        input.Password,
                        input.Post,
                        input.Publish);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }


                    break;
                case "metaWeblog.getPost":
                    if (permission.CanEditPosts)
                    {
                        var postStruct = await service.GetPost(
                        input.BlogId,
                        input.PostId,
                        input.UserName,
                        input.Password,
                        cancellationToken
                        );

                        output.Post = postStruct;
                        if (string.IsNullOrEmpty(output.Post.postId))
                        {
                            output.Fault = new FaultStruct { faultCode = "404", faultString = "post not found" };
                        }
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "metaWeblog.newMediaObject":
                case "wp.uploadFile":
                    if (permission.CanEditPosts || permission.CanEditPages)
                    {
                        output.MediaInfo = await service.NewMediaObject(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.MediaObject);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "metaWeblog.getCategories":
                    if (permission.CanEditPosts || permission.CanEditPages)
                    {
                        output.Categories = await service.GetCategories(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "wp.newCategory":
                    if (permission.CanEditPosts || permission.CanEditPages)
                    {
                        output.CategoryId = await service.NewCategory(
                        input.BlogId,
                        input.Category,
                        input.UserName,
                        input.Password
                        );
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "metaWeblog.getRecentPosts":
                    if (permission.CanEditPosts)
                    {
                        var posts = await service.GetRecentPosts(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.NumberOfPosts,
                        cancellationToken);

                        output.Posts = posts;
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }
                    break;
                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                case "wp.getUsersBlogs":
                    output.Blogs = await service.GetUserBlogs(
                        input.AppKey, 
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    break;
                case "blogger.deletePost":
                    if (permission.CanEditPosts)
                    {
                        output.Completed = await service.DeletePost(
                        input.BlogId,
                        input.PostId,
                        input.UserName,
                        input.Password
                        );
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "blogger.getUserInfo":
                    // Not implemented.  Not planned.
                    throw new MetaWeblogException("10", "The method GetUserInfo is not implemented.");
                case "wp.newPage":
                    if (permission.CanEditPages)
                    {
                        output.PageId = await service.NewPage(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        input.Page,
                        input.Publish);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "wp.getPageList":
                    if (permission.CanEditPages)
                    {
                        output.Pages = await service.GetPageList(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "wp.getPages":
                    if (permission.CanEditPages)
                    {
                        output.Pages = await service.GetPages(
                        input.BlogId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "wp.getPage":
                    if (permission.CanEditPages)
                    {
                        output.Page = await service.GetPage(
                        input.BlogId,
                        input.PageId,
                        input.UserName,
                        input.Password,
                        cancellationToken);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "wp.editPage":
                    if (permission.CanEditPages)
                    {
                        output.Completed = await service.EditPage(
                        input.BlogId,
                        input.PageId,
                        input.UserName,
                        input.Password,
                        input.Page,
                        input.Publish);
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

                    break;
                case "wp.deletePage":
                    if (permission.CanEditPages)
                    {
                        output.Completed = await service.DeletePage(
                        input.BlogId,
                        input.PageId,
                        input.UserName,
                        input.Password
                        );
                    }
                    else
                    {
                        output.AddSecurityFault();
                    }

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
