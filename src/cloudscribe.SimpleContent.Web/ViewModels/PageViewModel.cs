// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2016-06-07
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Services;
using cloudscribe.Web.Common;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {
            filter = new HtmlProcessor();
            EditorSettings = new EditorModel();
        }

        private HtmlProcessor filter;

        public ProjectSettings ProjectSettings { get; set; }
        public Page CurrentPage { get; set; } = null;
        public EditorModel EditorSettings { get; set; } = null;

        public bool CanEdit { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public string Mode { get; set; } = string.Empty;
        public bool ShowComments { get; set; } = true;
        public bool CommentsAreOpen { get; set; } = false;
        //public int ApprovedCommentCount { get; set; } = 0;
        //public bool CommentsRequireApproval { get; set; } = true;

        public Comment TmpComment { get; set; } = null;
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

        public string FilterHtml(Page p)
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
    }
}
