// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2017-11-21
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common;
using Microsoft.AspNetCore.Mvc;
using Markdig;
using System;
using cloudscribe.SimpleContent.Web.Services;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel(IContentProcessor contentProcessor)
        {
            _contentProcessor = contentProcessor;
           
        }

        private IContentProcessor _contentProcessor;
        
        public IProjectSettings ProjectSettings { get; set; }
        public IPage CurrentPage { get; set; } = null;

        public string EditPath { get; set; } = string.Empty;
        public string NewItemPath { get; set; } = string.Empty;

        public string PageTreePath { get; set; } = string.Empty;
      

        public bool CanEdit { get; set; } = false;
        public bool IsNew { get; set; } = false;
        //public string Mode { get; set; } = string.Empty;
        public bool ShowComments { get; set; } = true;
        public bool CommentsAreOpen { get; set; } = false;
        //public int ApprovedCommentCount { get; set; } = 0;
        //public bool CommentsRequireApproval { get; set; } = true;

        public IComment TmpComment { get; set; } = null;
        public ITimeZoneHelper TimeZoneHelper { get; set; }
        public string TimeZoneId { get; set; } = "GMT";

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

        
        public string FilterHtml(IPage p)
        {
            return _contentProcessor.FilterHtml(p, ProjectSettings);
            
        }

        public string FilterComment(IComment c)
        {
            return _contentProcessor.FilterComment(c);
        }

        //private string firstImageUrl;
        public string ExtractFirstImageUrl(IPage page, IUrlHelper urlHelper)
        {
            return _contentProcessor.ExtractFirstImageUrl(page, urlHelper);

            //if (urlHelper == null) return string.Empty;

            //var baseUrl = string.Concat(urlHelper.ActionContext.HttpContext.Request.Scheme,
            //           "://",
            //           urlHelper.ActionContext.HttpContext.Request.Host.ToUriComponent());

            //if (page.ContentType == "markdown")
            //{
            //    var mdImg = mdProcessor.ExtractFirstImageUrl(page.Content);
            //    if (!string.IsNullOrEmpty(mdImg))
            //    {
            //        if (mdImg.StartsWith("http")) return mdImg;

            //        return baseUrl + mdImg;
            //    }

            //    return string.Empty;
            //}

            //if (!string.IsNullOrWhiteSpace(firstImageUrl))
            //{
            //    if (firstImageUrl.StartsWith("http")) return firstImageUrl;

            //    return baseUrl + firstImageUrl; //don't extract it more than once
            //}

            //if (page == null) return string.Empty;
            

            //firstImageUrl = filter.ExtractFirstImageUrl(page.Content);

            //if (firstImageUrl == null) return string.Empty;

            //if (firstImageUrl.StartsWith("http")) return firstImageUrl;
            
            //return baseUrl + firstImageUrl;
        }

        public ImageSizeResult ExtractFirstImageDimensions(IPage page)
        {
            return _contentProcessor.ExtractFirstImageDimensions(page);
        }
    }
}
