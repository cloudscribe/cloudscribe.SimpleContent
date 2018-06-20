using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class ConfigContentTemplateProvider : IContentTemplateProvider
    {
        public ConfigContentTemplateProvider(
            IOptions<ContentTemplateConfig> configAccessor
            )
        {
            _configuredTemplates = configAccessor.Value;
        }

        private ContentTemplateConfig _configuredTemplates;

        public Task<List<ContentTemplate>> GetAllTemplates(string projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = _configuredTemplates.Templates
                .Where(x =>
                  x.ProjectId == "*" || x.ProjectId == projectId
                ).OrderBy(x => x.Title)
                .ToList();

            return Task.FromResult(result);

        }

        public Task<ContentTemplate> GetTemplate(string projectId, string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = _configuredTemplates.Templates
                .Where(x =>
                  (x.ProjectId == "*" || x.ProjectId == projectId)
                  && x.Key == key
                ).FirstOrDefault();

            return Task.FromResult(result);
        }
    }
}
