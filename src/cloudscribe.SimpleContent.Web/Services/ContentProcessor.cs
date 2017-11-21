using cloudscribe.SimpleContent.Models;
using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class ContentProcessor : IContentProcessor
    {
        public ContentProcessor(
            IHtmlProcessor htmlProcessor,
            IMarkdownProcessor markdownProcessor
            )
        {
            _htmlProcessor = htmlProcessor;
            _markdownProcessor = markdownProcessor;
        }

        private IHtmlProcessor _htmlProcessor;
        private IMarkdownProcessor _markdownProcessor;
        private MarkdownPipeline _mdPipeline = null;

        public string FilterHtml(IContentItem p, IProjectSettings projectSettings)
        {
            if (p.ContentType == "markdown")
            {
                if (_mdPipeline == null)
                {
                    _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                }

                return Markdown.ToHtml(p.Content, _mdPipeline);
            }
            return _htmlProcessor.FilterHtml(
                p.Content,
                projectSettings.CdnUrl,
                projectSettings.LocalMediaVirtualPath);
        }

        private string pslug = string.Empty;
        private string firstImageUrl;
        public string ExtractFirstImageUrl(IContentItem item, IUrlHelper urlHelper, string fallbackImageUrl = null)
        {
            if (urlHelper == null) return string.Empty;
            if (item == null) return string.Empty;

            var baseUrl = string.Concat(urlHelper.ActionContext.HttpContext.Request.Scheme,
                        "://",
                        urlHelper.ActionContext.HttpContext.Request.Host.ToUriComponent());

            if (item.ContentType == "markdown")
            {
                var mdImg = _markdownProcessor.ExtractFirstImageUrl(item.Content);
                if (!string.IsNullOrEmpty(mdImg))
                {
                    if (mdImg.StartsWith("http")) return mdImg;

                    return baseUrl + mdImg;
                }

                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(firstImageUrl) && pslug == item.Slug)
            {
                if (firstImageUrl.StartsWith("http")) return firstImageUrl;

                return baseUrl + firstImageUrl; //don't extract it more than once
            }

            if (item == null) return string.Empty;


            firstImageUrl = _htmlProcessor.ExtractFirstImageUrl(item.Content);
            pslug = item.Slug;

            if (firstImageUrl == null) return fallbackImageUrl;

            if (firstImageUrl.StartsWith("http")) return firstImageUrl;



            return baseUrl + firstImageUrl;
        }

        public ImageSizeResult ExtractFirstImageDimensions(IContentItem item)
        {
            return _htmlProcessor.ExtractFirstImageDimensions(item.Content);
        }

        public string FilterComment(IComment c)
        {
            return _htmlProcessor.FilterCommentLinks(c.Content);
        }

    }
}
