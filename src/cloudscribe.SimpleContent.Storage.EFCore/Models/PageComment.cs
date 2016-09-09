// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-08
// Last Modified:			2016-09-09
// 

using cloudscribe.SimpleContent.Models;
using System;

namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PageComment : IComment
    {
        public PageComment()
        {

        }

        public string Id { get; set; }
        public string ContentId
        {
            get { return pageEntityId; }
            set { pageEntityId = value; }
        }

        // not part of IComment
        private string pageEntityId;
        public string PageEntityId
        {
            get { return pageEntityId; }
            set { pageEntityId = value; }
        }

        public string ProjectId { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsApproved { get; set; }

        public static PageComment FromIComment(IComment comment)
        {
            var c = new PageComment();
            c.Author = comment.Author;
            c.Content = comment.Content;
            c.ContentId = comment.ContentId;
            c.Email = comment.Email;
            c.Id = comment.Id;
            c.Ip = comment.Ip;
            c.IsAdmin = comment.IsAdmin;
            c.IsApproved = comment.IsApproved;
            c.ProjectId = comment.ProjectId;
            c.PubDate = comment.PubDate;
            c.UserAgent = comment.UserAgent;
            c.Website = comment.Website;

            return c;
        }
    }
}
