// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-06
// Last Modified:           2016-02-15
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.MetaWeblog
{
    public class MetaWeblogRequestValidator : IMetaWeblogRequestValidator
    {
        public Task<bool> IsValid(MetaWeblogRequest request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.MethodName)) { return Task.FromResult(false); }

            switch (request.MethodName)
            {
                case "system.listMethods":
                    return Task.FromResult(true);
                    
                case "wpcom.getFeatures":
                    return Task.FromResult(true);
                    
                case "metaWeblog.newPost":
                    if(string.IsNullOrEmpty(request.BlogId)) { return Task.FromResult(false); }
                    if(string.IsNullOrEmpty(request.Post.title)) { return Task.FromResult(false); }

                    return Task.FromResult(true);

                case "metaWeblog.getPost":
                    //TODO: more checks

                    return Task.FromResult(true);
         
                case "metaWeblog.newMediaObject":
                case "wp.uploadFile":

                    var requestedFileExtension = Path.GetExtension(request.MediaObject.name);
                    //TODO: validate extension against white list of allowed extensions

                    return Task.FromResult(true);

                case "metaWeblog.getCategories":
                case "wp.getCategories":


                    return Task.FromResult(true);

                case "wp.newCategory":


                    return Task.FromResult(true);

                case "metaWeblog.getRecentPosts":


                    return Task.FromResult(true);

                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":


                    return Task.FromResult(true);

                case "wp.getUsersBlogs":


                    return Task.FromResult(true);

                case "metaWeblog.editPost":
                case "blogger.deletePost":
                case "wp.editPage":
                case "wp.deletePage":


                    return Task.FromResult(true);

                case "wp.newPage":


                    return Task.FromResult(true);

                case "wp.getPage":


                    return Task.FromResult(true);

                case "wp.getPageList":


                    return Task.FromResult(true);

                case "wp.getPages":


                    return Task.FromResult(true);

                case "wp.getAuthors":


                    return Task.FromResult(true);

                case "wp.getTags":


                    return Task.FromResult(true);




            }


            return Task.FromResult(false);
        }
    }
}
