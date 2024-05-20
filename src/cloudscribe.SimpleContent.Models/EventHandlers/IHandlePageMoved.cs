using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models.EventHandlers
{
    public interface IHandlePageMoved
    {
        Task Handle(
            string projectId,
            IPage movedPage,
            IPage targetPage,
            string position,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
