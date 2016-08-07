using System.Collections.Generic;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectService
    {
        Task Create(ProjectSettings project);
        Task Update(ProjectSettings project);
        Task<ProjectSettings> GetCurrentProjectSettings();
        Task<ProjectSettings> GetProjectSettings(string projectId);
        Task<List<ProjectSettings>> GetUserProjects(string userName, string password);
    }
}