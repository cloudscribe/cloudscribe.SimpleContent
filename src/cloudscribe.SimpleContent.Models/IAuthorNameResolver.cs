using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IAuthorNameResolver
    {
        Task<string> GetAuthorName(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken));
    }

}
