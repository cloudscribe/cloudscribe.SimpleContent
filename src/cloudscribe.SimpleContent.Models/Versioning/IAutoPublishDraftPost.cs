using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models.Versioning
{
    public interface IAutoPublishDraftPost
    {
        Task PublishIfNeeded(IPost post);
    }
}
