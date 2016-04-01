using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    public class AtomCategory
    {
        public AtomCategory()
        {
            
        }

        public AtomCategory(string term)
        {
            this.Term = term;
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

        private string categoryLabel = string.Empty;
        /// <summary>
        /// Gets or sets the human-readable label of this category for display in end-user applications.
        /// </summary>
        /// <value>The human-readable label of this category for display in end-user applications.</value>
        /// <remarks>
        ///     <para>
        ///         The <see cref="Label"/> property is <i>language-sensitive</i>, with the natural language of the value being specified by the <see cref="Language"/> property. 
        ///         Entities represent their corresponding characters, not markup.
        ///     </para>
        /// </remarks>
        public string Label
        {
            get
            {
                return categoryLabel;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    categoryLabel = string.Empty;
                }
                else
                {
                    categoryLabel = value.Trim();
                }
            }
        }

        private Uri categoryScheme;
        /// <summary>
        /// Gets or sets an IRI that identifies the categorization scheme used by this category.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that identifies the categorization scheme used by this category.</value>
        /// <remarks>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        public Uri Scheme
        {
            get
            {
                return categoryScheme;
            }

            set
            {
                categoryScheme = value;
            }
        }

        private string categoryTerm = string.Empty;
        /// <summary>
        /// Gets or sets a string that identifies this category.
        /// </summary>
        /// <value>A string that identifies this category.</value>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is an empty string.</exception>
        public string Term
        {
            get
            {
                return categoryTerm;
            }

            set
            {
                Guard.ArgumentNotNullOrEmptyString(value, "value");
                categoryTerm = value.Trim();
            }
        }


    }
}
