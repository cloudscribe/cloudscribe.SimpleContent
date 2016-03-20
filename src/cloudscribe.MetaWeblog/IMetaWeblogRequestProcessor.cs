
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public interface IMetaWeblogRequestProcessor
    {
        Task<MetaWeblogResult> ProcessRequest(
            MetaWeblogRequest input,
            MetaWeblogSecurityResult permission,
            CancellationToken cancellationToken);
    }
}