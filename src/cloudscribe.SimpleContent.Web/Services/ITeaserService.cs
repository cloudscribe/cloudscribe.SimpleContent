using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// Service which extracts abbreviated "teaser" text from blog postings for display on index/listing views.
    /// </summary>
    public interface ITeaserService
    {
        /// <summary>
        /// Checks project/post settings to see if a teaser should be displayed. If so, then returns the abbreviated teaser HTML. Otherwise returns the unmodified HTML.
        /// </summary>
        /// <param name="projectSettings">SimpleContent project settings.</param>
        /// <param name="post">SimpleContent blog posting.</param>
        /// <param name="html">The final HTML which would be rendered for the blog posting.</param>
        /// <returns>Abbreviated teaser or unmodified HTML.</returns>
        string CreateTeaserIfNeeded(IProjectSettings projectSettings, IPost post, string html);
    }
}