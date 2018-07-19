using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public class ConfigLinkListOptionsProvider : ILinkListOptionsProvider
    {
        public ConfigLinkListOptionsProvider(
            IOptions<LinkListOptions> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private LinkListOptions _options;

        public Task<LinkListOptions> ResolveLinkListOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(_options);
        }

    }
}
