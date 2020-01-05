// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-04
// Last Modified:           2016-02-17
// 


using System.Xml.Linq;

namespace cloudscribe.MetaWeblog
{
    public class MetaWeblogResultFormatter : IMetaWeblogResultFormatter
    {
        public XDocument Format(MetaWeblogResult metaWeblogResult)
        {
            var xml = new XDocument();
            var methodResponse = new XElement("methodResponse");
            xml.Add(methodResponse);

            if (!string.IsNullOrEmpty(metaWeblogResult.Fault.faultCode))
            {
                BuildFaultResponse(methodResponse, metaWeblogResult);
                return xml;
            }

            var methodParams = new XElement("params");
            methodResponse.Add(methodParams);

            

            switch(metaWeblogResult.Method)
            {
                case "system.listMethods":
                    BuildListMethodsResponse(methodParams, metaWeblogResult);
                    break;

                case "wpcom.getFeatures":
                    BuildWPGetFeaturesResponse(methodParams, metaWeblogResult);
                    break;

                case "metaWeblog.newPost":
                    BuildNewPostResponse(methodParams, metaWeblogResult);
                    break;

                case "metaWeblog.getPost":
                    BuildGetPostResponse(methodParams, metaWeblogResult);
                    break;

                case "metaWeblog.newMediaObject":
                case "wp.uploadFile":
                    BuildMediaInfoResponse(methodParams, metaWeblogResult);
                    break;

                case "metaWeblog.getCategories":
                case "wp.getCategories":
                    BuildGetCategoriesResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.newCategory":
                    BuildNewCategoryResponse(methodParams, metaWeblogResult);
                    break;

                case "metaWeblog.getRecentPosts":
                    BuildRecentPostsResponse(methodParams, metaWeblogResult);
                    break;

                case "blogger.getUsersBlogs":
                case "metaWeblog.getUsersBlogs":
                    BuildUserBlogsResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.getUsersBlogs":
                    BuildWPUserBlogsResponse(methodParams, metaWeblogResult);
                    break;

                case "metaWeblog.editPost":
                case "blogger.deletePost":
                case "wp.editPage":
                case "wp.deletePage":
                    BuildActionResultBoolResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.newPage":
                    BuildNewPageResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.getPage":
                    BuildGetPageResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.getPageList":
                    BuildWPPageListResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.getPages":
                    BuildWPGetPagesResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.getAuthors":
                    BuildGetAuthorsResponse(methodParams, metaWeblogResult);
                    break;

                case "wp.getTags":
                    BuildGetTagsResponse(methodParams, metaWeblogResult);
                    break;



            }

            return xml;

        }

