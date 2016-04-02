using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    /// <summary>
    /// Represents an agent used to generate an <see cref="AtomFeed"/>, for debugging and other purposes.
    /// </summary>
    public class AtomGenerator
    {

        public AtomGenerator()
        {
            
        }

        public AtomGenerator(string content)
        {
            this.Content = content;
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

        private string generatorText = "cloudscribe.Syndication";
        /// <summary>
        /// Gets or sets a human-readable name for the generating agent.
        /// </summary>
        /// <value>A human-readable name for the generating agent.</value>
        /// <remarks>
        ///     Entities represent their corresponding characters, not markup.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Content
        {
            get
            {
                return generatorText;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                generatorText = value.Trim();
            }
        }

        private Uri generatorUri;
        /// <summary>
        /// Gets or sets an IRI that is relevant to the generating agent.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that is relevant to the generating agent.</value>
        /// <remarks>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        public Uri Uri
        {
            get
            {
                return generatorUri;
            }

            set
            {
                generatorUri = value;
            }
        }

        private string generatorVersion = "1.0";
        /// <summary>
        /// Gets or sets the version of the generating agent.
        /// </summary>
        /// <value>The version of the generating agent.</value>
        public string Version
        {
            get
            {
                return generatorVersion;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    generatorVersion = string.Empty;
                }
                else
                {
                    generatorVersion = value.Trim();
                }
            }
        }

    }
}
