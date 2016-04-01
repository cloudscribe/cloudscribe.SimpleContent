using System;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents the source feed that an <see cref="RssItem"/> was republished from.
    /// </summary>
    public class RssSource
    {

        public RssSource()
        {
            
        }

        public RssSource(Uri url)
        {
            this.Url = url;
        }

        public RssSource(Uri url, string title) : this(url)
        {
            this.Title = title;
        }

        private string sourceTitle = string.Empty;
        /// <summary>
        /// Gets or sets the title of the source feed.
        /// </summary>
        /// <value>The title of the source feed.</value>
        public string Title
        {
            get
            {
                return sourceTitle;
            }

            set
            {
                sourceTitle = value.Trim();
            }
        }

        private Uri sourceUrl;
        /// <summary>
        /// Gets or sets the URL of the source feed.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of the source feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Url
        {
            get
            {
                return sourceUrl;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                sourceUrl = value;
            }
        }

    }
}
