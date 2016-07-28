
using cloudscribe.SimpleContent.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.HtmlAgilityPack;

namespace cloudscribe.SimpleContent.Services
{
    public class HtmlProcessor
    {
        public HtmlProcessor()
        { }


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

        public string FilterCommentLinks(string rawComment)
        {
            return _linkRegex.Replace(rawComment, new MatchEvaluator(Evaluator));
        }

        //if cdnUrl provided converts all image, js and css references to absolute urls. Example: http://static.mydomain.com

        public string FilterHtml(
            string htmlInput,
            string cdnUrl = "",
            string rootPath = "")
        {
            var htmlOutput = htmlInput;

            // Youtube content embedded using this syntax: [youtube:xyzAbc123]
            var video = "<div class=\"video\"><iframe src=\"//www.youtube.com/embed/{0}?modestbranding=1&amp;theme=light\" allowfullscreen></iframe></div>";
            htmlOutput = Regex.Replace(htmlOutput, @"\[youtube:(.*?)\]", (Match m) => string.Format(video, m.Groups[1].Value));

            if (!rootPath.StartsWith("/"))
                rootPath = "/" + rootPath;

            if (cdnUrl.Length > 0)
            {
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

            }



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

                //post.Content = htmlOutput;

            }

            return Task.FromResult(htmlOutput);
        }

        public string ConvertUrlsToAbsolute(
            string absoluteBaseMediaUrl,
            string htmlInput)
        {
            var writer = new StringWriter();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlInput);

            foreach (var img in doc.DocumentNode.Descendants("img"))
            {
                if(img.Attributes["src"] != null)
                {
                    img.Attributes["src"].Value = new Uri(new Uri(absoluteBaseMediaUrl), img.Attributes["src"].Value).AbsoluteUri;
                }
                
            }

            foreach (var a in doc.DocumentNode.Descendants("a"))
            {
                if(a.Attributes["href"] != null)
                {
                    a.Attributes["href"].Value = new Uri(new Uri(absoluteBaseMediaUrl), a.Attributes["href"].Value).AbsoluteUri;
                }
                
            }

            doc.Save(writer);

            var newHtml = writer.ToString();

            return newHtml;

        }

    }
}
