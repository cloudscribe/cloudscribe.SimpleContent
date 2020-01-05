

namespace cloudscribe.MetaWeblog.Models
{
    /// <summary>
    /// MetaWeblog MediaObject struct
    ///     passed in the newMediaObject call
    /// </summary>
    public struct MediaObjectStruct
    {
        /// <summary>
        ///     Media object bytes
        /// </summary>
        public byte[] bytes;

        /// <summary>
        ///     Name of media object (filename)
        /// </summary>
        public string name;

        /// <summary>
        ///     Type of file
        /// </summary>
        public string type;
    }
}
