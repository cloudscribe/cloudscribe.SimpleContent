using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// Service which extracts abbreviated "teaser" text from blog postings for display on index/listing views.
    /// </summary>
    public interface ITeaserService
    {
        /// <summary>
        /// Generates a teaser based on the provided parameters.
        /// </summary>
        /// <param name="truncationMode"></param>
        /// <param name="truncationLength"></param>
        /// <param name="html"></param>
        /// <param name="cacheKey">typically should use the post id</param>
        /// <param name="slug">used only for logging warnings</param>
        /// <param name="languageCode">used to determine of right to left processing should be used</param>
        /// <returns></returns>
        TeaserResult GenerateTeaser(
            TeaserTruncationMode truncationMode,
            int truncationLength,
            string html,
            string cacheKey,
            string slug,
            string languageCode);

        /// <summary>
        /// Checks project/post settings to see if a teaser should be displayed. If so, then returns the abbreviated teaser HTML. Otherwise returns the unmodified HTML.
        /// </summary>
        /// <param name="projectSettings">SimpleContent project settings.</param>
        /// <param name="post">SimpleContent blog posting.</param>
        /// <param name="html">The final HTML which would be rendered for the blog posting.</param>
        /// <returns>Abbreviated teaser or unmodified HTML.</returns>
        string CreateTeaserIfNeeded(IProjectSettings projectSettings, IPost post, string html);

        /// <summary>
        /// Checks project/post settings to see if a teaser should be displayed.
        /// </summary>
        /// <param name="projectSettings">SimpleContent project settings.</param>
        /// <param name="post">SimpleContent blog posting.</param>
        /// <returns>True if a teaser should be displayed; otherwise, false.</returns>
        bool ShouldDisplayTeaser(IProjectSettings projectSettings, IPost post);
        bool ShouldDisplayReadMore(IProjectSettings projectSettings, IPost post);
    }
}