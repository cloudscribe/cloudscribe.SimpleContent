using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public interface IGalleryOptionsProvider
    {
        Task<GalleryOptions> ResolveGalleryOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
