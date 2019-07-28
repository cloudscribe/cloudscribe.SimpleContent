using cloudscribe.HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cloudscribe.ContentUtils
{
    public class ExcerptHelper
    {
        public ExcerptHelper(
            //ILogger<ExcerptHelper> logger = null
            )
        {

        }

        private const int defaultLengthWords = 20;
        private const int defaultLengthCharacters = 200;
        private const int defaultLengthAbsolute = 30;
        private const string terminator = "";

        /// <summary>
        /// this is an expensive method as often many retries ar eneeded to produce a valid html fragment.
        /// therefore this should not be used for dynamic excerpt generation.
        /// Excerpt should be generated and saved when content is edited.
        /// </summary>
        /// <param name="truncationMode"></param>
        /// <param name="truncationLength"></param>
        /// <param name="html"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public ExcerptResult GenerateExcerpt(
            ExcerptTruncationMode truncationMode,
            int truncationLength,
            string html,
            //string cacheKey,
            //string slug,
            string languageCode //,
            //bool logWarnings = true
            )
        {
            var result = new ExcerptResult();

            if (string.IsNullOrWhiteSpace(html))
            {
                result.HtmlContent = html;
                result.DidTruncate = false;
                return result;
            }

            // Try to get language metadata
            var cultureInfo = CultureInfo.InvariantCulture;
            if (!string.IsNullOrEmpty(languageCode))
            {
                try
                {
                    cultureInfo = new CultureInfo(languageCode);
                }
                catch (CultureNotFoundException) { }
            }

            var contentLength = GetContentLength(html, truncationMode);
            if (contentLength <= truncationLength)
            {
                result.HtmlContent = html;
                result.DidTruncate = false;
                return result;
            }

            //if (_cache != null)
            //{
            //    var cachedTeaser = _cache.GetTeaser(cacheKey);
            //    if (!string.IsNullOrEmpty(cachedTeaser))
            //    {
            //        result.Content = cachedTeaser;
            //        result.DidTruncate = true;
            //        return result;
            //    }
            //}

            var isRightToLeftLanguage = cultureInfo.TextInfo.IsRightToLeft;

            // Get global teaser settings.
            var truncationLengthToUse = truncationLength <= 0 ? GetDefaultTeaserLength(truncationMode) : truncationLength;

            // Truncate the raw content first. In general, Humanizer is smart enough to ignore tags, especially if using word truncation.
            var text = TruncatePost(truncationMode, html, truncationLengthToUse, isRightToLeftLanguage);
            // Don't leave dangling <p> tags.
            HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;

            //var modeDesc = GetModeDescription(truncationMode);

            //if we get bad output try increasing the allowed length unti it is valid
            while (!IsValidMarkup(text) && truncationLengthToUse <= contentLength)
            {
                truncationLengthToUse += 1;
                //if (_log != null && logWarnings)
                //{
                //    _log.LogWarning($"teaser truncation for post {slug}, produced invalid html, so trying again and increasing the truncation length to {truncationLengthToUse} {modeDesc}. You should re-publish this post to create a persistent teaser.");
                //}

                text = TruncatePost(truncationMode, html, truncationLengthToUse, isRightToLeftLanguage);
            }

            if (!IsValidMarkup(text))
            {
                //if (_log != null)
                //{
                //    _log.LogError($"failed to create valid teaser for post {slug}, so returning full content");
                //}

                result.HtmlContent = html;
                result.DidTruncate = false;
                return result;
            }

            //if (_cache != null)
            //{
            //    _cache.AddToCache(text, cacheKey);
            //}

            var doc = new HtmlDocument();
            doc.LoadHtml(text);

            result.HtmlContent = doc.DocumentNode.InnerHtml;
            result.DidTruncate = true;
            return result;

        }

        private bool IsValidMarkup(string html)
        {
            var errors = GetMarkupErrors(html);
            return errors.Count() == 0;
        }

        private IEnumerable<HtmlParseError> GetMarkupErrors(string html)
        {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.OptionFixNestedTags = true;
            document.LoadHtml(html);

            return document.ParseErrors;
        }

        //private string GetModeDescription(ExcerptTruncationMode mode)
        //{
        //    switch (mode)
        //    {
        //        case ExcerptTruncationMode.Length:
        //            return "string length";
        //        case ExcerptTruncationMode.Character:
        //            return "letters or digits";
        //        case ExcerptTruncationMode.Word:
        //        default:
        //            return "words";
        //    }
        //}

        private int GetContentLength(string html, ExcerptTruncationMode mode)
        {
            if (string.IsNullOrEmpty(html)) return 0;
            switch (mode)
            {
                case ExcerptTruncationMode.Length:
                    return html.Length;
                case ExcerptTruncationMode.Character:
                    return html.ToCharArray().Count(char.IsLetterOrDigit);
                case ExcerptTruncationMode.Word:
                default:
                    return html.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();
            }
        }

        // Internal for unit testing purposes only.
        private int GetDefaultTeaserLength(ExcerptTruncationMode mode)
        {
            switch (mode)
            {
                case ExcerptTruncationMode.Length:
                    return defaultLengthAbsolute;
                case ExcerptTruncationMode.Character:
                    return defaultLengthCharacters;
                case ExcerptTruncationMode.Word:
                default:
                    return defaultLengthWords;
            }
        }

        // Internal for unit testing purposes only.
        private string TruncatePost(ExcerptTruncationMode mode, string content, int length, bool isRightToLeftLanguage = false)
        {
            var truncateFrom = isRightToLeftLanguage ? TruncateFrom.Left : TruncateFrom.Right;
            switch (mode)
            {
                case ExcerptTruncationMode.Length:
                    return content.Truncate(length, terminator, Truncator.FixedLength, truncateFrom);
                case ExcerptTruncationMode.Character:
                    return content.Truncate(length, terminator, Truncator.FixedNumberOfCharacters, truncateFrom);
                case ExcerptTruncationMode.Word:
                default:
                    return content.Truncate(length, terminator, Truncator.FixedNumberOfWords, truncateFrom);
            }
        }


    }
}
