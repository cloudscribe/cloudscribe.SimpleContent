

namespace cloudscribe.MetaWeblog.Models
{
    /// <summary>
    /// MetaWeblog MediaInfo struct
    ///     returned from NewMediaObject call
    /// </summary>
    public struct MediaInfoStruct
    {
        /// <summary>
        ///     Url that points to Saved MediaObejct
        /// </summary>
        public string url;

        public string file;

        /// <summary>
        ///     Type of file
        /// </summary>
        public string type;
    }
}
