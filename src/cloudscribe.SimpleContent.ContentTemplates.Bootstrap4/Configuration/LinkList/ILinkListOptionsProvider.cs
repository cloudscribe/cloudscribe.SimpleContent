using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public interface ILinkListOptionsProvider
    {
        Task<LinkListOptions> ResolveLinkListOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
