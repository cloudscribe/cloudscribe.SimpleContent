using System;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents a means of uniquely identifying a <see cref="RssItem"/>.
    /// </summary>
    public class RssGuid
    {
        public RssGuid()
        {
           
        }

        public RssGuid(string value)
        {
            this.Value = value;
        }

        public RssGuid(string value, bool isPermanentUrl) : this(value)
        {
            this.IsPermanentLink = isPermanentUrl;
        }

        private bool guidIsPermalink = true;
        /// <summary>
        /// Gets or sets a value indicating if the guid represents a permanent URL of a web page associated with this item.
        /// </summary>
        /// <value><b>true</b> if the guid <see cref="RssGuid.Value">value</see> represents a permanent URL of a web page; otherwise <b>false</b>.</value>
        /// <remarks>
        ///     If set to <b>false</b>, the guid may employ any syntax the feed's publisher has devised for ensuring the uniqueness of the string, 
        ///     such as the <a href="http://www.faqs.org/rfcs/rfc4151.html">Tag URI scheme</a> described in RFC 4151.
        /// </remarks>
        public bool IsPermanentLink
        {
            get
            {
                return guidIsPermalink;
            }

            set
            {
                guidIsPermalink = value;
            }
        }

        private string guidIdentifier = String.Empty;
        /// <summary>
        /// Gets or sets a string value that uniquely identifies this item.
        /// </summary>
        /// <value>A string value that uniquely identifies this item.</value>
        /// <remarks>
        ///     <para>
        ///         If the guid's <see cref="RssGuid.IsPermanentLink"/> property has a value of <b>true</b>, the <see cref="RssGuid.Value"/> property <b>must</b> be 
        ///         the permanent URL of the web page associated with this item. Otherwise the <see cref="RssGuid.Value"/> property may employ any syntax the feed's publisher 
        ///         has devised for ensuring the uniqueness of the string.
        ///     </para>
        ///     <para>
        ///         When choosing to employ a syntax for ensuring the uniqueness of the string, the <a href="http://www.faqs.org/rfcs/rfc4151.html">Tag URI scheme</a> 
        ///         described in RFC 4151 is recommended.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Value
        {
            get
            {
                return guidIdentifier;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                guidIdentifier = value.Trim();
            }
        }

    }
}
