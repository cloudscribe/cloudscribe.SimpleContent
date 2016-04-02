using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Syndication.Models
{
    /// <summary>
    /// Specifies the web content syndication format that the syndicated content conforms to.
    /// </summary>
    /// <seealso cref="EnumerationMetadataAttribute"/>
    /// <seealso cref="MimeMediaTypeAttribute"/>
    //[Serializable()]
    public enum SyndicationContentFormat
    {
        /// <summary>
        /// No web content syndication format specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Attention Profiling Markup Language (APML) 1.0 syndication format.
        /// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apml")]
        //[EnumerationMetadata(DisplayName = "APML 1.0", AlternateValue = "APML")]
        //[MimeMediaType(Name = "text", SubName = "x-apml", Documentation = "http://www.apml.org")]
        Apml = 1,

        /// <summary>
        ///  Indicates that the syndication resource conforms to the Atom 1.0 syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "Atom 1.0", AlternateValue = "feed")]
        //[MimeMediaType(Name = "application", SubName = "atom+xml", Documentation = "http://www.atomenabled.org/developers/syndication/atom-format-spec.php")]
        Atom = 2,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Web Log Markup Language (BlogML) 2.0 syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "BlogML 2.0", AlternateValue = "blog")]
        //[MimeMediaType(Name = "application", SubName = "blog+xml", Documentation = "http://blogml.org")]
        BlogML = 3,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Microsummary Generator 0.1 syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "Microsummary Generator 0.1", AlternateValue = "generator")]
        //[MimeMediaType(Name = "application", SubName = "x.microsummary+xml", Documentation = "http://developer.mozilla.org/en/docs/Microsummary_XML_grammar_reference")]
        MicroSummaryGenerator = 4,

        /// <summary>
        /// Indicates that the syndication resource conforms to the News Markup Language (NewsML) G2 1.0 syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "NewsML-G2 1.0", AlternateValue = "NewsML")]
        //[MimeMediaType(Name = "text", SubName = "vnd.IPTC.NewsML", Documentation = "http://www.newsml.org")]
        NewsML = 5,

        /// <summary>
        /// Indicates that the syndication resource conforms to the OpenSearch Description 1.1 syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "OpenSearch Description 1.1", AlternateValue = "OpenSearchDescription")]
        //[MimeMediaType(Name = "application", SubName = "opensearchdescription+xml", Documentation = "http://www.opensearch.org/Specifications/OpenSearch/1.1#OpenSearch_description_document")]
        OpenSearchDescription = 6,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Outline Processor Markup Language (OPML) 2.0 syndication format.
        /// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Opml")]
        //[EnumerationMetadata(DisplayName = "OPML 2.0", AlternateValue = "opml")]
        //[MimeMediaType(Name = "text", SubName = "x-opml", Documentation = "http://www.opml.org/spec2")]
        Opml = 7,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Really Simple Discovery (RSD) 1.0 syndication format.
        /// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rsd")]
        //[EnumerationMetadata(DisplayName = "RSD 1.0", AlternateValue = "rsd")]
        //[MimeMediaType(Name = "application", SubName = "rsd+xml", Documentation = "http://cyber.law.harvard.edu/blogs/gems/tech/rsd.html")]
        Rsd = 8,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Really Simple Syndication (RSS) 2.0 syndication format.
        /// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rss")]
        //[EnumerationMetadata(DisplayName = "RSS 2.0", AlternateValue = "rss")]
        //[MimeMediaType(Name = "application", SubName = "rss+xml", Documentation = "http://www.rssboard.org/rss-specification")]
        Rss = 9,

        /// <summary>
        /// Indicates that the syndication resource conforms to the Resource Description Framework (RDF) 1.0 syndication format.
        /// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rdf")]
        //[EnumerationMetadata(DisplayName = "RDF 1.0", AlternateValue = "RDF")]
        //[MimeMediaType(Name = "application", SubName = "rdf+xml", Documentation = "http://w3.org/TR/2003/WD-rdf-concepts-20030123/#ref-rdf-mime-type")]
        Rdf = 10,

        /// <summary>
        ///  Indicates that the syndication resource conforms to the Atom Publishing Protocol 1.0 Category Document syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "Atom Publishing Category 1.0", AlternateValue = "categories")]
        //[MimeMediaType(Name = "application", SubName = "atomcat+xml", Documentation = "http://bitworking.org/projects/atom/rfc5023.html#iana-atomcat")]
        AtomCategoryDocument = 11,

        /// <summary>
        ///  Indicates that the syndication resource conforms to the Atom Publishing Protocol 1.0 Service Document syndication format.
        /// </summary>
        //[EnumerationMetadata(DisplayName = "Atom Publishing Service 1.0", AlternateValue = "service")]
        //[MimeMediaType(Name = "application", SubName = "atomsvc+xml", Documentation = "http://bitworking.org/projects/atom/rfc5023.html#iana-atomsvc")]
        AtomServiceDocument = 12
    }

}
