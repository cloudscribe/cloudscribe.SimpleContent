using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    /// <summary>
    /// Represents an Atom syndication feed, including metadata about the feed, and some or all of the entries associated with it.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Atom is an an XML-based Web content and metadata syndication format that describes lists of related information known as <i>feeds</i>. 
    ///         Feeds are composed of a number of items, known as <i>entries</i>, each with an extensible set of attached metadata.
    ///     </para>
    ///     <para>
    ///         This implementation conforms to the Atom 1.0 specification, which can be found 
    ///         at <a href="http://www.atomenabled.org/developers/syndication/atom-format-spec.php">http://www.atomenabled.org/developers/syndication/atom-format-spec.php</a>.
    ///     </para>
    ///     <para>
    ///         If multiple <see cref="AtomEntry"/> objects with the same <see cref="AtomEntry.Id"/> value appear in an Atom Feed Document, they represent the same entry. 
    ///         Their <see cref="AtomEntry.UpdatedOn"/> timestamps <i>should</i> be different. If an Atom Feed Document contains multiple entries with the same <see cref="AtomEntry.Id"/>, 
    ///         Atom Processors <u>may</u> choose to display all of them or some subset of them. One typical behavior would be to display only the entry with the latest <see cref="AtomEntry.UpdatedOn"/> timestamp.
    ///     </para>
    /// </remarks>
    public class AtomFeed
    {
        public AtomFeed()
        {
            
        }

        public AtomFeed(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
        {
            this.Id = id;
            this.Title = title;
            this.UpdatedOn = utcUpdatedOn;
        }

        private Uri commonObjectBaseUri;
        /// <summary>
        /// Gets or sets the base URI other than the base URI of the document or external entity.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a base URI other than the base URI of the document or external entity. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is interpreted as a URI Reference as defined in <a href="http://www.ietf.org/rfc/rfc2396.txt">RFC 2396: Uniform Resource Identifiers</a>, 
        ///         after processing according to <a href="http://www.w3.org/TR/xmlbase/#escaping">XML Base, Section 3.1 (URI Reference Encoding and Escaping)</a>.</para>
        /// </remarks>
        public Uri BaseUri
        {
            get
            {
                return commonObjectBaseUri;
            }

            set
            {
                commonObjectBaseUri = value;
            }
        }


        private CultureInfo commonObjectLanguage;
        /// <summary>
        /// Gets or sets the natural or formal language in which the content is written.
        /// </summary>
        /// <value>A <see cref="CultureInfo"/> that represents the natural or formal language in which the content is written. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is a language identifier as defined by <a href="http://www.ietf.org/rfc/rfc3066.txt">RFC 3066: Tags for the Identification of Languages</a>, or its successor.
        ///     </para>
        /// </remarks>
        public CultureInfo Language
        {
            get
            {
                return commonObjectLanguage;
            }

            set
            {
                commonObjectLanguage = value;
            }
        }

        private Collection<AtomPersonConstruct> feedAuthors;
        /// <summary>
        /// Gets or sets the authors of this feed.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the authors of this feed.</value>
        /// <remarks>
        ///     <para>A <see cref="AtomFeed"/> <b>must</b> contain one or more authors, unless all of the feeds's child <see cref="AtomEntry"/> objects contain at least one author.</para>
        ///     <para>
        ///         The <see cref="Authors"/> are considered to apply to any <see cref="AtomEntry"/> contained in this feed if the entry does not contain any authors and the entry's source does contain any authors.
        ///     </para>
        /// </remarks>
        public Collection<AtomPersonConstruct> Authors
        {
            get
            {
                if (feedAuthors == null)
                {
                    feedAuthors = new Collection<AtomPersonConstruct>();
                }
                return feedAuthors;
            }
        }

        private Collection<AtomCategory> feedCategories;
        /// <summary>
        /// Gets or sets the categories associated with this feed.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomCategory"/> objects that represent the categories associated with this feed.</value>
        public Collection<AtomCategory> Categories
        {
            get
            {
                if (feedCategories == null)
                {
                    feedCategories = new Collection<AtomCategory>();
                }
                return feedCategories;
            }
        }


        private Collection<AtomPersonConstruct> feedContributors;
        /// <summary>
        /// Gets or sets the entities who contributed to this feed.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the entities who contributed to this feed.</value>
        public Collection<AtomPersonConstruct> Contributors
        {
            get
            {
                if (feedContributors == null)
                {
                    feedContributors = new Collection<AtomPersonConstruct>();
                }
                return feedContributors;
            }
        }


        private IEnumerable<AtomEntry> feedEntries;
        /// <summary>
        /// Gets or sets the distinct content published in this feed.
        /// </summary>
        /// <value>A <see cref="IEnumerable{T}"/> collection of <see cref="AtomEntry"/> objects that represent distinct content published in this feed.</value>
        /// <remarks>
        ///     This <see cref="IEnumerable{T}"/> collection of <see cref="AtomEntry"/> objects is internally represented as a <see cref="Collection{T}"/> collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public IEnumerable<AtomEntry> Entries
        {
            get
            {
                if (feedEntries == null)
                {
                    feedEntries = new Collection<AtomEntry>();
                }
                return feedEntries;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                feedEntries = value;
            }

        }

        private static SyndicationContentFormat feedFormat = SyndicationContentFormat.Atom;
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

        private AtomGenerator feedGenerator;
        /// <summary>
        /// Gets or sets the agent used to generate this feed.
        /// </summary>
        /// <value>A <see cref="AtomGenerator"/> object that represents the agent used to generate this feed. The default value is a <b>null</b> reference.</value>
        public AtomGenerator Generator
        {
            get
            {
                return feedGenerator;
            }

            set
            {
                feedGenerator = value;
            }
        }


        private AtomId feedId;
        /// <summary>
        /// Gets or sets a permanent, universally unique identifier for this feed.
        /// </summary>
        /// <value>A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this feed.</value>
        /// <remarks>
        ///     <para>
        ///         When an <i>Atom Document</i> is relocated, migrated, syndicated, republished, exported, or imported, the content of its universally unique identifier <b>must not</b> change. 
        ///         Put another way, an <see cref="AtomId"/> pertains to all instantiations of a particular <see cref="AtomFeed"/>; revisions retain the same 
        ///         content in their <see cref="AtomId"/> properties. It is suggested that the<see cref="AtomId"/> be stored along with the associated resource.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomId Id
        {
            get
            {
                return feedId;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                feedId = value;
            }
        }

        private Collection<AtomLink> feedLinks;
        /// <summary>
        /// Gets or sets references from this feed to one or more Web resources.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomLink"/> objects that represent references from this feed to one or more Web resources.</value>
        /// <remarks>
        ///     <para>
        ///         A feed <i>should</i> contain one <see cref="AtomLink"/> with a <see cref="AtomLink.Relation"/> property of <i>self</i>. 
        ///         This is the preferred URI for retrieving Atom Feed Documents representing this Atom feed.
        ///     </para>
        ///     <para>
        ///         A feed <b>must not</b> contain more than one <see cref="AtomLink"/> with a <see cref="AtomLink.Relation"/> property of <i>alternate</i> 
        ///         that has the same combination of <see cref="AtomLink.ContentType"/> and <see cref="AtomLink.ContentLanguage"/> property values.
        ///     </para>
        /// </remarks>
        public Collection<AtomLink> Links
        {
            get
            {
                if (feedLinks == null)
                {
                    feedLinks = new Collection<AtomLink>();
                }
                return feedLinks;
            }
        }

        private AtomLogo feedLogo;
        /// <summary>
        /// Gets or sets an image that provides visual identification for this feed.
        /// </summary>
        /// <value>A <see cref="AtomLogo"/> object that represents an image that provides visual identification for this feed. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     The image <i>should</i> have an aspect ratio of 2 (horizontal) to 1 (vertical).
        /// </remarks>
        public AtomLogo Logo
        {
            get
            {
                return feedLogo;
            }

            set
            {
                feedLogo = value;
            }
        }

        private AtomTextConstruct feedRights;
        /// <summary>
        /// Gets or sets information about rights held in and over this feed.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information about rights held in and over this feed.</value>
        /// <remarks>
        ///     The <see cref="Rights"/> property <i>should not</i> be used to convey machine-readable licensing information.
        /// </remarks>
        public AtomTextConstruct Rights
        {
            get
            {
                return feedRights;
            }

            set
            {
                feedRights = value;
            }
        }

        private AtomTextConstruct feedSubtitle;
        /// <summary>
        /// Gets or sets information that conveys a human-readable description or subtitle for this feed.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable description or subtitle for this feed.</value>
        public AtomTextConstruct Subtitle
        {
            get
            {
                return feedSubtitle;
            }

            set
            {
                feedSubtitle = value;
            }
        }

        private AtomTextConstruct feedTitle;
        /// <summary>
        /// Gets or sets information that conveys a human-readable title for this feed.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this feed.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomTextConstruct Title
        {
            get
            {
                return feedTitle;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                feedTitle = value;
            }
        }

        private DateTime feedUpdatedOn = DateTime.MinValue;
        /// <summary>
        /// Gets or sets a date-time indicating the most recent instant in time when this feed was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this feed was modified in a way the publisher considers significant. 
        ///     Publishers <i>may</i> change the value of this element over time. The default value is <see cref="DateTime.MinValue"/>, which indicates that no update time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime UpdatedOn
        {
            get
            {
                return feedUpdatedOn;
            }

            set
            {
                feedUpdatedOn = value;
            }
        }

        private static Version feedVersion = new Version(1, 0);
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
