using System.Xml.Linq;

namespace cloudscribe.MetaWeblog
{
    public interface IMetaWeblogRequestParser
    {
        MetaWeblogRequest ParseRequest(XDocument postedDocument);
    }
}