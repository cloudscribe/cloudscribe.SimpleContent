using System;
using System.Globalization;

namespace cloudscribe.Syndication.Models.Atom
{
    /// <summary>
    /// Represents a permanent, universally unique identifier for an <see cref="AtomEntry"/> or <see cref="AtomFeed"/>.
    /// </summary>
    /// <seealso cref="AtomEntry.Id"/>
    /// <seealso cref="AtomFeed.Id"/>
    /// <remarks>
    ///     <para>
    ///         When an <i>Atom Document</i> is relocated, migrated, syndicated, republished, exported, or imported, the content of its universally unique identifier <b>must not</b> change. 
    ///         Put another way, an <see cref="AtomId"/> pertains to all instantiations of a particular <see cref="AtomEntry"/> or <see cref="AtomFeed"/>; revisions retain the same 
    ///         content in their <see cref="AtomId"/> properties. It is suggested that the<see cref="AtomId"/> be stored along with the associated resource.
    ///     </para>
    ///     <para>
    ///         The content of an <see cref="AtomId"/> <b>must</b> be created in a way that assures uniqueness. 
    ///         Because of the risk of confusion between IRIs that would be equivalent if they were mapped to URIs and dereferenced, 
    ///         the following normalization strategy <i>should</i> be applied when generating unique identifiers: 
    ///         <list type="bullet">
    ///             <item>
    ///                 <description>
    ///                      Provide the scheme in lowercase characters.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                     Provide the host, if any, in lowercase characters.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                    Only perform percent-encoding where it is essential.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                     Use uppercase A through F characters when percent-encoding.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                    Prevent dot-segments from appearing in paths.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                     For schemes that define a default authority, use an empty authority if the default is desired.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                    For schemes that define an empty path to be equivalent to a path of "/", use "/".
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                     For schemes that define a port, use an empty port if the default is desired.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                    Preserve empty fragment identifiers and queries.
    ///                 </description>
    ///             </item>
    ///             <item>
    ///                 <description>
    ///                    Ensure that all components of the IRI are appropriately character normalized, e.g., by using NFC or NFKC.
    ///                 </description>
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Instances of <see cref="AtomId"/> objects can be compared to determine whether an entry or feed is the same as one seen before. 
    ///         Processors <b>must</b> compare <see cref="AtomId"/> objects on a character-by-character basis (in a case-sensitive fashion). 
    ///         Comparison operations <b>must</b> be based solely on the IRI character strings and <b>must not</b> rely on dereferencing the IRIs or URIs mapped from them.
    ///     </para>
    /// </remarks>
    public class AtomId
    {
        public AtomId()
        {
            
        }

        public AtomId(Uri uri)
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

        private Uri idUri;
        /// <summary>
        /// Gets or sets an IRI that represents a permanent, universally unique identifier for this entity.
        /// </summary>
        /// <value>A <see cref="Uri"/> that represents a Internationalized Resource Identifier (IRI) that represents a permanent, universally unique identifier for this entity.</value>
        /// <remarks>
        ///     <para>This <see cref="Uri"/> <b>must</b> represent an <i>absolute</i> URI.</para>
        ///     <para>See <a href="http://www.ietf.org/rfc/rfc3987.txt">RFC 3987: Internationalized Resource Identifiers</a> for the IRI technical specification.</para>
        ///     <para>See <a href="http://msdn2.microsoft.com/en-us/library/system.uri.aspx">System.Uri</a> for enabling support for IRIs within Microsoft .NET framework applications.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is a null reference (Nothing in Visual Basic).</exception>
        public Uri Uri
        {
            get
            {
                return idUri;
            }

            set
            {
                Guard.ArgumentNotNull(value, "value");
                idUri = value;
            }
        }

    }
}
