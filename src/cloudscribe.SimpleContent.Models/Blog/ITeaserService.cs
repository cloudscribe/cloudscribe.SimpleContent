namespace cloudscribe.SimpleContent.Models
{
    /// <summary>
    /// Service which extracts abbreviated "teaser" text from html content for display on index/listing views.
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

    }
}
