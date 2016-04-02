using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    /// <summary>
    /// Represents a reference from an <see cref="AtomEntry"/> or <see cref="AtomFeed"/> to a Web resource.
    /// </summary>
    public class AtomLink
    {
        public AtomLink()
        {
            
        }

        public AtomLink(Uri href)
        {
            this.Uri = href;
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

        private CultureInfo linkResourceLanguage;
        /// <summary>
        /// Gets or sets the natural or formal language in which this Web resource content is written.
        /// </summary>
        /// <value>A <see cref="CultureInfo"/> that represents the natural or formal language in which this resource content is written. The default value is a <b>null</b> reference.</value>
        /// <remarks>
        ///     <para>
        ///         The value of this property is a language identifier as defined by <a href="http://www.ietf.org/rfc/rfc3066.txt">RFC 3066: Tags for the Identification of Languages</a>, or its successor.
        ///     </para>
        /// </remarks>
        public CultureInfo ContentLanguage
        {
            get
            {
                return linkResourceLanguage;
            }

            set
            {
                linkResourceLanguage = value;
            }
        }

        private string linkMediaType = string.Empty;
        /// <summary>
        /// Gets or sets an advisory media type for this Web resource.
        /// </summary>
        /// <value>An advisory MIME media type that provides a hint about the type of the representation that is expected to be returned by the Web resource.</value>
        /// <remarks>
        ///     The advisory media type <b>does not</b> override the actual media type returned with the representation. 
        ///     The value <b>must</b> conform to the syntax of a MIME media type as specified by <a href="http://www.ietf.org/rfc/rfc4288.txt">RFC 4288: Media Type Specifications and Registration Procedures</a>.
        /// </remarks>
        public string ContentType
        {
            get
            {
                return linkMediaType;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    linkMediaType = string.Empty;
                }
                else
                {
                    linkMediaType = value.Trim();
                }
            }
        }

        private long linkLength = long.MinValue;
        /// <summary>
        /// Gets or sets an advisory length for this Web resource content in octets.
        /// </summary>
        /// <value>An advisory length for this Web resource content in octets. The default value is <see cref="Int64.MinValue"/>, which indicates that no advisory length was specified.</value>
        /// <remarks>
        ///     The <see cref="Length"/> does not override the actual content length of the representation as reported by the underlying protocol.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is less than <i>zero</i>.</exception>
        public long Length
        {
            get
            {
                return linkLength;
            }

            set
            {
                Guard.ArgumentNotLessThan(value, "value", 0);
                linkLength = value;
            }
        }

        private string linkRelation = string.Empty;
        /// <summary>
        /// Gets or sets a value that indicates the link relation type of this Web resource.
        /// </summary>
        /// <value>A value that indicates the link relation type.</value>
        /// <remarks>
        ///     <para>If the <see cref="Relation"/> property is not specified, the <see cref="AtomLink"/> <b>must</b> be interpreted as if the link relation type is <i>alternate</i>.</para>
        ///     <para>
        ///         The value of the <see cref="Relation"/> property <b>must</b> be a string that is non-empty and matches either the <i>isegment-nz-nc</i> or 
        ///         the <i>IRI</i> production in <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers (IRIs)</a>. 
        ///         Note that use of a relative reference other than a simple name is not allowed. If a name is given, implementations <b>must</b> consider the link relation type equivalent 
        ///         to the same name registered within the IANA Registry of Link Relations (<a href="http://www.atomenabled.org/developers/syndication/atom-format-spec.php#IANA">Section 7</a>), 
        ///         and thus to the IRI that would be obtained by appending the value of the rel attribute to the string "<i>http://www.iana.org/assignments/relation/</i>". 
        ///         The value of <see cref="Relation"/> property describes the meaning of the link, but does not impose any behavioral requirements on Atom Processors.
        ///     </para>
        ///     <para>
        ///         The Atom specification defines five initial values for the Registry of Link Relations:
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      <i>alternate</i>: Signifies that the IRI in the value of the <see cref="AtomLink.Uri"/> property 
        ///                      identifies an alternate version of the resource described by the containing element.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      <i>related</i>: Signifies that the IRI in the value of the <see cref="AtomLink.Uri"/> property 
        ///                      identifies a resource related to the resource described by the containing element.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      <i>self</i>: Signifies that the IRI in the value of the <see cref="AtomLink.Uri"/> property 
        ///                      identifies a resource equivalent to the containing element.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      <i>enclosure</i>: Signifies that the IRI in the value of the <see cref="AtomLink.Uri"/> property identifies 
        ///                      a related resource that is potentially large in size and might require special handling.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      <i>via</i>: Signifies that the IRI in the value of the <see cref="AtomLink.Uri"/> property identifies 
        ///                      a resource that is the source of the information provided in the containing element.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        public string Relation
        {
            get
            {
                return linkRelation;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    linkRelation = string.Empty;
                }
                else
                {
                    linkRelation = value.Trim();
                }
            }
        }

        private string linkTitle = string.Empty;
        /// <summary>
        /// Gets or sets human-readable information about this Web resource.
        /// </summary>
        /// <value>Human-readable information about this Web resource.</value>
        /// <remarks>
        ///     The <see cref="Title"/> property is <i>language-sensitive</i>, with the natural language of the value being specified by the <see cref="Language"/> property. 
        ///     Entities represent their corresponding characters, not markup.
        /// </remarks>
        public string Title
        {
            get
            {
                return linkTitle;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    linkTitle = string.Empty;
                }
                else
                {
                    linkTitle = value.Trim();
                }
            }
        }

        private Uri linkResourceLocation;
        /// <summary>
        /// Gets or sets an IRI that identifies the location of this Web resource.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that identifies the location of this Web resource.</value>
        /// <remarks>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Uri
        {
            get
            {
                return linkResourceLocation;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                linkResourceLocation = value;
            }
        }


    }
}
