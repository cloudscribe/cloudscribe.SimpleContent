using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Models
{
    public interface IMediaRootPathResolver
    {
        Task<MediaRootPathInfo> Resolve(CancellationToken cancellationToken = default(CancellationToken));
    }
}
