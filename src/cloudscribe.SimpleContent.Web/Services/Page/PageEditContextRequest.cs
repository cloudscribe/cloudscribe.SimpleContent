﻿using MediatR;
using System;
using System.Security.Claims;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageEditContextRequest : IRequest<PageEditContext>
    {
        public PageEditContextRequest(
            ClaimsPrincipal user,
            string slug,
            string pageId,
            string script,
            Guid? historyId
            )
        {
            User = user;
            Slug = slug;
            PageId = pageId;
            HistoryId = historyId;
            Script = script;
        }

        public ClaimsPrincipal User { get; private set; }
        public string Slug { get; private set; }
        public string Script { get; private set; }
        public string PageId { get; private set; }
        public Guid? HistoryId { get; private set; }
    }
}
