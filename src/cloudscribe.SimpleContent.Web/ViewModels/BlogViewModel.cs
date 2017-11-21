// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-11-21
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.Web.Common;
using cloudscribe.Web.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class BlogViewModel
    {
        public BlogViewModel(IContentProcessor contentProcessor)
        {
            _contentProcessor = contentProcessor;
            ProjectSettings = new ProjectSettings();
            Paging = new PaginationSettings();
            Categories = new Dictionary<string, int>();
            Archives = new Dictionary<string, int>();
            BlogRoutes = new DefaultBlogRoutes();
        }

        private IContentProcessor _contentProcessor;
        public IProjectSettings ProjectSettings { get; set; }
        public IPost CurrentPost { get; set; } = null;

        public string CurrentCategory { get; set; } = string.Empty;

        public string ListAction { get; set; } = "Index";

        public string EditPath { get; set; } = string.Empty;
        public string NewItemPath { get; set; } = string.Empty;

        public List<IPost> Posts { get; set; }
        public Dictionary<string, int> Categories { get; set; }
        public Dictionary<string, int> Archives { get; set; }
        public PaginationSettings Paging { get; private set; }
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public int Day { get; set; } = 0;
        public bool CanEdit { get; set; } = false;
        //public bool IsNewPost { get; set; } = false;
        //public string Mode { get; set; } = string.Empty;
        public bool ShowComments { get; set; } = true;
        public bool CommentsAreOpen { get; set; } = false;
        //public int ApprovedCommentCount { get; set; } = 0;
        //public bool CommentsRequireApproval { get; set; } = true;

        public IComment TmpComment { get; set; } = null;
        public IPost TmpPost { get; set; } = null;
        public ITimeZoneHelper TimeZoneHelper { get; set; }
        public string TimeZoneId { get; set; } = "GMT";

        public string PreviousPostUrl { get; set; } = string.Empty;

        public string NextPostUrl { get; set; } = string.Empty;
        
        public string FilterHtml(IPost p)
        {
            return _contentProcessor.FilterHtml(p, ProjectSettings);
            
        }

        public string FilterComment(IComment c)
        {
            return _contentProcessor.FilterComment(c);
        }

        public string FormatDate(DateTime pubDate)
        {
            var localTime = TimeZoneHelper.ConvertToLocalTime(pubDate, TimeZoneId);
            return localTime.ToString(ProjectSettings.PubDateFormat);
        }

        public string FormatDateForEdit(DateTime pubDate)
        {
            var localTime = TimeZoneHelper.ConvertToLocalTime(pubDate, TimeZoneId);
            return localTime.ToString();
        }

        public string FormatCommentDate(DateTime pubDate)
        {
            var localTime = TimeZoneHelper.ConvertToLocalTime(pubDate, TimeZoneId);
            return localTime.ToString();
        }

        public IBlogRoutes BlogRoutes { get; set; }

        //private string pslug = string.Empty;
        //private string firstImageUrl;
        public string ExtractFirstImageUrl(IPost post, IUrlHelper urlHelper, string fallbackImageUrl = null)
        {
            return _contentProcessor.ExtractFirstImageUrl(post, urlHelper, fallbackImageUrl);
            //if (urlHelper == null) return string.Empty;
            //if (post == null) return string.Empty;
            
            //var baseUrl = string.Concat(urlHelper.ActionContext.HttpContext.Request.Scheme,
            //            "://",
            //            urlHelper.ActionContext.HttpContext.Request.Host.ToUriComponent());

            //if (post.ContentType == "markdown")
            //{
            //    var mdImg = mdProcessor.ExtractFirstImageUrl(post.Content);
            //    if(!string.IsNullOrEmpty(mdImg))
            //    {
            //        if (mdImg.StartsWith("http")) return mdImg;

            //        return baseUrl + mdImg;
            //    }

            //    return string.Empty;
            //}

            //if (!string.IsNullOrWhiteSpace(firstImageUrl) && pslug == post.Slug)
            //{
            //    if (firstImageUrl.StartsWith("http")) return firstImageUrl;

            //    return baseUrl + firstImageUrl; //don't extract it more than once
            //}

            //if (post == null) return string.Empty;
            

            //firstImageUrl = filter.ExtractFirstImageUrl(post.Content);
            //pslug = post.Slug;

            //if (firstImageUrl == null) return fallbackImageUrl;

            //if(firstImageUrl.StartsWith("http")) return firstImageUrl;

            

            //return baseUrl + firstImageUrl;
        }

        public ImageSizeResult ExtractFirstImageDimensions(IPost post)
        {
            return _contentProcessor.ExtractFirstImageDimensions(post);
        }



    }
}
