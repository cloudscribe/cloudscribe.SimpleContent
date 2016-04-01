using System;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents a media object such as an audio, video, or executable file that can be associated with 
    /// an <see cref="RssItem"/>.
    /// </summary>
    public class RssEnclosure
    {
        public RssEnclosure()
        {
           
        }

        public RssEnclosure(long length, string type, Uri url)
        {
            this.ContentType = type;
            this.Length = length;
            this.Url = url;
        }

        private string enclosureType = string.Empty;
        /// <summary>
        /// Gets or sets the media object's MIME content type.
        /// </summary>
        /// <value>The media object's MIME content type.</value>
        /// <remarks>
        ///     See <a href="http://www.iana.org/assignments/media-types/">http://www.iana.org/assignments/media-types/</a> for a listing of the registered IANA MIME media types.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string ContentType
        {
            get
            {
                return enclosureType;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                enclosureType = value.Trim();
            }
        }

        private long enclosureLength = long.MinValue;
        /// <summary>
        /// Gets or sets the size of the media object.
        /// </summary>
        /// <value>The size, in bytes, of the media object. The default value is <see cref="Int64.MinValue"/>, which indicates that no size was specified.</value>
        /// <remarks>
        ///     <para>
        ///         Though an enclosure <b>must</b> specify its size with the length attribute, the size of some media objects cannot be determined by an RSS publisher. 
        ///         When an enclosure's size cannot be determined, a publisher <i>should</i> use a length of 0.
        ///     </para>
        ///     <para>
        ///         The peer-to-peer file-sharing protocol BitTorrent deploys files using a small key file called a torrent that tells a client how to find and download the file. 
        ///         When an enclosure is delivered in a multi-step process like the one used by BitTorrent, the length <i>should</i> be the size 
        ///         of the first file that must be downloaded to begin the process.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is less than <i>zero</i>.</exception>
        public long Length
        {
            get
            {
                return enclosureLength;
            }

            set
            {
                Guard.ArgumentNotLessThan(value, "value", 0);
                enclosureLength = value;
            }
        }

        private Uri enclosureUrl;
        /// <summary>
        /// Gets or sets the URL of the media object.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of the media object.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Url
        {
            get
            {
                return enclosureUrl;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                enclosureUrl = value;
            }
        }

    }
}
