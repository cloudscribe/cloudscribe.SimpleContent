// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-08-06
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.Web.Common;
using cloudscribe.Web.Pagination;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class BlogViewModel
    {
        public BlogViewModel()
        {
            ProjectSettings = new ProjectSettings();
            Paging = new PaginationSettings();
            Categories = new Dictionary<string, int>();
            Archives = new Dictionary<string, int>();

            filter = new HtmlProcessor();
            cryptoHelper = new CryptoHelper();
            EditorSettings = new EditorModel();
            BlogRoutes = new DefaultBlogRoutes();
        }

        private HtmlProcessor filter;
        private CryptoHelper cryptoHelper;
        public ProjectSettings ProjectSettings { get; set; }
        public Post CurrentPost { get; set; } = null;
        public EditorModel EditorSettings { get; set; } = null;
        public List<Post> Posts { get; set; }
        public Dictionary<string, int> Categories { get; set; }
        public Dictionary<string, int> Archives { get; set; }
        public PaginationSettings Paging { get; private set; }
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public int Day { get; set; } = 0;
        public bool CanEdit { get; set; } = false;
        public bool IsNewPost { get; set; } = false;
        public string Mode { get; set; } = string.Empty;
        public bool ShowComments { get; set; } = true;
        public bool CommentsAreOpen { get; set; } = false;
        //public int ApprovedCommentCount { get; set; } = 0;
        //public bool CommentsRequireApproval { get; set; } = true;

        public Comment TmpComment { get; set; } = null;
        public Post TmpPost { get; set; } = null;
        public ITimeZoneHelper TimeZoneHelper { get; set; }
        public string TimeZoneId { get; set; } = "GMT";

        public string PreviousPostUrl { get; set; } = string.Empty;

        public string NextPostUrl { get; set; } = string.Empty;

        public string FilterHtml(Post p)
        {
            return filter.FilterHtml(
                p.Content, 
                ProjectSettings.CdnUrl, 
                ProjectSettings.LocalMediaVirtualPath);
        }

        public string FilterComment(Comment c)
        {
            return filter.FilterCommentLinks(c.Content);
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
        



    }
}
