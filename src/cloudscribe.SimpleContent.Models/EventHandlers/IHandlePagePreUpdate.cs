using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models.EventHandlers
{
    public interface IHandlePagePreUpdate
    {
        Task Handle(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
