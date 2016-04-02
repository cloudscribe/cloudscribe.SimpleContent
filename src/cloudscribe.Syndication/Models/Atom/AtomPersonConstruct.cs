using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    /// <summary>
    /// Represents a person, corporation, or similar entity.
    /// </summary>
    public class AtomPersonConstruct
    {
        public AtomPersonConstruct()
        {
        }

        public AtomPersonConstruct(string name)
        {
            this.Name = name;
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

        private string personConstructEmailAddress = string.Empty;
        /// <summary>
        /// Gets or sets the e-mail address associated with this entity.
        /// </summary>
        /// <value>The e-mail address associated with this entity.</value>
        /// <remarks>
        ///     The email address <b>must</b> conform to <a href="http://www.ietf.org/rfc/rfc2822.txt">RFC 2822: Internet Message Format, 3.4.1, Addr-spec Specification</a>.
        /// </remarks>
        public string EmailAddress
        {
            get
            {
                return personConstructEmailAddress;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    personConstructEmailAddress = string.Empty;
                }
                else
                {
                    personConstructEmailAddress = value.Trim();
                }
            }
        }

        private string personConstructName;
        /// <summary>
        /// Gets or sets the human-readable name for this entity.
        /// </summary>
        /// <value>The human-readable name for this entity.</value>
        /// <remarks>
        ///     The <see cref="Name"/> property is <i>language-sensitive</i>, with the natural language of the value being specified by the <see cref="Language"/> property.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Name
        {
            get
            {
                return personConstructName;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                personConstructName = value.Trim();
            }
        }

        private Uri personConstructUri;
        /// <summary>
        /// Gets or sets the IRI associated with this entity.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) associated with this entity.</value>
        /// <remarks>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        public Uri Uri
        {
            get
            {
                return personConstructUri;
            }

            set
            {
                personConstructUri = value;
            }
        }


    }
}
