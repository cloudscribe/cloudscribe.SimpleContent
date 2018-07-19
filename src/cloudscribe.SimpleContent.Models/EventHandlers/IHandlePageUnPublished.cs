using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models.EventHandlers
{
    public interface IHandlePageUnPublished
    {
        Task Handle(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
