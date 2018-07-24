using MediatR;
using System;
using System.Security.Claims;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageViewContextRequest : IRequest<PageViewContext>
    {
        public PageViewContextRequest(
            ClaimsPrincipal user,
            string slug,
            bool showDraft,
            Guid? historyId
            )
        {
            User = user;
            Slug = slug;
            ShowDraft = showDraft;
            HistoryId = historyId;

        }

        public ClaimsPrincipal User { get; private set; }
        public string Slug { get; private set; }
        public bool ShowDraft { get; private set; }
        public Guid? HistoryId { get; private set; }

    }
}
