using System;
using System.Collections.ObjectModel;

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents distinct content published in an <see cref="RssFeed"/> such as a news article, 
    /// weblog entry or some other form of discrete update.
    /// </summary>
    public class RssItem
    {
        public RssItem()
        {
           
        }

        private string itemAuthor = string.Empty;
        /// <summary>
        /// Gets or sets the e-mail address of the person who wrote this item.
        /// </summary>
        /// <value>The e-mail address of the person who wrote this item.</value>
        /// <remarks>
        ///     <para>
        ///         There is no requirement to follow a specific format for email addresses. Publishers can format addresses according to the RFC 2822 Address Specification, 
        ///         the RFC 2368 guidelines for mailto links, or some other scheme. The recommended format for e-mail addresses is <i>username@hostname.tld (Real Name)</i>.
        ///     </para>
        ///     <para>
        ///         A feed published by an individual <i>should</i> omit the item <see cref="RssItem.Author">author</see> 
        ///         and use the <see cref="RssChannel.ManagingEditor"/> or <see cref="RssChannel.Webmaster"/> channel properties to provide contact information.
        ///     </para>
        /// </remarks>
        public string Author
        {
            get
            {
                return itemAuthor;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    itemAuthor = string.Empty;
                }
                else
                {
                    itemAuthor = value.Trim();
                }
            }
        }

        private Collection<RssCategory> itemCategories;
        /// <summary>
        /// Gets the categories or tags to which this item belongs.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> of <see cref="RssCategory"/> objects that represent the categories to which this item belongs. The default value is an <i>empty</i> collection.
        /// </value>
        public Collection<RssCategory> Categories
        {
            get
            {
                if (itemCategories == null)
                {
                    itemCategories = new Collection<RssCategory>();
                }
                return itemCategories;
            }
        }

        private Uri itemComments;
        /// <summary>
        /// Gets or sets the URL of a web page that contains comments received in response to this item.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of a web page that contains comments received in response to this item.</value>
        public Uri Comments
        {
            get
            {
                return itemComments;
            }

            set
            {
                itemComments = value;
            }
        }

        private string itemDescription = string.Empty;
        /// <summary>
        /// Gets or sets character data that contains this item's full content or a summary of its contents.
        /// </summary>
        /// <value>Character data that contains this item's full content or a summary of its contents.</value>
        /// <remarks>
        ///     <para>The description <i>may</i> be empty if the item specifies a <see cref="RssItem.Title"/>.</para>
        ///     <para>
        ///         The description <b>must</b> be suitable for presentation as HTML. 
        ///         HTML markup must be encoded as character data either by employing the <b>HTML entities</b> (&lt; and &gt;) <i>or</i> a <b>CDATA</b> section.
        ///     </para>
        ///     <para>
        ///         The description <i>should not</i> contain relative URLs, because the RSS format does not provide a means to identify the base URL of a document. 
        ///         When a relative URL is present, an aggregator <i>may</i> attempt to resolve it to a full URL using the channel's <see cref="RssChannel.Link">link</see> as the base.
        ///     </para>
        /// </remarks>
        public string Description
        {
            get
            {
                return itemDescription;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    itemDescription = string.Empty;
                }
                else
                {
                    itemDescription = value.Trim();
                }
            }
        }

        private Collection<RssEnclosure> itemEnclosures;
        /// <summary>
        /// Gets the media objects associated with this item.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> of <see cref="RssEnclosure"/> objects that represent the media objects such as an audio, video, or executable file that are associated with this item. 
        ///     The default value is an <i>empty</i> collection.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Support for the enclosure element in RSS software varies significantly because of disagreement over whether the specification permits more than one enclosure per item. 
        ///         Although the original author intended to permit no more than one enclosure in each item, this limit is not explicit in the specification. 
        ///         For best support in the widest number of aggregators, an item <i>should not</i> contain more than one enclosure.
        ///     </para>
        /// </remarks>
        public Collection<RssEnclosure> Enclosures
        {
            get
            {
                if (itemEnclosures == null)
                {
                    itemEnclosures = new Collection<RssEnclosure>();
                }
                return itemEnclosures;
            }
        }

        private RssGuid itemGuid;
        /// <summary>
        /// Gets or sets the unique identifier for this item.
        /// </summary>
        /// <value>
        ///     A <see cref="RssGuid"/> object that represents the unique identifier for this item. The default value is a <b>null</b> reference.
        /// </value>
        /// <remarks>
        ///     A publisher <i>should</i> provide a guid for each item.
        /// </remarks>
        public RssGuid Guid
        {
            get
            {
                return itemGuid;
            }

            set
            {
                itemGuid = value;
            }
        }

        private Uri itemLink;
        /// <summary>
        /// Gets or sets the URL of a web page associated with this item.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of a web page associated with this item.</value>
        public Uri Link
        {
            get
            {
                return itemLink;
            }

            set
            {
                itemLink = value;
            }
        }

        private DateTime itemPublicationDate = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the publication date and time of this item.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> object that represents the publication date and time of this item. 
        ///     The default value is <see cref="DateTime.MinValue"/>, which indicates that no publication date was specified.
        /// </value>
        /// <remarks>
        ///     The specification recommends that aggregators <i>should</i> ignore items with a publication date that occurs in the future, 
        ///     providing a means for publishers to embargo an item until that date. However, it is recommended that publishers <i>should not</i> 
        ///     include items in a feed until they are ready for publication.
        /// </remarks>
        public DateTime PublicationDate
        {
            get
            {
                return itemPublicationDate;
            }

            set
            {
                itemPublicationDate = value;
            }
        }

        private RssSource itemSource;
        /// <summary>
        /// Gets or sets the source feed that this item was republished from.
        /// </summary>
        /// <value>
        ///     A <see cref="RssSource"/> object that represents the source feed that this item was republished from. The default value is a <b>null</b> reference.
        /// </value>
        public RssSource Source
        {
            get
            {
                return itemSource;
            }

            set
            {
                itemSource = value;
            }
        }

        private string itemTitle = string.Empty;
        /// <summary>
        /// Gets or sets character data that provides this item's headline.
        /// </summary>
        /// <value>Character data that provides this item's headline.</value>
        /// <remarks>
        ///     This property is optional if the item contains a <see cref="RssItem.Description"/>.
        /// </remarks>
        public string Title
        {
            get
            {
                return itemTitle;
            }

            set
            {
                itemTitle = value.Trim();
            }
        }


    }
}
