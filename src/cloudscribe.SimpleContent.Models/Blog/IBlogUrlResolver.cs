using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IBlogUrlResolver
    {
        Task<string> ResolveBlogUrl(IProjectSettings project);
        Task<string> ResolvePostUrl(IPost post, IProjectSettings projectSettings);
        Task ConvertToRelativeUrls(IPost post, IProjectSettings projectSettings);
    }
}
