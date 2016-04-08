using System.Xml.Linq;
using Microsoft.AspNet.Mvc;

namespace cloudscribe.Syndication.Models.Rss
{
    public interface IXmlFormatter
    {
        XDocument BuildXml(RssChannel channel);
    }
}