using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{

    /// <summary>
    /// Represents human-readable text.
    /// </summary>
    public class AtomTextConstruct
    {

        public AtomTextConstruct()
        {

        }

        public AtomTextConstruct(string content)
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

        private string textConstructContent = string.Empty;
        /// <summary>
        /// Gets or sets the content of this human-readable text.
        /// </summary>
        /// <value>The content of this human-readable text.</value>
        /// <remarks>
        ///     The <see cref="Content"/> property is <i>language-sensitive</i>, with the natural language of the value being specified by the <see cref="Language"/> property.
        /// </remarks>
        public string Content
        {
            get
            {
                return textConstructContent;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    textConstructContent = String.Empty;
                }
                else
                {
                    textConstructContent = value.Trim();
                }
            }
        }

        private AtomTextConstructType textConstructType = AtomTextConstructType.None;
        /// <summary>
        /// Gets or sets the entity encoding utilized by this human-readable text.
        /// </summary>
        /// <value>
        ///     An <see cref="AtomTextConstructType"/> enumeration value that represents the entity encoding utilized by this human-readable text. 
        ///     The default value is <see cref="AtomTextConstructType.None"/>.
        /// </value>
        public AtomTextConstructType TextType
        {
            get
            {
                return textConstructType;
            }

            set
            {
                textConstructType = value;
            }
        }

    }
}
