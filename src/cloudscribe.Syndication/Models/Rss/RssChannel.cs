using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

//http://www.mikesdotnetting.com/article/174/generating-rss-and-atom-feeds-in-webmatrix

namespace cloudscribe.Syndication.Models.Rss
{
    /// <summary>
    /// Represents information about the meta-data and contents associated to an <see cref="RssFeed"/>.
    /// </summary>
    public class RssChannel
    {

        public RssChannel()
        {
           
        }

        public RssChannel(Uri link, string title, string description)
        {
            this.Link = link;
            this.Title = title;
            this.Description = description;
        }

        private Collection<RssCategory> channelCategories = null;
        /// <summary>
        /// Gets the categories or tags to which this channel belongs.
        /// </summary>
        /// <value>
        ///     A <see cref="Collection{T}"/> collection of <see cref="RssCategory"/> objects that represent the categories to which this channel belongs. The default value is an <i>empty</i> collection.
        /// </value>
        public Collection<RssCategory> Categories
        {
            get
            {
                if (channelCategories == null)
                {
                    channelCategories = new Collection<RssCategory>();
                }
                return channelCategories;
            }
        }

        private string channelCopyrightNotice = string.Empty;
        /// <summary>
        /// Gets or sets the human-readable copyright statement that applies to this feed.
        /// </summary>
        /// <value>The human-readable copyright statement that applies to this feed.</value>
        /// <remarks>
        ///     When a feed lacks a copyright element, aggregators <i>should not</i> assume that is in the public domain and can be republished and redistributed without restriction.
        /// </remarks>
        public string Copyright
        {
            get
            {
                return channelCopyrightNotice;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    channelCopyrightNotice = string.Empty;
                }
                else
                {
                    channelCopyrightNotice = value.Trim();
                }
            }
        }

        private string channelDescription = string.Empty;
        /// <summary>
        /// Gets or sets character data that provides a human-readable characterization or summary of this feed.
        /// </summary>
        /// <value>Character data that provides a human-readable characterization or summary of this feed.</value>
        /// <remarks>
        ///     The description character data <b>must</b> be suitable for presentation as HTML.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Description
        {
            get
            {
                return channelDescription;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                channelDescription = value.Trim();
            }
        }

