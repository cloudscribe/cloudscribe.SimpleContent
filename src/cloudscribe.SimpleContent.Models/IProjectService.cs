using System.Collections.Generic;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectService
    {
        Task Create(IProjectSettings project);
        Task Update(IProjectSettings project);
        Task<IProjectSettings> GetCurrentProjectSettings();
        Task<IProjectSettings> GetProjectSettings(string projectId);
        Task<List<IProjectSettings>> GetUserProjects(string userName, string password);

        void ClearNavigationCache();
    }
}