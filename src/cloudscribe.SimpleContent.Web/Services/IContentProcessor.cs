using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public interface IContentProcessor
    {
        ImageSizeResult ExtractFirstImageDimensions(IContentItem item);
        ImageSizeResult ExtractFirstImageDimensions(string htmlInput, string fallbackWidth = "550px", string fallbackHeight = "550px");
        string ExtractFirstImageUrl(IContentItem item, IUrlHelper urlHelper, string fallbackImageUrl = null);
        string FilterHtml(IContentItem p, IProjectSettings projectSettings);
        ContentFilterResult FilterHtmlForList(IPost post, IProjectSettings settings);
        ContentFilterResult FilterHtmlForRss(IPost post, IProjectSettings settings, string absoluteMediaBaseUrl);
        string FilterComment(IComment comment);

        string ConvertMarkdownToHtml(string markdown);

        Task<string> ConvertMediaUrlsToRelative(string mediaVirtualPath, string absoluteBaseMediaUrl, string htmlInput);
        string ConvertUrlsToAbsolute(string absoluteBaseMediaUrl, string htmlInput);
        string RemoveImageStyleAttribute(string htmlInput);
    }
}