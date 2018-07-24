using MediatR;
using System;
using System.Security.Claims;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageEditRquest : IRequest<PageEditContext>
    {
        public PageEditRquest(
            ClaimsPrincipal user,
            string slug,
            string pageId,
            Guid? historyId
            )
        {
            User = user;
            Slug = slug;
            PageId = pageId;
            HistoryId = historyId;
        }

        public ClaimsPrincipal User { get; private set; }
        public string Slug { get; private set; }
        public string PageId { get; private set; }
        public Guid? HistoryId { get; private set; }
    }
}
