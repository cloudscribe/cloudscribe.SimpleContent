using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public class ConfigColumnTemplateOptionsProvider : IColumnTemplateOptionsProvider
    {
        public ConfigColumnTemplateOptionsProvider(
            IOptions<ColumnTemplateOptions> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private ColumnTemplateOptions _options;

        public Task<ColumnTemplateOptions> ResolveColumnTemplateOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(_options);
        }

    }
}
