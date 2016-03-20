

using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    public interface IXmlFileSystemOptionsResolver
    {
        Task<XmlFileSystemOptions> Resolve(string blogId);
    }
}