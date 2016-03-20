// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-04
// Last Modified:           2016-02-22
// 

using cloudscribe.MetaWeblog.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{

    public interface IMetaWeblogService
    {
        #region MetaWeblog API

        //"metaWeblog.newPost
        Task<string> NewPost(
            string blogId,
            string userName,
            string password,
            PostStruct newPost, 
            bool publish,
            string authorDisplayName
            );
        
        //metaWeblog.editPost
        Task<bool> EditPost(
            string blogId,
            string postId,
            string userName,
            string password,
            PostStruct post, 
            bool publish);
        
        //metaWeblog.getPost
        Task<PostStruct> GetPost(
            string blogId,
            string postId,
            string userName,
            string password,
            CancellationToken cancellationToken);


        //metaWeblog.getCategories
        Task<List<CategoryStruct>> GetCategories(
            string blogId,
            string userName,
            string password,
            bool userIsBlogOwner,
            CancellationToken cancellationToken);
        

        //metaWeblog.getRecentPosts
        Task<List<PostStruct>> GetRecentPosts(
            string blogId,
            string userName,
            string password,
            int numberOfPosts,
            CancellationToken cancellationToken);
        
        //wp.uploadFile
        //metaWeblog.newMediaObject
        Task<MediaInfoStruct> NewMediaObject(
            string blogId,
            string userName,
            string password,
            MediaObjectStruct mediaObject);

        #endregion

        #region Blogger API

        //blogger.deletePost
        //bool DeletePost(string username, string password, string postid);
        // void DeletePost(string userName, string password, int postid);
        Task<bool> DeletePost(
            string blogId,
            string postId,
            string userName,
            string password
            );

        //blogger.getUsersBlogs
        //metaWeblog.getUsersBlogs
        //wp.getUsersBlogs
        //blogger.getUsersBlogs
        Task<List<BlogInfoStruct>> GetUserBlogs(
            string key, 
            string userName,
            string password,
            MetaWeblogSecurityResult permission,
            CancellationToken cancellationToken);

        #endregion

        // wp.newCategory
        Task<string> NewCategory(
            string blogId, 
            string category,
            string userName,
            string password
            );

        //wp.getPages method
        Task<List<PageStruct>> GetPages(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken);

        //wp.getPageList method
        Task<List<PageStruct>> GetPageList(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken);

        //wp.getPage method
        Task<PageStruct> GetPage(
            string blogId, 
            string pageId,
            string userName,
            string password,
            CancellationToken cancellationToken);

        //wp.newPage method
        Task<string> NewPage(
            string blogId,
            string userName,
            string password,
            PageStruct newPage, 
            bool publish);

        //wp.editPage
        Task<bool> EditPage(
            string blogId, 
            string pageId,
            string userName,
            string password,
            PageStruct page, 
            bool publish);

        //wp.deletePage
        Task<bool> DeletePage(
            string blogId, 
            string pageId,
            string userName,
            string password
            );
    }

    
}
