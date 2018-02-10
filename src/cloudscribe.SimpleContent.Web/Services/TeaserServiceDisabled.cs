using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// This implementation can be registered with DI if you want to completely disable the teaser generation and always use full content
    /// </summary>
    public class TeaserServiceDisabled : ITeaserService
    {
        public TeaserServiceDisabled()
        {

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
            result.Content = html;
            result.DidTruncate = false;

            return result;

        }
    }
}
