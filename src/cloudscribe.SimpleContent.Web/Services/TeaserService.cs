// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  John Jacobs
// Created:                 2017-12-22
// Last Modified:           2017-12-22
// 
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Humanizer.Localisation;
using System.Globalization;
using cloudscribe.HtmlAgilityPack;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// Encapsulates the logic to display abbreviated teasers of blog posts on the index view.
    /// </summary>
    public class TeaserService : ITeaserService
    {
        private const int defaultLengthWords = 20;
        private const int defaultLengthCharacters = 200;
        private const int defaultLengthParagraphs = 1;
        private const string terminator = "...";

        public string CreateTeaserIfNeeded(IProjectSettings projectSettings, IPost post, string html)
            => ShouldDisplayTeaser(projectSettings, post) ? CreateTeaser(projectSettings, html) : html;

        private bool ShouldDisplayTeaser(IProjectSettings projectSettings, IPost post)
            => !string.IsNullOrWhiteSpace(post.TeaserOverride) || (projectSettings.AutoTeaserMode == AutoTeaserMode.On && !post.SuppressAutoTeaser);

        private string CreateTeaser(IProjectSettings projectSettings, string html)
        {
            // Try to get language metadata for humanizer.
            var languageCode = projectSettings.LanguageCode;
            var cultureInfo = CultureInfo.InvariantCulture;
            try
            {
                cultureInfo = new CultureInfo(languageCode);
            }
            catch { }

            var isRightToLeftLanguage = cultureInfo.TextInfo.IsRightToLeft;

            // Get global teaser settings.
            var autoTeaserMode = projectSettings.AutoTeaserMode;
            var teaserTruncationMode = projectSettings.TeaserTruncationMode;
            var teaserTruncationLength = projectSettings.TeaserTruncationLength <= 0 ? GetDefaultTeaserLength(teaserTruncationMode) : projectSettings.TeaserTruncationLength;

            // Truncate the raw content first. In general, Humanizer is smart enough to ignore tags, especially if using word truncation.
            var text = TruncatePost(teaserTruncationMode, html, teaserTruncationLength, isRightToLeftLanguage);

            var doc = new HtmlDocument();
            // Don't leave dangling <p> tags.
            HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;
            doc.LoadHtml(text);
            return doc.DocumentNode.InnerHtml;
        }

        private int GetDefaultTeaserLength(TeaserTruncationMode mode)
        {
            switch (mode)
            {
                case TeaserTruncationMode.Length:
                    return defaultLengthParagraphs;
                case TeaserTruncationMode.Character:
                    return defaultLengthCharacters;
                case TeaserTruncationMode.Word:
                default:
                    return defaultLengthWords;
            }
        }

        private string TruncatePost(TeaserTruncationMode mode, string content, int length, bool isRightToLeftLanguage = false)
        {
            var truncateFrom = isRightToLeftLanguage ? TruncateFrom.Left : TruncateFrom.Right;
            content = content ?? "";
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