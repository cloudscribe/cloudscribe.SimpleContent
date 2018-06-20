using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentTemplateProvider
    {
        Task<List<ContentTemplate>> GetAllTemplates(string projectId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ContentTemplate> GetTemplate(string projectId, string key, CancellationToken cancellationToken = default(CancellationToken));

    }
}
