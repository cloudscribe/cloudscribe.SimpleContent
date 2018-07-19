using System.Threading;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentTemplateService
    {
        object DesrializeTemplateModel(IPage page, ContentTemplate template);
        object DesrializeTemplateModel(IPost post, ContentTemplate template);
        Task<int> GetCountOfTemplates(string projectId, string forFeature);
        Task<ContentTemplate> GetTemplate(string projectId, string key, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<ContentTemplate>> GetTemplates(string projectId, string forFeature, string query, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
    }
}
