using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageUrlResolver
    {
        Task<string> ResolvePageUrl(IPage page);
        Task ConvertToRelativeUrls(IPage page, IProjectSettings projectSettings);
    }
}