        private string channelGenerator = "cloudscribe.Syndication";
        /// <summary>
        /// Gets or sets a value that credits the software that created this feed.
        /// </summary>
        /// <value>A value that credits the software that created this feed. The default value is an agent that describes this syndication framework.</value>
        public string Generator
        {
            get
            {
                return channelGenerator;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    channelGenerator = string.Empty;
                }
                else
                {
                    channelGenerator = value.Trim();
                }
            }
        }

        private RssImage channelImage;
        /// <summary>
        /// Gets or sets the graphical logo for this feed.
        /// </summary>
        /// <value>
        ///     A <see cref="RssImage"/> object that represents the graphical logo for this feed. The default value is a <b>null</b> reference.
        /// </value>
        public RssImage Image
        {
            get
            {
                return channelImage;
            }

            set
            {
                channelImage = value;
            }
        }

        private IEnumerable<RssItem> channelItems;
        /// <summary>
        /// Gets or sets the distinct content published in this feed.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}"/> collection of <see cref="RssItem"/> objects that represent distinct content published in this feed.</value>
        /// <remarks>
        ///     This <see cref="IEnumerable{T}"/> collection of <see cref="RssItem"/> objects is internally represented as a <see cref="Collection{T}"/> collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public IEnumerable<RssItem> Items
        {
            get
            {
                if (channelItems == null)
                {
                    channelItems = new Collection<RssItem>();
                }
                return channelItems;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                channelItems = value;
            }
        }

        private CultureInfo channelLanguage;
        /// <summary>
        /// Gets or sets the natural language employed in this feed.
        /// </summary>
        /// <value>A <see cref="CultureInfo"/> object that represents the natural language employed in this feed. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     The language <b>must</b> be identified using one of the <a href="http://www.rssboard.org/rss-language-codes">RSS language codes</a> 
        ///     or a <a href="http://www.w3.org/TR/REC-html40/struct/dirlang.html#langcodes">W3C language code</a>.
        /// </remarks>
        public CultureInfo Language
        {
            get
            {
                if (channelLanguage == null) return new CultureInfo("en-US");
                return channelLanguage;
            }

            set
            {
                channelLanguage = value;
            }
        }

        private DateTime channelLastBuildDate = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the last date and time the content of this feed was updated.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> object that represents the last date and time the content of this feed was updated. 
        ///     The default value is <see cref="DateTime.MinValue"/>, which indicates that no last build date was specified.
        /// </value>
        public DateTime LastBuildDate
        {
            get
            {
                return channelLastBuildDate;
            }

            set
            {
                channelLastBuildDate = value;
            }
        }

        private Uri channelLink;
        /// <summary>
        /// Gets or sets the URL of the web site associated with this feed.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents the URL of the web site associated with this feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Link
        {
            get
            {
                return channelLink;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                channelLink = value;
            }
        }

        private string channelManagingEditor = string.Empty;
        /// <summary>
        /// Gets or sets the e-mail address of the person to contact regarding the editorial content of this feed.
        /// </summary>
        /// <value>The e-mail address of the person to contact regarding the editorial content of this feed.</value>
        /// <remarks>
        ///     <para>
        ///         There is no requirement to follow a specific format for email addresses. Publishers can format addresses according to the RFC 2822 Address Specification, 
        ///         the RFC 2368 guidelines for mailto links, or some other scheme. The recommended format for e-mail addresses is <i>username@hostname.tld (Real Name)</i>.
        ///     </para>
        /// </remarks>
        public string ManagingEditor
        {
            get
            {
                return channelManagingEditor;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    channelManagingEditor = string.Empty;
                }
                else
                {
                    channelManagingEditor = value.Trim();
                }
            }
        }

        private DateTime channelPublicationDate = DateTime.MinValue;
        /// <summary>
        /// Gets or sets the publication date and time of this feed's content.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> object that represents the publication date and time of this feed's content. 
        ///     The default value is <see cref="DateTime.MinValue"/>, which indicates that no publication date was specified.
        /// </value>
        /// <remarks>
        ///     Publishers of daily, weekly or monthly periodicals can use this element to associate feed items with the date they most recently went to press.
        /// </remarks>
        public DateTime PublicationDate
        {
            get
            {
                return channelPublicationDate;
            }

            set
            {
                channelPublicationDate = value;
            }
        }

        private string channelRating = string.Empty;
        /// <summary>
        /// Gets or sets an advisory label for the content in this feed.
        /// </summary>
        /// <value>A string value, formatted according to the specification for the Platform for Internet Content Selection (PICS), that supplies an advisory label for the content in this feed.</value>
        /// <remarks>
        ///     <para>
        ///         For further information on the <b>Platform for Internet Content Selection (PICS)</b> advisory label formatting specification, 
        ///         see <a href="http://www.w3.org/TR/REC-PICS-labels#General">http://www.w3.org/TR/REC-PICS-labels#General</a>.
        ///     </para>
        /// </remarks>
        public string Rating
        {
            get
            {
                return channelRating;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    channelRating = string.Empty;
                }
                else
                {
                    channelRating = value.Trim();
                }
            }
        }

        private Uri channelSelfLink;
        /// <summary>
        /// Gets or sets a URL that describes the feed itself.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a URL that points to where this feed can be retrieved from.</value>
        /// <remarks>
        ///     <para>
        ///         Identifying a feed's URL within the feed makes it more portable, self-contained, and easier to cache. 
        ///         For these reasons, a feed <i>should</i> provide a value for <see cref="SelfLink"/> that is used for this purpose.
        ///     </para>
        ///     <para>
        ///         Identifying a self referential link is achieved by including a <i>atom:link</i> element within the channel. 
        ///         See <a href="http://www.rssboard.org/rss-profile#namespace-elements-atom-link">RSS Profile</a> for more information.
        ///     </para>
        /// </remarks>
        public Uri SelfLink
        {
            get
            {
                return channelSelfLink;
            }

            set
            {
                channelSelfLink = value;
            }
        }

        private Collection<DayOfWeek> channelSkipDays;
        /// <summary>
        /// Gets or sets the days of the week during which this feed is not updated.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="DayOfWeek"/> enumeration values that indicate the days of the week during which this feed is not updated.</value>
        /// <remarks>
        ///     <see cref="DayOfWeek"/> enumeration values within this collection <b>must not</b> be duplicated.
        /// </remarks>
        public Collection<DayOfWeek> SkipDays
        {
            get
            {
                if (channelSkipDays == null)
                {
                    channelSkipDays = new Collection<DayOfWeek>();
                }
                return channelSkipDays;
            }
        }

        private Collection<int> channelSkipHours;
        /// <summary>
        /// Gets or sets the hours of the day during which this feed is not updated.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="Int32"/> objects that indicate the hours of the day during which this feed is not updated.</value>
        /// <remarks>
        ///     Values from 0 to 23 are permitted, with 0 representing midnight. Integer values within this collection <b>must not</b> be duplicated.
        /// </remarks>
        public Collection<int> SkipHours
        {
            get
            {
                if (channelSkipHours == null)
                {
                    channelSkipHours = new Collection<int>();
                }
                return channelSkipHours;
            }
        }

        private RssTextInput channelTextInput;
        /// <summary>
        /// Gets or sets a form to submit a text query to this feed's publisher over the Common Gateway Interface (CGI).
        /// </summary>
        /// <value>
        ///     A <see cref="TextInput"/> object that represents a form to submit a text query to this feed's publisher over the Common Gateway Interface (CGI). 
        ///     The default value is a <b>null</b> reference.
        /// </value>
        public RssTextInput TextInput
        {
            get
            {
                return channelTextInput;
            }

            set
            {
                channelTextInput = value;
            }
        }

        private int channelTimeToLive = int.MinValue;
        /// <summary>
        /// Gets or sets the maximum number of minutes to cache the data before a client should request it again.
        /// </summary>
        /// <value>
        ///     The maximum number of minutes to cache the data before an aggregator should request it again. 
        ///     The default value is <see cref="Int32.MinValue"/>, which indicates no time-to-live was specified.
        /// </value>
        /// <remarks>
        ///     Aggregators that support this property <i>should</i> treat it as a publisher's suggestion of a feed's update frequency, not a hard rule.
        /// </remarks>
        public int TimeToLive
        {
            get
            {
                return channelTimeToLive;
            }

            set
            {
                channelTimeToLive = value;
            }
        }

        private string channelTitle = string.Empty;
        /// <summary>
        /// Gets or sets character data that provides the name of this feed.
        /// </summary>
        /// <value>Character data that provides the name of this feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Title
        {
            get
            {
                return channelTitle;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                channelTitle = value.Trim();
            }
        }

        private string channelWebmaster = string.Empty;
        /// <summary>
        /// Gets or sets the e-mail address of the person to contact about technical issues regarding this feed.
        /// </summary>
        /// <value>The e-mail address of the person to contact about technical issues regarding this feed.</value>
        /// <remarks>
        ///     <para>
        ///         There is no requirement to follow a specific format for email addresses. Publishers can format addresses according to the RFC 2822 Address Specification, 
        ///         the RFC 2368 guidelines for mailto links, or some other scheme. The recommended format for e-mail addresses is <i>username@hostname.tld (Real Name)</i>.
        ///     </para>
        /// </remarks>
        public string Webmaster
        {
            get
            {
                return channelWebmaster;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    channelWebmaster = string.Empty;
                }
                else
                {
                    channelWebmaster = value.Trim();
                }
            }
        }


    }
}
