using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    public class AtomEntry
    {

        public AtomEntry()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEntry"/> class using the supplied <see cref="AtomId"/>, <see cref="AtomTextConstruct"/>, and <see cref="DateTime"/>.
        /// </summary>
        /// <param name="id">A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this entry.</param>
        /// <param name="title">A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this entry.</param>
        /// <param name="utcUpdatedOn">
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this entry was modified in a way the publisher considers significant. 
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </param>
        /// <exception cref="ArgumentNullException">The <paramref name="id"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="title"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomEntry(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
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


        private Collection<AtomPersonConstruct> entryAuthors;
        /// <summary>
        /// Gets or sets the authors of this entry.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the authors of this entry.</value>
        /// <remarks>
        ///     <para>
        ///        An entry <b>must</b> contain one or more authors, unless the entry contains an <see cref="AtomEntry.Source"/> object that contains an author or, 
        ///        in an Atom Feed Document, the <see cref="AtomFeed"/> contains an author itself.
        ///     </para>
        /// </remarks>
        public Collection<AtomPersonConstruct> Authors
        {
            get
            {
                if (entryAuthors == null)
                {
                    entryAuthors = new Collection<AtomPersonConstruct>();
                }
                return entryAuthors;
            }
        }

        private Collection<AtomCategory> entryCategories;
        /// <summary>
        /// Gets or sets the categories associated with this entry.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomCategory"/> objects that represent the categories associated with this entry.</value>
        public Collection<AtomCategory> Categories
        {
            get
            {
                if (entryCategories == null)
                {
                    entryCategories = new Collection<AtomCategory>();
                }
                return entryCategories;
            }
        }

        private AtomContent entryContent;
        /// <summary>
        /// Gets or sets information that contains or links to the content of this entry.
        /// </summary>
        /// <value>A <see cref="AtomContent"/> object that represents information that contains or links to the content of this entry.</value>
        public AtomContent Content
        {
            get
            {
                return entryContent;
            }

            set
            {
                entryContent = value;
            }
        }

        private Collection<AtomPersonConstruct> entryContributors;
        /// <summary>
        /// Gets or sets the entities who contributed to this entry.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the entities who contributed to this entry.</value>
        public Collection<AtomPersonConstruct> Contributors
        {
            get
            {
                if (entryContributors == null)
                {
                    entryContributors = new Collection<AtomPersonConstruct>();
                }
                return entryContributors;
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

        private AtomId entryId;
        /// <summary>
        /// Gets or sets a permanent, universally unique identifier for this entry.
        /// </summary>
        /// <value>A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this entry.</value>
        /// <remarks>
        ///     <para>
        ///         When an <i>Atom Document</i> is relocated, migrated, syndicated, republished, exported, or imported, the content of its universally unique identifier <b>must not</b> change. 
        ///         Put another way, an <see cref="AtomId"/> pertains to all instantiations of a particular <see cref="AtomEntry"/>; revisions retain the same 
        ///         content in their <see cref="AtomId"/> properties. It is suggested that the<see cref="AtomId"/> be stored along with the associated resource.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomId Id
        {
            get
            {
                return entryId;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                entryId = value;
            }
        }

        private Collection<AtomLink> entryLinks;
        /// <summary>
        /// Gets or sets references from this entry to one or more Web resources.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomLink"/> objects that represent references from this entry to one or more Web resources.</value>
        /// <remarks>
        ///     <para>
        ///         An entry <b>must not</b> contain more than one <see cref="AtomLink"/> with a <see cref="AtomLink.Relation"/> property of <i>alternate</i> 
        ///         that has the same combination of <see cref="AtomLink.ContentType"/> and <see cref="AtomLink.ContentLanguage"/> property values.
        ///     </para>
        /// </remarks>
        public Collection<AtomLink> Links
        {
            get
            {
                if (entryLinks == null)
                {
                    entryLinks = new Collection<AtomLink>();
                }
                return entryLinks;
            }
        }


        private DateTime entryPublishedOn = DateTime.MinValue;
        /// <summary>
        /// Gets or sets a date-time indicating an instant in time associated with an event early in the life cycle of this entry.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates an instant in time associated with an event early in the life cycle of this entry. 
        ///     The default value is <see cref="DateTime.MinValue"/>, which indicates that no publication time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime PublishedOn
        {
            get
            {
                return entryPublishedOn;
            }

            set
            {
                entryPublishedOn = value;
            }
        }

        private AtomTextConstruct entryRights;
        /// <summary>
        /// Gets or sets information about rights held in and over this entry.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information about rights held in and over this entry.</value>
        /// <remarks>
        ///     The <see cref="Rights"/> property <i>should not</i> be used to convey machine-readable licensing information. 
        ///     If an <see cref="AtomEntry"/> does not provide any rights information, then the <see cref="AtomFeed.Rights"/> of the containing feed, if present, is considered to apply to the entry.
        /// </remarks>
        public AtomTextConstruct Rights
        {
            get
            {
                return entryRights;
            }

            set
            {
                entryRights = value;
            }
        }


        private AtomSource entrySource;
        /// <summary>
        /// Gets or sets the meta-data of the source feed that this entry was copied from.
        /// </summary>
        /// <value>A <see cref="AtomSource"/> object that represents the meta-data of the source feed that this entry was copied from.</value>
        /// <remarks>
        ///     <para>
        ///         The <see cref="AtomSource"/> is designed to allow the aggregation of entries from different feeds while retaining information about an entry's source feed. 
        ///         For this reason, Atom Processors that are performing such aggregation <i>should</i> include at least the required feed-level meta-data elements 
        ///         (<see cref="AtomFeed.Id">id</see>, <see cref="AtomFeed.Title">title</see>, and <see cref="AtomFeed.UpdatedOn">updated</see>) in the <see cref="AtomSource"/>.
        ///     </para>
        /// </remarks>
        public AtomSource Source
        {
            get
            {
                return entrySource;
            }

            set
            {
                entrySource = value;
            }
        }

        private AtomTextConstruct entrySummary;
        /// <summary>
        /// Gets or sets information that conveys a short summary, abstract, or excerpt for this entry.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a short summary, abstract, or excerpt for this entry.</value>
        /// <remarks>
        ///     <para>
        ///         It is not advisable for the<see cref="Summary"/> property to duplicate <see cref="Title"/> or <see cref="Content"/> because Atom Processors might assume there is a useful summary when there is none.
        ///     </para>
        ///     <para>
        ///         Entries <b>must</b> contain a <see cref="Summary"/> in either of the following cases:
        ///         <list type="number">
        ///             <item>
        ///                 <description>
        ///                      The <see cref="AtomEntry"/> contains an <see cref="Content"/> property that has a <see cref="AtomContent.Source"/> property (and is thus empty).
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      The <see cref="AtomEntry"/> contains content that is encoded in Base64; i.e., the <see cref="AtomContent.ContentType"/> property of <see cref="Content"/> property 
        ///                      is a <a href="http://www.ietf.org/rfc/rfc4288.txt">MIME media type</a>, but is not an <a href="http://www.ietf.org/rfc/rfc3023.txt">XML media type</a>, 
        ///                      does not begin with <b>text/</b>, and does not end with <b>/xml</b> or <b>+xml</b>.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        public AtomTextConstruct Summary
        {
            get
            {
                return entrySummary;
            }

            set
            {
                entrySummary = value;
            }
        }

        private AtomTextConstruct entryTitle;
        /// <summary>
        /// Gets or sets information that conveys a human-readable title for this entry.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this entry.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomTextConstruct Title
        {
            get
            {
                return entryTitle;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                entryTitle = value;
            }
        }


        private DateTime entryUpdatedOn = DateTime.MinValue;
        /// <summary>
        /// Gets or sets a date-time indicating the most recent instant in time when this entry was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this entry was modified in a way the publisher considers significant. 
        ///     Publishers <i>may</i> change the value of this element over time. The default value is <see cref="DateTime.MinValue"/>, which indicates that no update time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime UpdatedOn
        {
            get
            {
                return entryUpdatedOn;
            }

            set
            {
                entryUpdatedOn = value;
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
