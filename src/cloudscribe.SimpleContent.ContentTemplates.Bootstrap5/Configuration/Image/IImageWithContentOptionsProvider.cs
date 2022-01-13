using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public interface IImageWithContentOptionsProvider
    {
        Task<ImageWithContentOptions> ResolveImageWithContentOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
