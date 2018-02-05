// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Authors:                  John Jacobs/Joe Audette
// Created:                 2017-12-22
// Last Modified:           2018-02-04 
// 
using cloudscribe.HtmlAgilityPack;
using cloudscribe.SimpleContent.Models;
using Humanizer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// Encapsulates the logic to display abbreviated teasers of blog posts on the index view.
    /// </summary>
    public class TeaserService : ITeaserService
    {
        public TeaserService(
            TeaserCache cache = null,
            ILogger<TeaserService> logger = null
            )
        {
            if(cache != null)
            {
                _cache = cache;
            }

            if(logger != null) // can be null in Unit Tests
            {
                _log = logger;
            }
           
           
        }

        private ILogger _log = null;
        private TeaserCache _cache = null;

        private const int defaultLengthWords = 20;
        private const int defaultLengthCharacters = 200;
        private const int defaultLengthAbsolute = 30;
        private const string terminator = "";

        public string CreateTeaserIfNeeded(IProjectSettings projectSettings, IPost post, string html)
            => ShouldDisplayTeaser(projectSettings, post) ? CreateTeaser(projectSettings, post, html) : html;

        public bool ShouldDisplayTeaser(IProjectSettings projectSettings, IPost post)
        {
            return !string.IsNullOrWhiteSpace(post.TeaserOverride) || (projectSettings.AutoTeaserMode == AutoTeaserMode.On && !post.SuppressAutoTeaser);
        }

        public bool ShouldDisplayReadMore(IProjectSettings projectSettings, IPost post)
        {
            if (!string.IsNullOrWhiteSpace(post.TeaserOverride)) { return true; }
            if (projectSettings.AutoTeaserMode == AutoTeaserMode.On && !post.SuppressAutoTeaser)
            {
                // known issue:
                // this will not be correct if content is markdown
                // since it hasn't been converted to html yet
                var contentLength = GetContentLength(post.Content, projectSettings.TeaserTruncationMode);
                if (contentLength > projectSettings.TeaserTruncationLength)
                {
                    return true;
                }

            }

            return false;
        }

        public TeaserResult GenerateTeaser(
            TeaserTruncationMode truncationMode,
            int truncationLength,
            string html,
            string cacheKey,
            string slug,
            string languageCode)
        {
            var result = new TeaserResult();

            if (string.IsNullOrWhiteSpace(html))
            {
                result.Content = html;
                result.DidTruncate = false;
                return result;
            }

            // Try to get language metadata for humanizer.
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
            if(contentLength <= truncationLength)
            {
                result.Content = html;
                result.DidTruncate = false;
                return result;
            }

            if (_cache != null)
            {
                var cachedTeaser = _cache.GetTeaser(cacheKey);
                if (!string.IsNullOrEmpty(cachedTeaser))
                {
                    result.Content = cachedTeaser;
                    result.DidTruncate = true;
                    return result;
                }
            }

            var isRightToLeftLanguage = cultureInfo.TextInfo.IsRightToLeft;

            // Get global teaser settings.
            //var autoTeaserMode = projectSettings.AutoTeaserMode;
            //var teaserTruncationMode = projectSettings.TeaserTruncationMode;
            var truncationLengthToUse = truncationLength <= 0 ? GetDefaultTeaserLength(truncationMode) : truncationLength;

            // Truncate the raw content first. In general, Humanizer is smart enough to ignore tags, especially if using word truncation.
            var text = TruncatePost(truncationMode, html, truncationLengthToUse, isRightToLeftLanguage);
            // Don't leave dangling <p> tags.
            HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;

            var modeDesc = GetModeDescription(truncationMode);
            
            //if we get bad output try increasing the allowed length unti it is valid
            while (!IsValidMarkup(text) && truncationLengthToUse <= contentLength)
            {
                truncationLengthToUse += 1;
                if (_log != null)
                {
                    _log.LogWarning($"teaser truncation for post {slug}, produced invalid html, so trying again and increasing the truncation length to {truncationLengthToUse} {modeDesc}. Might be best to make an explicit teaser for this post.");
                }

                text = TruncatePost(truncationMode, html, truncationLengthToUse, isRightToLeftLanguage);
            }

            if (!IsValidMarkup(text))
            {
                if (_log != null)
                {
                    _log.LogError($"failed to create valid teaser for post {slug}, so returning full content");
                }

                result.Content = html;
                result.DidTruncate = false;
                return result;
            }

            if (_cache != null)
            {
                _cache.AddToCache(text, cacheKey);
            }

            result.Content = text;
            result.DidTruncate = true;
            return result;

            
        }


        // Internal for unit testing purposes only.
        internal string CreateTeaser(IProjectSettings projectSettings, IPost post, string html)
        {
            if (!string.IsNullOrWhiteSpace(post.TeaserOverride))
            {
                return post.TeaserOverride;
            }

            if(string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            // Try to get language metadata for humanizer.
            var cultureInfo = CultureInfo.InvariantCulture;
            if(!string.IsNullOrEmpty(projectSettings.LanguageCode))
            {
                try
                {
                    cultureInfo = new CultureInfo(projectSettings.LanguageCode);
                }
                catch (CultureNotFoundException) { }
            }

            if(_cache != null)
            {
                var cachedTeaser = _cache.GetTeaser(post.Id);
                if(!string.IsNullOrEmpty(cachedTeaser))
                {
                    return cachedTeaser;
                }
            }
            
            var isRightToLeftLanguage = cultureInfo.TextInfo.IsRightToLeft;

            // Get global teaser settings.
            var autoTeaserMode = projectSettings.AutoTeaserMode;
            var teaserTruncationMode = projectSettings.TeaserTruncationMode;
            var teaserTruncationLength = projectSettings.TeaserTruncationLength <= 0 ? GetDefaultTeaserLength(teaserTruncationMode) : projectSettings.TeaserTruncationLength;

            // Truncate the raw content first. In general, Humanizer is smart enough to ignore tags, especially if using word truncation.
            var text = TruncatePost(teaserTruncationMode, html, teaserTruncationLength, isRightToLeftLanguage);
            // Don't leave dangling <p> tags.
            HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;

            var modeDesc = GetModeDescription(teaserTruncationMode);
            var contentLength = GetContentLength(html, teaserTruncationMode);
            //if we get bad output try increasing the allowed length unti it is valid
            while (!IsValidMarkup(text) && teaserTruncationLength <= contentLength)
            {
                teaserTruncationLength += 1;
                if (_log != null)
                {
                    _log.LogWarning($"teaser truncation for post {post.Slug}, produced invalid html, so trying again and increasing the truncation length to {teaserTruncationLength} {modeDesc}. Might be best to make an explicit teaser for this post.");
                }
                
                text = TruncatePost(teaserTruncationMode, html, teaserTruncationLength, isRightToLeftLanguage);
            }

            if (!IsValidMarkup(text))
            {
                if(_log != null)
                {
                    _log.LogError($"failed to create valid teaser for post {post.Slug}, so returning full content");
                }
                
                return html;
            }

            if (_cache != null)
            {
                _cache.AddToCache(text, post.Id);
            }

            return text;

            //var doc = new HtmlDocument();
           
            //doc.LoadHtml(text);
            //return doc.DocumentNode.InnerHtml;
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

        private string GetModeDescription(TeaserTruncationMode mode)
        {
            switch (mode)
            {
                case TeaserTruncationMode.Length:
                    return "string length";
                case TeaserTruncationMode.Character:
                    return "letters or digits";
                case TeaserTruncationMode.Word:
                default:
                    return "words";
            }
        }

        private int GetContentLength(string html, TeaserTruncationMode mode)
        {
            if (string.IsNullOrEmpty(html)) return 0;
            switch (mode)
            {
                case TeaserTruncationMode.Length:
                    return html.Length;
                case TeaserTruncationMode.Character:
                    return html.ToCharArray().Count(char.IsLetterOrDigit);
                case TeaserTruncationMode.Word:
                default:
                    return html.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Count();
            }
        }

        // Internal for unit testing purposes only.
        internal int GetDefaultTeaserLength(TeaserTruncationMode mode)
        {
            switch (mode)
            {
                case TeaserTruncationMode.Length:
                    return defaultLengthAbsolute;
                case TeaserTruncationMode.Character:
                    return defaultLengthCharacters;
                case TeaserTruncationMode.Word:
                default:
                    return defaultLengthWords;
            }
        }

        // Internal for unit testing purposes only.
        internal string TruncatePost(TeaserTruncationMode mode, string content, int length, bool isRightToLeftLanguage = false)
        {
            var truncateFrom = isRightToLeftLanguage ? TruncateFrom.Left : TruncateFrom.Right;
            switch (mode)
            {
                case TeaserTruncationMode.Length:
                    return content.Truncate(length, terminator, Truncator.FixedLength, truncateFrom);
                case TeaserTruncationMode.Character:
                    return content.Truncate(length, terminator, Truncator.FixedNumberOfCharacters, truncateFrom);
                case TeaserTruncationMode.Word:
                default:
                    return content.Truncate(length, terminator, Truncator.FixedNumberOfWords, truncateFrom);
            }
        }

    }
}