        private void BuildGetTagsResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var keyword in metaWeblogResult.Keywords)
            {
                var v = new XElement("value");
                dataElement.Add(v);
                var structElement = new XElement("struct");
                v.Add(structElement);

                var IdElement
                    = new XElement("member",
                    new XElement("name", "name"),
                    new XElement("value",
                    new XElement("string", keyword)
                    ) // end value
                    ); // end member

                structElement.Add(IdElement);
                
            }

        }

        private void BuildGetAuthorsResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var author in metaWeblogResult.Authors)
            {
                var v = new XElement("value");
                dataElement.Add(v);
                var structElement = new XElement("struct");
                v.Add(structElement);

                var IdElement
                    = new XElement("member",
                    new XElement("name", "user_id"),
                    new XElement("value",
                    new XElement("string", author.user_id)
                    ) // end value
                    ); // end member

                structElement.Add(IdElement);

                var lohginElement
                    = new XElement("member",
                    new XElement("name", "user_login"),
                    new XElement("value",
                    new XElement("string", author.user_login)
                    ) //end value
                    ); // end member

                structElement.Add(lohginElement);

                var emailElement
                    = new XElement("member",
                    new XElement("name", "user_email"),
                    new XElement("value",
                    new XElement("string", author.user_email)
                    ) //end value
                    ); // end member

                structElement.Add(emailElement);
                
                var metaElement
                    = new XElement("member",
                    new XElement("name", "meta_value"),
                    new XElement("value",
                    new XElement("string", author.meta_value)
                    ) //end value
                    ); // end member

                structElement.Add(metaElement);




            }

        }

        private void BuildWPGetPagesResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var page in metaWeblogResult.Pages)
            {
                var v = new XElement("value");
                dataElement.Add(v);
                var structElement = new XElement("struct");
                v.Add(structElement);

                var blogIdElement
                    = new XElement("member",
                    new XElement("name", "page_id"),
                    new XElement("value",
                    new XElement("string", page.pageId)
                    ) // end value
                    ); // end member

                structElement.Add(blogIdElement);

                var pageNameElement
                    = new XElement("member",
                    new XElement("name", "page_title"),
                    new XElement("value",
                    new XElement("string", page.title)
                    ) //end value
                    ); // end member
                
                structElement.Add(pageNameElement);

                var pageTitleElement
                    = new XElement("member",
                    new XElement("name", "title"),
                    new XElement("value",
                    new XElement("string", page.title)
                    ) //end value
                    ); // end member

                structElement.Add(pageTitleElement);

                var descElement
                    = new XElement("member",
                    new XElement("name", "description"),
                    new XElement("value",
                    new XElement("string", page.description)
                    ) //end value
                    ); // end member

                structElement.Add(descElement);

                var linkElement
                    = new XElement("member",
                    new XElement("name", "link"),
                    new XElement("value",
                    new XElement("string", page.link)
                    ) //end value
                    ); // end member

                structElement.Add(linkElement);


                var breaksMember =
                new XElement("member",
                new XElement("name", "mt_convert_breaks"),
                new XElement("value",
                new XElement("string", "__default__")
                ) // end value
                ); //end member

                structElement.Add(breaksMember);

                if (!string.IsNullOrEmpty(page.pageParentId))
                {
                    var ppiMember =
                    new XElement("member",
                    new XElement("name", "wp_page_parent_id"),
                    new XElement("value",
                    new XElement("string", page.pageParentId)
                    ) // end value
                    ); //end member

                    structElement.Add(ppiMember);
                }

                //var parentTitleElement
                //    = new XElement("member",
                //    new XElement("name", "wp_page_parent_title"),
                //    new XElement("value",
                //    new XElement("string", page.parentTitle)
                //    ) //end value
                //    ); // end member

                //structElement.Add(parentTitleElement);


                if (!string.IsNullOrEmpty(page.pageOrder))
                {
                    var commentPolicyMember =
                    new XElement("member",
                    new XElement("name", "wp_page_order"),
                    new XElement("value",
                    new XElement("string", page.pageOrder)
                    ) // end value
                    ); //end member

                    structElement.Add(commentPolicyMember);
                }

                if (!string.IsNullOrEmpty(page.published))
                {
                    var publishedMember =
                    new XElement("member",
                    new XElement("name", "page_status"),
                    new XElement("value",
                    new XElement("string", page.published)
                    ) // end value
                    ); //end member

                    structElement.Add(publishedMember);
                }

                if (!string.IsNullOrEmpty(page.commentPolicy))
                {
                    var publishedMember =
                    new XElement("member",
                    new XElement("name", "mt_allow_comments"),
                    new XElement("value",
                    new XElement("int", page.commentPolicy)
                    ) // end value
                    ); //end member

                    structElement.Add(publishedMember);
                }






            }

        }

        private void BuildWPPageListResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var page in metaWeblogResult.Pages)
            {
                var v = new XElement("value");
                dataElement.Add(v);
                var structElement = new XElement("struct");
                v.Add(structElement);

                var blogIdElement
                    = new XElement("member",
                    new XElement("name", "page_id"),
                    new XElement("value",
                    new XElement("string", page.pageId)
                    ) // end value
                    ); // end member
                
                structElement.Add(blogIdElement);

                var blogNameElement
                    = new XElement("member",
                    new XElement("name", "page_title"),
                    new XElement("value",
                    new XElement("string", page.title)
                    ) //end value
                    ); // end member

                structElement.Add(blogNameElement);
                
                var parentTitleElement
                    = new XElement("member",
                    new XElement("name", "wp_page_parent_title"),
                    new XElement("value",
                    new XElement("string", page.parentTitle)
                    ) //end value
                    ); // end member

                structElement.Add(parentTitleElement);


                

            }

        }

        private void BuildGetPageResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var structElement = new XElement("struct");

            paramElement.Add(valueElement);
            valueElement.Add(structElement);


            var postIdMember =
                new XElement("member",
                new XElement("name", "page_id"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.pageId)
                ) // end value
                ); //end member

            structElement.Add(postIdMember);

            var postTitleMember =
                new XElement("member",
                new XElement("name", "title"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.title)
                ) // end value
                ); //end member

            structElement.Add(postTitleMember);

            var postDescMember =
                new XElement("member",
                new XElement("name", "description"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.description)
                ) // end value
                ); //end member

            structElement.Add(postDescMember);

            var postLinkMember =
                new XElement("member",
                new XElement("name", "link"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.link)
                ) // end value
                ); //end member

            structElement.Add(postLinkMember);

            var breaksMember =
                new XElement("member",
                new XElement("name", "mt_convert_breaks"),
                new XElement("value",
                new XElement("string", "__default__")
                ) // end value
                ); //end member

            structElement.Add(breaksMember);

            if (!string.IsNullOrEmpty(metaWeblogResult.Page.pageParentId))
            {
                var slugMember =
                new XElement("member",
                new XElement("name", "wp_page_parent_id"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.pageParentId)
                ) // end value
                ); //end member

                structElement.Add(slugMember);
            }

            if (!string.IsNullOrEmpty(metaWeblogResult.Page.parentTitle))
            {
                var excerptMember =
                new XElement("member",
                new XElement("name", "wp_page_parent_title"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.parentTitle)
                ) // end value
                ); //end member

                structElement.Add(excerptMember);
            }

            if (!string.IsNullOrEmpty(metaWeblogResult.Page.pageOrder))
            {
                var commentPolicyMember =
                new XElement("member",
                new XElement("name", "wp_page_order"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.pageOrder)
                ) // end value
                ); //end member

                structElement.Add(commentPolicyMember);
            }

            if (!string.IsNullOrEmpty(metaWeblogResult.Page.published))
            {
                var publishedMember =
                new XElement("member",
                new XElement("name", "page_status"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Page.published)
                ) // end value
                ); //end member

                structElement.Add(publishedMember);
            }

            if (!string.IsNullOrEmpty(metaWeblogResult.Page.commentPolicy))
            {
                var publishedMember =
                new XElement("member",
                new XElement("name", "mt_allow_comments"),
                new XElement("value",
                new XElement("int", metaWeblogResult.Page.commentPolicy)
                ) // end value
                ); //end member

                structElement.Add(publishedMember);
            }

           
           
        }

        private void BuildNewPageResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var newElement =
                new XElement("param",
                new XElement("value",
                new XElement("string", metaWeblogResult.PageId)
                )// end value
                ); // end member

            methodParams.Add(newElement);
        }

        private void BuildActionResultBoolResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var boolElement =
                new XElement("param",
                new XElement("value",
                new XElement("boolean", metaWeblogResult.Completed ? "1" : "0")
                )// end value
                ); // end member

            methodParams.Add(boolElement);
        }

        private void BuildWPUserBlogsResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            methodParams.Add(paramElement);

            foreach (var blog in metaWeblogResult.Blogs)
            {
                var v = new XElement("value");
                dataElement.Add(v);
                var structElement = new XElement("struct");
                v.Add(structElement);

                var isAdminElement
                    = new XElement("member",
                    new XElement("name", "isAdmin"),
                    new XElement("value", 
                    new XElement("boolean","1")
                    )
                    );

                structElement.Add(isAdminElement);

                var urlElement
                    = new XElement("member",
                    new XElement("name", "url"),
                    new XElement("value",
                    new XElement("string", blog.url)
                    ) // end value
                    ); // end member

                structElement.Add(urlElement);

                var blogIdElement
                    = new XElement("member",
                    new XElement("name", "blogid"),
                    new XElement("value",
                    new XElement("string", blog.blogId)
                    ) // end value
                    ); // end member

                structElement.Add(blogIdElement);

                var blogNameElement
                    = new XElement("member",
                    new XElement("name", "blogName"),
                    new XElement("value",
                    new XElement("string", blog.blogName)
                    ) //end value
                    ); // end member

                structElement.Add(blogNameElement);

                if(!string.IsNullOrEmpty(blog.xmlrpcUrl))
                {
                    var rpcElement
                    = new XElement("member",
                    new XElement("name", "xmlrpc"),
                    new XElement("value",
                    new XElement("string", blog.xmlrpcUrl)
                    ) //end value
                    ); // end member

                    structElement.Add(rpcElement);
                }

            }

        }

        private void BuildUserBlogsResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var blog in metaWeblogResult.Blogs)
            {
                var v = new XElement("value");
                dataElement.Add(v);
                var structElement = new XElement("struct");
                v.Add(structElement);

                var urlElement
                    = new XElement("member",
                    new XElement("name", "url"),
                    new XElement("value", blog.url)
                    );

                structElement.Add(urlElement);

                var blogIdElement
                    = new XElement("member",
                    new XElement("name", "blogid"),
                    new XElement("value", blog.blogId)
                    );

                structElement.Add(blogIdElement);

                var blogNameElement
                    = new XElement("member",
                    new XElement("name", "blogName"),
                    new XElement("value", blog.blogName)
                    );

                structElement.Add(blogNameElement);

            }

        }

        private void BuildRecentPostsResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var post in metaWeblogResult.Posts)
            {
                var vElement = new XElement("value");
                dataElement.Add(vElement);
                var structElement = new XElement("struct");
                vElement.Add(structElement);
                
                var memberElement =
                        new XElement("member",
                        new XElement("name", "postid"),
                        new XElement("value",
                        new XElement("string", post.postId)
                        )// end value
                        ); // end member

                structElement.Add(memberElement);
                
                var dateCreatedElement =
                        new XElement("member",
                        new XElement("name", "dateCreated"),
                        new XElement("value",
                        new XElement("string", Utils.ConvertDatetoISO8601(post.postDate))
                        )// end value
                        ); // end member

                structElement.Add(dateCreatedElement);

                var titleElement =
                        new XElement("member",
                        new XElement("name", "title"),
                        new XElement("value",
                        new XElement("string", post.title)
                        )// end value
                        ); // end member

                structElement.Add(titleElement);

                var descElement =
                        new XElement("member",
                        new XElement("name", "description"),
                        new XElement("value",
                        new XElement("string", post.description)
                        )// end value
                        ); // end member

                structElement.Add(descElement);

                var linkElement =
                        new XElement("member",
                        new XElement("name", "link"),
                        new XElement("value",
                        new XElement("string", post.link)
                        )// end value
                        ); // end member

                structElement.Add(linkElement);

                if (!string.IsNullOrEmpty(post.slug))
                {
                    var slugElement =
                        new XElement("member",
                        new XElement("name", "wp_slug"),
                        new XElement("value",
                        new XElement("string", post.slug)
                        )// end value
                        ); // end member

                    structElement.Add(slugElement);
                }

                
                if (!string.IsNullOrEmpty(post.excerpt))
                {
                    var excerptElement =
                        new XElement("member",
                        new XElement("name", "mt_excerpt"),
                        new XElement("value",
                        new XElement("string", post.excerpt)
                        )// end value
                        ); // end member

                    structElement.Add(excerptElement);
                }

                if (!string.IsNullOrEmpty(post.commentPolicy))
                {
                    var allowCommentsElement =
                        new XElement("member",
                        new XElement("name", "mt_allow_comments"),
                        new XElement("value",
                        new XElement("string", post.commentPolicy)
                        )// end value
                        ); // end member

                    structElement.Add(allowCommentsElement);
                }

                if((post.tags != null) &&(post.tags.Count > 0))
                {
                    var tags = new string[post.tags.Count];
                    for (var i = 0; i < post.tags.Count; i++)
                    {
                        tags[i] = post.tags[i];
                    }

                    var tagList = string.Join(",", tags);

                    var tagElement =
                        new XElement("member",
                        new XElement("name", "mt_keywords"),
                        new XElement("value",
                        new XElement("string", tagList)
                        )// end value
                        ); // end member

                    structElement.Add(tagElement);
                }

                var publishElement =
                        new XElement("member",
                        new XElement("name", "publish"),
                        new XElement("value",
                        new XElement("boolean", post.publish ? "1" : "0")
                        )// end value
                        ); // end member

                structElement.Add(publishElement);

                if ((post.categories != null) &&(post.categories.Count > 0))
                {
                    var mem = new XElement("member");
                    var nm = new XElement("name", "categories");
                    var v = new XElement("value");
                    var ar = new XElement("array");
                    var data = new XElement("data");

                    structElement.Add(mem);
                    mem.Add(nm);
                    mem.Add(v);
                    v.Add(ar);
                    ar.Add(data);

                    foreach (var cat in post.categories)
                    {
                        var val =
                            new XElement("value",
                            new XElement("string", cat)
                            );

                        data.Add(val);

                    }
                }

            }

            
        }

        private void BuildNewCategoryResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            XElement newElement =
                new XElement("param",
                new XElement("value",
                new XElement("string", metaWeblogResult.CategoryId)
                )//end value
                )// end param
                ;

            methodParams.Add(newElement);
        }

        private void BuildGetCategoriesResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var arrayElement = new XElement("array");
            var dataElement = new XElement("data");
            paramElement.Add(valueElement);
            valueElement.Add(arrayElement);
            arrayElement.Add(dataElement);

            foreach (var category in metaWeblogResult.Categories)
            {
                var vElement = new XElement("value");
                dataElement.Add(vElement);
                var structElement = new XElement("struct");
                vElement.Add(structElement);
                if(!string.IsNullOrEmpty(category.description))
                {
                    var memberElement =
                        new XElement("member",
                        new XElement("name", "description"),
                        new XElement("value",
                        new XElement("string", category.description)
                        )// end value
                        ); // end member

                    structElement.Add(memberElement);
                }

                var catIdElement =
                        new XElement("member",
                        new XElement("name", "categoryId"),
                        new XElement("value",
                        new XElement("string", category.id)
                        )// end value
                        ); // end member

                structElement.Add(catIdElement);

                if (!string.IsNullOrEmpty(category.parentId))
                {
                    var parentIdElement =
                        new XElement("member",
                        new XElement("name", "parentId"),
                        new XElement("value",
                        new XElement("string", category.parentId)
                        )// end value
                        ); // end member

                    structElement.Add(parentIdElement);
                }

                var catTitleElement =
                        new XElement("member",
                        new XElement("name", "title"),
                        new XElement("value",
                        new XElement("string", category.title)
                        )// end value
                        ); // end member

                structElement.Add(catTitleElement);

                var catNameElement =
                        new XElement("member",
                        new XElement("name", "categoryName"),
                        new XElement("value",
                        new XElement("string", category.title)
                        )// end value
                        ); // end member

                structElement.Add(catNameElement);

                if (!string.IsNullOrEmpty(category.htmlUrl))
                {
                    var htmlUrlElement =
                        new XElement("member",
                        new XElement("name", "htmlUrl"),
                        new XElement("value",
                        new XElement("string", category.htmlUrl)
                        )// end value
                        ); // end member

                    structElement.Add(htmlUrlElement);
                }

                if (!string.IsNullOrEmpty(category.rssUrl))
                {
                    var rssUrlElement =
                        new XElement("member",
                        new XElement("name", "rssUrl"),
                        new XElement("value",
                        new XElement("string", category.rssUrl)
                        )// end value
                        ); // end member

                    structElement.Add(rssUrlElement);
                }

            }
            
            
        }

        private void BuildMediaInfoResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            XElement newElement =
                new XElement("param",
                new XElement("value",
                new XElement("struct",

                new XElement("member",
                new XElement("name", "file"),
                new XElement("value",
                new XElement("string", metaWeblogResult.MediaInfo.file)
                ) // end value
                ), // end member

                new XElement("member",
                new XElement("name", "url"),
                new XElement("value",
                new XElement("string", metaWeblogResult.MediaInfo.url)
                ) // end value
                ), // end member

                new XElement("member",
                new XElement("name", "type"),
                new XElement("value",
                new XElement("string", metaWeblogResult.MediaInfo.type)
                ) // end value
                ) // end member


                )//end struct
                )//end value
                )// end param
                ;

            methodParams.Add(newElement);
        }

        private void BuildGetPostResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            var paramElement = new XElement("param");
            methodParams.Add(paramElement);
            var valueElement = new XElement("value");
            var structElement = new XElement("struct");

            paramElement.Add(valueElement);
            valueElement.Add(structElement);


            var postIdMember = 
                new XElement("member",
                new XElement("name", "postid"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Post.postId)
                ) // end value
                ); //end member

            structElement.Add(postIdMember);

            var postTitleMember = 
                new XElement("member",
                new XElement("name", "title"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Post.title)
                ) // end value
                ); //end member

            structElement.Add(postTitleMember);

            var postDescMember =
                new XElement("member",
                new XElement("name", "description"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Post.description)
                ) // end value
                ); //end member

            structElement.Add(postDescMember);

            var postLinkMember = 
                new XElement("member",
                new XElement("name", "link"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Post.link)
                ) // end value
                ); //end member

            structElement.Add(postLinkMember);

            if(!string.IsNullOrEmpty(metaWeblogResult.Post.slug))
            {
                var slugMember =
                new XElement("member",
                new XElement("name", "wp_slug"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Post.slug)
                ) // end value
                ); //end member

                structElement.Add(slugMember);
            }

            if (!string.IsNullOrEmpty(metaWeblogResult.Post.excerpt))
            {
                var excerptMember =
                new XElement("member",
                new XElement("name", "mt_excerpt"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Post.excerpt)
                ) // end value
                ); //end member

                structElement.Add(excerptMember);
            }

            if (!string.IsNullOrEmpty(metaWeblogResult.Post.commentPolicy))
            {
                var commentPolicyMember =
                new XElement("member",
                new XElement("name", "mt_allow_comments"),
                new XElement("value",
                new XElement("int", metaWeblogResult.Post.commentPolicy)
                ) // end value
                ); //end member

                structElement.Add(commentPolicyMember);
            }

            var dateCreatedMember =
                new XElement("member",
                new XElement("name", "dateCreated"),
                new XElement("value",
                new XElement("dateTime.iso8601", Utils.ConvertDatetoISO8601(metaWeblogResult.Post.postDate))
                ) // end value
                ); //end member

            structElement.Add(dateCreatedMember);

            var publishMember =
                new XElement("member",
                new XElement("name", "publish"),
                new XElement("value",
                new XElement("boolean", metaWeblogResult.Post.publish ? "1" : "0")
                ) // end value
                ); //end member

            structElement.Add(publishMember);

            if((metaWeblogResult.Post.tags != null) &&(metaWeblogResult.Post.tags.Count > 0))
            {
                var tags = new string[metaWeblogResult.Post.tags.Count];
                for (var i = 0; i < metaWeblogResult.Post.tags.Count; i++)
                {
                    tags[i] = metaWeblogResult.Post.tags[i];
                }

                var tagList = string.Join(",", tags);

                var tagsMember =
                new XElement("member",
                new XElement("name", "mt_keywords"),
                new XElement("value",
                new XElement("string", tagList)
                ) // end value
                ); //end member

                structElement.Add(tagsMember);

            }

            if ((metaWeblogResult.Post.categories != null) &&(metaWeblogResult.Post.categories.Count > 0))
            {
                var categoriesMember = new XElement("member");
                structElement.Add(categoriesMember);
                var catName = new XElement("name", "categories");
                categoriesMember.Add(catName);

                var catValueMember = new XElement("value");
                categoriesMember.Add(catValueMember);

                var catArrayMember = new XElement("array");
                catValueMember.Add(catArrayMember);

                var catDataMember = new XElement("data");
                catArrayMember.Add(catDataMember);

                foreach (var cat in metaWeblogResult.Post.categories)
                {
                    var v = new XElement("value",
                        new XElement("string", cat)
                        );

                    catDataMember.Add(v);
                }

            }

            
            

        }

        

        private void BuildNewPostResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            XElement newElement =
                new XElement("param",
                new XElement("value",
                new XElement("string", metaWeblogResult.PostId)
                )//end value
                )// end param
                ;

            methodParams.Add(newElement);
        }

        private void BuildWPGetFeaturesResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            XElement newElement =
                new XElement("param",
                new XElement("value",
                new XElement("array",
                new XElement("data",

                new XElement("value",
                new XElement("struct",
                new XElement("member",
                new XElement("name", "videopress_enabled"),
                new XElement("value",
                new XElement("boolean","0")
                )// end value
                )//end member
                )//end struct
                )//end value


                )// end data
                )//end array
                )//end value
                )// end param
                ;

            methodParams.Add(newElement);
        }

        private void BuildListMethodsResponse(XElement methodParams, MetaWeblogResult metaWeblogResult)
        {
            XElement newElement = 
                new XElement("param",
                new XElement("value",
                new XElement("array",
                new XElement("data",

                // these are not really all supported but I'm listing them as supported to see which
                // ones are actually called by blogging clients
                // like the Wordpress iPad app, BlogPress, and Blogsy

                new XElement("value",
                new XElement("string", "system.multicall")
                ), //end value

                new XElement("value",
                new XElement("string", "system.listMethods")
                ) ,//end value

                new XElement("value",
                new XElement("string", "system.getCapabilities")
                ), //end value

                new XElement("value",
                new XElement("string", "pingback.extensions.getPingbacks")
                ), //end value

                new XElement("value",
                new XElement("string", "pingback.ping")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.publishPost")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.getTrackbackPings")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.supportedTextFilters")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.supportedMethods")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.setPostCategories")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.getPostCategories")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.getRecentPostTitles")
                ), //end value

                new XElement("value",
                new XElement("string", "mt.getCategoryList")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.getUsersBlogs")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.deletePost")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.newMediaObject")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.setTemplate")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.getTemplate")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.getCategories")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.getRecentPosts")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.getPost")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.editPost")
                ), //end value

                new XElement("value",
                new XElement("string", "metaWeblog.newPost")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.deletePost")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.editPost")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.newPost")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.setTemplate")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.getTemplate")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.getRecentPosts")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.getPost")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.getUserInfo")
                ), //end value

                new XElement("value",
                new XElement("string", "blogger.getUsersBlogs")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.newPage")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getPageList")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getPages")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getPage")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.editPage")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.deletePage")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getUsersBlogs")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getCategories")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.deleteCategory")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.newCategory")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.suggestCategories")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getTags")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.uploadFile")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getCommentStatusList")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.newComment")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.editComment")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.deleteComment")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getComments")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getComment")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getCommentCount")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.setOptions")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getOptions")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getPageTemplates")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getPageStatusList")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getPostStatusList")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getAuthors")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getMediaLibrary")
                ), //end value

                new XElement("value",
                new XElement("string", "wp.getMediaItem")
                ) //end value

                )// end data
                )//end array
                )//end value
                )// end param

                ;

            methodParams.Add(newElement);
        }

        private void BuildFaultResponse(XElement methodResponse, MetaWeblogResult metaWeblogResult)
        {
            XElement faultElement = 
                new XElement("fault",
                new XElement("value",
                new XElement("struct",

                new XElement("member",
                new XElement("name","faultCode"),
                new XElement("value",
                new XElement("int", metaWeblogResult.Fault.faultCode)
                )// end value
                )// end member
                ,
                new XElement("member",
                new XElement("name", "faultString"),
                new XElement("value",
                new XElement("string", metaWeblogResult.Fault.faultString)
                )// end value
                ) //end member

                )//end struct
                )//end value
                );

            methodResponse.Add(faultElement);

        }
    }
}
