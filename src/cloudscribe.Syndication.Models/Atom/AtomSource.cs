using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    public class AtomSource
    {

        public AtomSource()
        {
           
        }

        public AtomSource(AtomId id, AtomTextConstruct title, DateTime utcUpdatedOn)
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

        private Collection<AtomPersonConstruct> sourceAuthors;
        /// <summary>
        /// Gets or sets the authors of this source.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the authors of this source.</value>
        public Collection<AtomPersonConstruct> Authors
        {
            get
            {
                if (sourceAuthors == null)
                {
                    sourceAuthors = new Collection<AtomPersonConstruct>();
                }
                return sourceAuthors;
            }
        }

        private Collection<AtomCategory> sourceCategories;
        /// <summary>
        /// Gets or sets the categories associated with this source.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomCategory"/> objects that represent the categories associated with this source.</value>
        public Collection<AtomCategory> Categories
        {
            get
            {
                if (sourceCategories == null)
                {
                    sourceCategories = new Collection<AtomCategory>();
                }
                return sourceCategories;
            }
        }


        private Collection<AtomPersonConstruct> sourceContributors;
        /// <summary>
        /// Gets or sets the entities who contributed to this source.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomPersonConstruct"/> objects that represent the entities who contributed to this source.</value>
        public Collection<AtomPersonConstruct> Contributors
        {
            get
            {
                if (sourceContributors == null)
                {
                    sourceContributors = new Collection<AtomPersonConstruct>();
                }
                return sourceContributors;
            }
        }

        private AtomGenerator sourceGenerator;
        /// <summary>
        /// Gets or sets the agent used to generate this source.
        /// </summary>
        /// <value>A <see cref="AtomGenerator"/> object that represents the agent used to generate this source. The default value is a <b>null</b> reference.</value>
        public AtomGenerator Generator
        {
            get
            {
                return sourceGenerator;
            }

            set
            {
                sourceGenerator = value;
            }
        }

        private AtomIcon sourceIcon;
        /// <summary>
        /// Gets or sets an image that provides iconic visual identification for this source.
        /// </summary>
        /// <value>A <see cref="AtomIcon"/> object that represents an image that provides iconic visual identification for this source. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     The image <i>should</i> have an aspect ratio of one (horizontal) to one (vertical) and <i>should</i> be suitable for presentation at a small size.
        /// </remarks>
        public AtomIcon Icon
        {
            get
            {
                return sourceIcon;
            }

            set
            {
                sourceIcon = value;
            }
        }

        private AtomId sourceId;
        /// <summary>
        /// Gets or sets a permanent, universally unique identifier for this source.
        /// </summary>
        /// <value>A <see cref="AtomId"/> object that represents a permanent, universally unique identifier for this source.</value>
        public AtomId Id
        {
            get
            {
                return sourceId;
            }

            set
            {
                sourceId = value;
            }
        }


        private Collection<AtomLink> sourceLinks;
        /// <summary>
        /// Gets or sets references from this source to one or more Web resources.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> collection of <see cref="AtomLink"/> objects that represent references from this source to one or more Web resources.</value>
        public Collection<AtomLink> Links
        {
            get
            {
                if (sourceLinks == null)
                {
                    sourceLinks = new Collection<AtomLink>();
                }
                return sourceLinks;
            }
        }

        private AtomLogo sourceLogo;
        /// <summary>
        /// Gets or sets an image that provides visual identification for this source.
        /// </summary>
        /// <value>A <see cref="AtomLogo"/> object that represents an image that provides visual identification for this source. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     The image <i>should</i> have an aspect ratio of 2 (horizontal) to 1 (vertical).
        /// </remarks>
        public AtomLogo Logo
        {
            get
            {
                return sourceLogo;
            }

            set
            {
                sourceLogo = value;
            }
        }


        private AtomTextConstruct sourceRights;
        /// <summary>
        /// Gets or sets information about rights held in and over this source.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information about rights held in and over this source.</value>
        /// <remarks>
        ///     The <see cref="Rights"/> property <i>should not</i> be used to convey machine-readable licensing information.
        /// </remarks>
        public AtomTextConstruct Rights
        {
            get
            {
                return sourceRights;
            }

            set
            {
                sourceRights = value;
            }
        }


        private AtomTextConstruct sourceSubtitle;
        /// <summary>
        /// Gets or sets information that conveys a human-readable description or subtitle for this source.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable description or subtitle for this source.</value>
        public AtomTextConstruct Subtitle
        {
            get
            {
                return sourceSubtitle;
            }

            set
            {
                sourceSubtitle = value;
            }
        }


        private AtomTextConstruct sourceTitle;
        /// <summary>
        /// Gets or sets information that conveys a human-readable title for this source.
        /// </summary>
        /// <value>A <see cref="AtomTextConstruct"/> object that represents information that conveys a human-readable title for this source.</value>
        public AtomTextConstruct Title
        {
            get
            {
                return sourceTitle;
            }

            set
            {
                sourceTitle = value;
            }
        }


        private DateTime sourceUpdatedOn = DateTime.MinValue;
        /// <summary>
        /// Gets or sets a date-time indicating the most recent instant in time when this source was modified in a way the publisher considers significant.
        /// </summary>
        /// <value>
        ///     A <see cref="DateTime"/> that indicates the most recent instant in time when this source was modified in a way the publisher considers significant. 
        ///     Publishers <i>may</i> change the value of this element over time. The default value is <see cref="DateTime.MinValue"/>, which indicates that no update time was provided.
        /// </value>
        /// <remarks>
        ///     The <see cref="DateTime"/> should be provided in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime UpdatedOn
        {
            get
            {
                return sourceUpdatedOn;
            }

            set
            {
                sourceUpdatedOn = value;
            }
        }


    }
}
