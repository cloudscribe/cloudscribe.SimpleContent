using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectService
    {
        Task Create(IProjectSettings project);
        Task Update(IProjectSettings project);
        Task<IProjectSettings> GetCurrentProjectSettings();
        Task<IProjectSettings> GetProjectSettings(string projectId);
        

        //void ClearNavigationCache();
    }
}