using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    public class AtomIcon
    {

        public AtomIcon()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomIcon"/> class using the supplied <see cref="Uri"/>.
        /// <param name="uri">A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that identifies an image that provides iconic visual identification for this feed.</param>
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="uri"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomIcon(Uri uri)
        {
            this.Uri = uri;
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

        private Uri iconUri;
        /// <summary>
        /// Gets or sets an IRI that identifies an image that provides iconic visual identification for this feed.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that identifies an image that provides iconic visual identification for this feed.</value>
        /// <remarks>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Uri
        {
            get
            {
                return iconUri;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                iconUri = value;
            }
        }

    }
}
