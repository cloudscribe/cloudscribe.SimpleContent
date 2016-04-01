using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    /// <summary>
    /// Represents information that contains or links to the content of an <see cref="AtomEntry"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Atom Documents <b>must</b> conform to the following <i>processing model</i> rules. Atom Processors <b>must</b> interpret <see cref="AtomContent"/> according to the first applicable rule.
    ///         <list type="number">
    ///             <item>
    ///                 <description>
    ///                      If the value of the <see cref="ContentType"/> property is <b>text</b>, the value of the <see cref="Content"/> property <b>must not</b> contain child elements. 
    ///                      Such text is intended to be presented to humans in a readable fashion. Thus, Atom Processors <i>may</i> collapse white space (including line breaks), 
    ///                      and display the text using typographic techniques such as justification and proportional fonts.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                      If the value of the <see cref="ContentType"/> property is <b>html</b>, the value of the <see cref="Content"/> property <b>must not</b> contain child elements 
    ///                      and <i>should</i> be suitable for handling as HTML. The HTML markup <b>must</b> be escaped. The HTML markup <i>should</i> be such that it could validly appear 
    ///                      directly within an HTML <b>div</b> element. Atom Processors that display the content <i>may</i> use the markup to aid in displaying it.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                      If the value of the <see cref="ContentType"/> property is <b>xhtml</b>, the the value of the <see cref="Content"/> property <b>must</b> be a single XHTML div element 
    ///                      and <i>should</i> be suitable for handling as XHTML. The XHTML div element itself <b>must not</b> be considered part of the content. Atom Processors that display the 
    ///                      content <i>may</i> use the markup to aid in displaying it. The escaped versions of characters represent those characters, not markup.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                      If the value is an <a href="http://www.ietf.org/rfc/rfc3023.txt">XML media type</a> or ends with <b>+xml</b> or <b>/xml</b> (case insensitive), 
    ///                     the content <i>may</i> include child elements and <i>should</i> be suitable for handling as the indicated media type. 
    ///                     If the <see cref="AtomContent.Source"/> is not provided, this would normally mean that the <see cref="AtomContent.Content"/> would contain a 
    ///                     single child element that would serve as the root element of the XML document of the indicated type.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                      If the value begins with <b>text/</b> (case insensitive), the <see cref="AtomContent.Content"/> <b>must not</b> contain child elements.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                     For all other values , the <see cref="AtomContent.Content"/> <b>must</b> be a valid Base64 encoding, as described in 
    ///                     <a href="http://www.ietf.org/rfc/rfc3548.txt">RFC 3548: The Base16, Base32, and Base64 Data Encodings</a>, section 3. 
    ///                     When decoded, it <i>should</i> be suitable for handling as the indicated media type. In this case, the characters in 
    ///                     the Base64 encoding <i>may</i> be preceded and followed in the atom:content element by white space, and lines are 
    ///                     separated by a single newline (U+000A) character.
    ///                 </description>
    ///             </item>
    ///         </list>
    ///     </para>
    /// </remarks>
    public class AtomContent
    {

        public AtomContent()
        {
           
        }

        public AtomContent(string content)
        {
            this.Content = content;
        }

        public AtomContent(string content, string encoding) : this(content)
        {
            this.ContentType = encoding;
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

        private string contentValue = string.Empty;
        /// <summary>
        /// Gets or sets the local content of this entry.
        /// </summary>
        /// <value>The local content of this entry.</value>
        /// <remarks>
        ///     The <see cref="Content"/> property is <i>language-sensitive</i>, with the natural language of the value being specified by the <see cref="Language"/> property.
        /// </remarks>
        public string Content
        {
            get
            {
                return contentValue;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    contentValue = string.Empty;
                }
                else
                {
                    contentValue = value.Trim();
                }
            }
        }

        private string contentMediaType = string.Empty;
        /// <summary>
        /// Gets or sets a value indicating the entity encoding of this content.
        /// </summary>
        /// <value>A value indicating the entity encoding of this content.</value>
        /// <remarks>
        ///     <para>
        ///         The Atom specification defines three initial values for the type of entry content:
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      <i>html</i>: The content <b>must not</b> contain child elements and <i>should</i> be suitable for handling as HTML. 
        ///                      The HTML markup <b>must</b> be escaped, and <i>should</i> be such that it could validly appear directly within an HTML <b>div</b> element. 
        ///                      Atom Processors that display the content <i>may</i> use the markup to aid in displaying it.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      <i>text</i>: The content <b>must not</b> contain child elements. Such text is intended to be presented to humans in a readable fashion.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      <i>xhtml</i>: The content <b>must</b> be a single XHTML div element and <i>should</i> be suitable for handling as XHTML. 
        ///                      The XHTML div element itself <b>must not</b> be considered part of the content. Atom Processors that display the content 
        ///                      <i>may</i> use the markup to aid in displaying it. The escaped versions of characters represent those characters, not markup.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        ///     <para>
        ///         If the value is an <a href="http://www.ietf.org/rfc/rfc3023.txt">XML media type</a> or ends with <b>+xml</b> or <b>/xml</b> (case insensitive), 
        ///         the content <i>may</i> include child elements and <i>should</i> be suitable for handling as the indicated media type. 
        ///         If the <see cref="AtomContent.Source"/> is not provided, this would normally mean that the <see cref="AtomContent.Content"/> would contain a 
        ///         single child element that would serve as the root element of the XML document of the indicated type.
        ///     </para>
        ///     <para>
        ///         If the content type is not one of those specified above, it <b>must</b> conform to the syntax of a MIME media type, but <b>must not</b> be a composite type. 
        ///         See <a href="http://www.ietf.org/rfc/rfc4288.txt">RFC 4288: Media Type Specifications and Registration Procedures</a> for more details.
        ///     </para>
        ///     <para>
        ///         If the value begins with <b>text/</b> (case insensitive), the <see cref="AtomContent.Content"/> <b>must not</b> contain child elements.
        ///     </para>
        ///     <para>
        ///         For all other values , the <see cref="AtomContent.Content"/> <b>must</b> be a valid Base64 encoding, as described in 
        ///         <a href="http://www.ietf.org/rfc/rfc3548.txt">RFC 3548: The Base16, Base32, and Base64 Data Encodings</a>, section 3. 
        ///         When decoded, it <i>should</i> be suitable for handling as the indicated media type. In this case, the characters in 
        ///         the Base64 encoding <i>may</i> be preceded and followed in the atom:content element by white space, and lines are 
        ///         separated by a single newline (U+000A) character.
        ///     </para>
        ///     <para>
        ///         If neither the <see cref="AtomContent.ContentType"/> nor the <see cref="AtomContent.Source"/> is provided, 
        ///         Atom Processors <b>must</b> behave as though the<see cref="AtomContent.ContentType"/> property has a value of <i>text</i>.
        ///     </para>
        /// </remarks>
        public string ContentType
        {
            get
            {
                return contentMediaType;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    contentMediaType = string.Empty;
                }
                else
                {
                    contentMediaType = value.Trim();
                }
            }
        }

        private Uri contentSource;
        /// <summary>
        /// Gets or sets an IRI that identifies the remote location of this content.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that identifies the remote location of this content.</value>
        /// <remarks>
        ///     <para>
        ///         If a <see cref="AtomContent.Source"/> property is specified, the <see cref="AtomContent.Content"/> property <b>must</b> be empty. 
        ///         Atom Processors <i>may</i> use the IRI to retrieve the content and <i>may</i> choose to ignore remote content or to present it in a different manner than local content.
        ///     </para>
        ///     <para>
        ///         If a <see cref="AtomContent.Source"/> property is specified, the <see cref="AtomContent.ContentType"/> <i>should</i> be provided and <b>must</b> be a 
        ///         <a href="http://www.ietf.org/rfc/rfc4288.txt">MIME media type</a>, rather than <b>text</b>, <b>html</b>, or <b>xhtml</b>. The value is advisory; 
        ///         that is to say, when the corresponding URI (mapped from an IRI, if necessary) is dereferenced, if the server providing that content also provides 
        ///         a media type, the server-provided media type is authoritative.
        ///     </para>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        public Uri Source
        {
            get
            {
                return contentSource;
            }

            set
            {
                contentSource = value;
            }
        }

    }
}
