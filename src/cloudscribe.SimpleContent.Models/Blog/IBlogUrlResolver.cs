using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IBlogUrlResolver
    {
        Task<string> ResolveBlogUrl(IProjectSettings project);
        Task<string> ResolvePostUrl(IPost post, IProjectSettings projectSettings);
        Task ConvertMediaToRelativeUrls(IPost post);
        Task ConvertMediaToAbsoluteUrls(IPost post, IProjectSettings projectSettings);
    }
}
