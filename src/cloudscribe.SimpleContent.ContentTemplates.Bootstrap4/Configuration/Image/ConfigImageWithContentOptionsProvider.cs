using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public class ConfigImageWithContentOptionsProvider : IImageWithContentOptionsProvider
    {
        public ConfigImageWithContentOptionsProvider(
            IOptions<ImageWithContentOptions> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private ImageWithContentOptions _options;

        public Task<ImageWithContentOptions> ResolveImageWithContentOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(_options);
        }
    }
}
