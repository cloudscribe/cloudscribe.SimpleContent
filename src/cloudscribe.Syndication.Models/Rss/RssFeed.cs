using System;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents a Really Simple Syndication (RSS) syndication feed.
    /// </summary>
    public class RssFeed
    {
        public RssFeed()
        {
            
        }

        public RssFeed(Uri link, string title)
        {
            this.Channel.Link = link;
            this.Channel.Title = title;
        }

        public RssFeed(string description)
        {
            this.Channel.Description = description;
        }

        private RssChannel feedChannel = new RssChannel();
        /// <summary>
        /// Gets or sets information about the meta-data and contents of the feed.
        /// </summary>
        /// <value>A <see cref="RssChannel"/> object that represents information about the meta-data and contents of the feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssChannel Channel
        {
            get
            {
                return feedChannel;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                feedChannel = value;
            }
        }

        private static SyndicationContentFormat feedFormat = SyndicationContentFormat.Rss;
        /// <summary>
        /// Gets the <see cref="SyndicationContentFormat"/> that this syndication resource implements.
        /// </summary>
        /// <value>The <see cref="SyndicationContentFormat"/> enumeration value that indicates the type of syndication format that this syndication resource implements.</value>
        public SyndicationContentFormat Format
        {
            get
            {
                return feedFormat;
            }
        }

        private static Version feedVersion = new Version(2, 0);
        /// <summary>
        /// Gets the <see cref="Version"/> of the <see cref="SyndicationContentFormat"/> that this syndication resource conforms to.
        /// </summary>
        /// <value>The <see cref="Version"/> of the <see cref="SyndicationContentFormat"/> that this syndication resource conforms to. The default value is <b>2.0</b>.</value>
        public Version Version
        {
            get
            {
                return feedVersion;
            }
        }

    }
}
