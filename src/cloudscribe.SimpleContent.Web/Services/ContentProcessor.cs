using cloudscribe.HtmlAgilityPack;
using cloudscribe.SimpleContent.Models;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class ContentProcessor : IContentProcessor
    {
        public ContentProcessor(
            ITeaserService teaserService,
            IMarkdownProcessor markdownProcessor
            )
        {
            _markdownProcessor = markdownProcessor;
            _teaserService = teaserService;
        }

        private IMarkdownProcessor _markdownProcessor;
        private ITeaserService _teaserService;
        private MarkdownPipeline _mdPipeline = null;

        private static readonly Regex _linkRegex = new Regex("((http://|https://|www\\.)([A-Z0-9.\\-]{1,})\\.[0-9A-Z?;~&%\\(\\)#,=\\-_\\./\\+]{2,}[0-9A-Z?~&%#=\\-_/\\+])",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
            );

        private const string Link = "<a href=\"{0}{1}\" rel=\"nofollow\">{2}</a>";

        private static string Evaluator(Match match)
        {
            var info = CultureInfo.InvariantCulture;
            return string.Format(info, Link, !match.Value.Contains("://") ? "http://" : string.Empty, match.Value, ShortenUrl(match.Value, 50));
        }

        private static string ShortenUrl(string url, int max)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;
            if (url.Length <= max)
            {
                return url;
            }

            // Remove the protocal
            var startIndex = url.IndexOf("://");
            if (startIndex > -1)
            {
                url = url.Substring(startIndex + 3);
            }

            if (url.Length <= max)
            {
                return url;
            }

            // Compress folder structure
            var firstIndex = url.IndexOf("/") + 1;
            var lastIndex = url.LastIndexOf("/");
            if (firstIndex < lastIndex)
            {
                url = url.Remove(firstIndex, lastIndex - firstIndex);
                url = url.Insert(firstIndex, "...");
            }

            if (url.Length <= max)
            {
                return url;
            }

            // Remove URL parameters
            var queryIndex = url.IndexOf("?");
            if (queryIndex > -1)
            {
                url = url.Substring(0, queryIndex);
            }

            if (url.Length <= max)
            {
                return url;
            }

            // Remove URL fragment
            var fragmentIndex = url.IndexOf("#");
            if (fragmentIndex > -1)
            {
                url = url.Substring(0, fragmentIndex);
            }

            if (url.Length <= max)
            {
                return url;
            }

            // Compress page
            firstIndex = url.LastIndexOf("/") + 1;
            lastIndex = url.LastIndexOf(".");
            if (lastIndex - firstIndex > 10)
            {
                var page = url.Substring(firstIndex, lastIndex - firstIndex);
                var length = url.Length - max + 3;
                if (page.Length > length)
                {
                    url = url.Replace(page, string.Format("...{0}", page.Substring(length)));
                }
            }

            return url;
        }

        private HtmlNode GetPrimaryOrFirstImage(HtmlDocument doc)
        {
            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                if (img.Attributes["data-primary-image"] != null)
                {
                    return img;
                }
            }

            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                if (img.Attributes["src"] != null)
                {
                    return img;
                }
            }

            return null;
        }

        public ContentFilterResult FilterHtmlForRss(IPost post, IProjectSettings settings, string absoluteMediaBaseUrl)
        {
            bool useTeaser = (settings.TeaserMode == TeaserMode.ListsAndFeed || settings.TeaserMode == TeaserMode.FeedOnly) && !post.SuppressTeaser;
            var result = FilterHtmlForList(post, settings, useTeaser);
            result.FilteredContent = ConvertUrlsToAbsolute(absoluteMediaBaseUrl, result.FilteredContent);

            return result;
        }

        public ContentFilterResult FilterHtmlForList(IPost post, IProjectSettings settings)
        {
            bool useTeaser = (settings.TeaserMode == TeaserMode.ListsAndFeed || settings.TeaserMode == TeaserMode.ListOnly) && !post.SuppressTeaser;

            return FilterHtmlForList(post, settings, useTeaser);
        }

        private ContentFilterResult FilterHtmlForList(IPost post, IProjectSettings settings, bool useTeaser)
        {
            var result = new ContentFilterResult();
            if (useTeaser)
            {
                TeaserResult teaserResult = null;
                string teaser = null;

                if (!string.IsNullOrWhiteSpace(post.TeaserOverride))
                {
                    if (post.ContentType == "markdown")
                    {
                        teaser = MapImageUrlsToCdn(
                            ConvertMarkdownToHtml(post.TeaserOverride),
                            settings.CdnUrl,
                            settings.LocalMediaVirtualPath);

                    }
                    else
                    {
                        teaser = MapImageUrlsToCdn(
                            post.TeaserOverride,
                            settings.CdnUrl,
                            settings.LocalMediaVirtualPath);
                    }

                    result.FilteredContent = teaser;
                    result.IsFullContent = false;
                }
                else
                {
                    // need to generate teaser
                    if (post.ContentType == "markdown")
                    {
                        var html = MapImageUrlsToCdn(
                            ConvertMarkdownToHtml(post.Content),
                            settings.CdnUrl,
                            settings.LocalMediaVirtualPath);

                        teaserResult = _teaserService.GenerateTeaser(
                            settings.TeaserTruncationMode,
                            settings.TeaserTruncationLength,
                            html,
                            post.Id,
                            post.Slug,
                            settings.LanguageCode
                            );
                    }
                    else
                    {
                        var html = MapImageUrlsToCdn(
                            post.Content,
                            settings.CdnUrl,
                            settings.LocalMediaVirtualPath);

                        teaserResult = _teaserService.GenerateTeaser(
                            settings.TeaserTruncationMode,
                            settings.TeaserTruncationLength,
                            html,
                            post.Id,
                            post.Slug,
                            settings.LanguageCode
                            );
                    }

                    result.FilteredContent = teaserResult.Content;
                    result.IsFullContent = !teaserResult.DidTruncate;
                }


            }
            else
            {
                // using full content
                if (post.ContentType == "markdown")
                {
                    result.FilteredContent = MapImageUrlsToCdn(
                            ConvertMarkdownToHtml(post.Content),
                            settings.CdnUrl,
                            settings.LocalMediaVirtualPath);
                }
                else
                {
                    result.FilteredContent = MapImageUrlsToCdn(
                            post.Content,
                            settings.CdnUrl,
                            settings.LocalMediaVirtualPath);
                }

                result.IsFullContent = true;
            }



            return result;
        }

        public string ConvertMarkdownToHtml(string markdown)
        {
            if (_mdPipeline == null)
            {
                _mdPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            }

            return Markdown.ToHtml(markdown, _mdPipeline);
        }


        public string FilterHtml(IContentItem p, IProjectSettings projectSettings)
        {
            if (p.ContentType == "markdown")
            {
                var html = ConvertMarkdownToHtml(p.Content);
                return MapImageUrlsToCdn(
                    html,
                    projectSettings.CdnUrl,
                    projectSettings.LocalMediaVirtualPath);
            }
            return MapImageUrlsToCdn(
                p.Content,
                projectSettings.CdnUrl,
                projectSettings.LocalMediaVirtualPath);
        }

        public string MapImageUrlsToCdn(
            string htmlInput,
            string cdnUrl = "",
            string rootPath = "")
        {
            if (string.IsNullOrWhiteSpace(htmlInput)) return htmlInput;
            if (string.IsNullOrWhiteSpace(cdnUrl)) return htmlInput;

            var htmlOutput = htmlInput;

            // think this is a legacy from MiniBlog not needed
            // Youtube content embedded using this syntax: [youtube:xyzAbc123]
            //var video = "<div class=\"video\"><iframe src=\"//www.youtube.com/embed/{0}?modestbranding=1&amp;theme=light\" allowfullscreen></iframe></div>";
           // htmlOutput = Regex.Replace(htmlOutput, @"\[youtube:(.*?)\]", (Match m) => string.Format(video, m.Groups[1].Value));

            if (!rootPath.StartsWith("/"))
                rootPath = "/" + rootPath;
            
            htmlOutput = Regex.Replace(htmlOutput, "<img.*?src=\"([^\"]+)\"", (Match m) =>
            {
                string src = m.Groups[1].Value;
                int index = src.IndexOf(rootPath);

                if (index > -1)
                {
                    string clean = src.Substring(index);
                    return m.Value.Replace(src, cdnUrl + clean);
                }

                return m.Value;
            });
            
            return htmlOutput;
        }

        public Task<string> ConvertMediaUrlsToRelative(
            string mediaVirtualPath,
            string absoluteBaseMediaUrl,
            string htmlInput)
        {
            if (string.IsNullOrEmpty(htmlInput)) { return Task.FromResult(""); }

            var htmlOutput = htmlInput;
            // convert any fully qualified image urls to relative urls
            if (absoluteBaseMediaUrl.Length > 0)
            {

                // need to change absolute urls to relative urls
                // absolute image urls are created when posting from open live writer

                htmlOutput = Regex.Replace(htmlOutput, "<img.*?src=\"([^\"]+)\"", (Match m) =>
                {
                    string src = m.Groups[1].Value;
                    int index = src.IndexOf(absoluteBaseMediaUrl);

                    if (index > -1)
                    {
                        string clean = src.Substring(absoluteBaseMediaUrl.Length);
                        return m.Value.Replace(src, mediaVirtualPath + clean);
                    }

                    return m.Value;
                });

                htmlOutput = Regex.Replace(htmlOutput, "<a.*?href=\"([^\"]+)\"", (Match m) =>
                {
                    string src = m.Groups[1].Value;
                    int index = src.IndexOf(absoluteBaseMediaUrl);

                    if (index > -1)
                    {
                        string clean = src.Substring(absoluteBaseMediaUrl.Length);
                        return m.Value.Replace(src, mediaVirtualPath + clean);
                    }

                    return m.Value;
                });
                
            }

            return Task.FromResult(htmlOutput);
        }

        public string ConvertUrlsToAbsolute(
            string absoluteBaseMediaUrl,
            string htmlInput)
        {
            if (string.IsNullOrWhiteSpace(htmlInput)) return htmlInput;
            var writer = new StringWriter();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);

            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                if (img.Attributes["src"] != null)
                {
                    var src = img.Attributes["src"].Value;
                    if (src.StartsWith("data")) continue; //base64
                    if (src.StartsWith("http")) continue;
                    img.Attributes["src"].Value = new Uri(new Uri(absoluteBaseMediaUrl), src).AbsoluteUri;

                }

            }

            foreach (var a in doc.DocumentNode.Descendants("a"))
            {
                if (a.Attributes["href"] != null)
                {
                    var href = a.Attributes["href"].Value;
                    if (href.StartsWith("http")) continue;
                    a.Attributes["href"].Value = new Uri(new Uri(absoluteBaseMediaUrl), href).AbsoluteUri;
                }

            }

            doc.Save(writer);

            var newHtml = writer.ToString();

            return newHtml;

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


            firstImageUrl = ExtractFirstImageUrl(item.Content, fallbackImageUrl);
            pslug = item.Slug;

            if (string.IsNullOrWhiteSpace(firstImageUrl)) return fallbackImageUrl;
            if (firstImageUrl == baseUrl) return fallbackImageUrl;

            if (firstImageUrl.StartsWith("http")) return firstImageUrl;



            return baseUrl + firstImageUrl;
        }

        /// <summary>
        /// extracts the src url of the first image found, if the first image is not the one you want 
        /// then you can add an attribute data-primary-image="true" to make it use that image instead of the first one
        /// </summary>
        /// <param name="htmlInput"></param>
        /// <param name="fallbackImageUrl"></param>
        /// <returns></returns>
        public string ExtractFirstImageUrl(string htmlInput, string fallbackImageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(htmlInput)) return htmlInput;

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);

            var img = GetPrimaryOrFirstImage(doc);
            if (img != null && img.Attributes["src"] != null)
            {
                return img.Attributes["src"].Value;
            }

            return fallbackImageUrl;

        }

        public ImageSizeResult ExtractFirstImageDimensions(IContentItem item)
        {
            return ExtractFirstImageDimensions(item.Content);
        }

        public ImageSizeResult ExtractFirstImageDimensions(string htmlInput, string fallbackWidth = "550px", string fallbackHeight = "550px")
        {
            if (!string.IsNullOrWhiteSpace(htmlInput))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlInput);

                var img = GetPrimaryOrFirstImage(doc);
                if (img != null && img.Attributes["style"] != null)
                {
                    var style = img.Attributes["style"].Value;
                    var styleItems = style.Split(';')
                    .Select(s => s.Trim()).ToArray();
                    return ExtractDims(styleItems as string[], fallbackWidth, fallbackHeight);
                }

            }


            return new ImageSizeResult { Width = fallbackWidth, Height = fallbackHeight };

        }

        private ImageSizeResult ExtractDims(string[] atts, string fallbackWidth = "550px", string fallbackHeight = "550px")
        {
            string h = fallbackHeight;
            string w = fallbackWidth;
            foreach (var item in atts)
            {

                if (item.StartsWith("height") && item.Contains(":"))
                {
                    h = item.Substring(item.LastIndexOf(":") + 1);

                }
                if (item.StartsWith("width") && item.Contains(":"))
                {
                    w = item.Substring(item.LastIndexOf(":") + 1);

                }
            }

            return new ImageSizeResult { Width = w, Height = h };

        }

        public string RemoveImageStyleAttribute(
            string htmlInput)
        {
            if (string.IsNullOrWhiteSpace(htmlInput)) return htmlInput;
            string newHtml;
            using (var writer = new StringWriter())
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlInput);

                foreach (var img in doc.DocumentNode.Descendants("img"))
                {
                    if (img.Attributes["style"] != null)
                    {
                        img.Attributes["style"].Remove();
                    }

                    if (img.Attributes["height"] != null)
                    {
                        img.Attributes["height"].Remove();
                    }

                    if (img.Attributes["width"] != null)
                    {
                        img.Attributes["width"].Remove();
                    }

                }

                doc.Save(writer);
                newHtml = writer.ToString();
            }



            return newHtml;

        }


        public string FilterComment(IComment c)
        {
            return FilterCommentLinks(c.Content);
        }

        public string FilterCommentLinks(string rawComment)
        {
            return _linkRegex.Replace(rawComment, new MatchEvaluator(Evaluator));
        }


    }
}
