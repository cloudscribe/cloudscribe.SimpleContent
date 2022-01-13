using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public class ConfigGalleryOptionsProvider : IGalleryOptionsProvider
    {
        public ConfigGalleryOptionsProvider(
            IOptions<GalleryOptions> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private GalleryOptions _options;

        public Task<GalleryOptions> ResolveGalleryOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(_options);
        }


    }
}
