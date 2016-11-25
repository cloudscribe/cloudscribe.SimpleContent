using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models.EventHandlers
{
    public interface IHandlePostPreUpdate
    {
        Task Handle(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
