using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.SimpleContent.Web.Services
{
    public interface IContentProcessor
    {
        ImageSizeResult ExtractFirstImageDimensions(IContentItem item);
        string ExtractFirstImageUrl(IContentItem item, IUrlHelper urlHelper, string fallbackImageUrl = null);
        string FilterHtml(IContentItem p, IProjectSettings projectSettings);
        string FilterComment(IComment comment);
    }
}