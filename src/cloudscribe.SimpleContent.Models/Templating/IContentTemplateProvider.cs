using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentTemplateProvider
    {
        Task<List<ContentTemplate>> GetAllTemplates();
    }
}
