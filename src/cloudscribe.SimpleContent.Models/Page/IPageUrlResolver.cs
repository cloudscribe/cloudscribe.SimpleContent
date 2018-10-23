using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageUrlResolver
    {
        Task<string> ResolvePageUrl(IPage page);
        Task ConvertMediaToRelativeUrls(IPage page);
        Task ConvertMediaToAbsoluteUrls(IPage page, IProjectSettings projectSettings);
    }
}
