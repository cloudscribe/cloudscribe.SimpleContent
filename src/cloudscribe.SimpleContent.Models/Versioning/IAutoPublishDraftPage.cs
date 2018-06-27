using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models.Versioning
{
    public interface IAutoPublishDraftPage
    {
        Task PublishIfNeeded(IPage page);
    }
}
