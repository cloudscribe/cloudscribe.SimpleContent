using cloudscribe.SimpleContent.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class DefaultAuthorNameResolver : IAuthorNameResolver
    {
        public Task<string> GetAuthorName(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = user.GetUserDisplayName();
            if(string.IsNullOrEmpty(result))
            {
                result = user.Identity.Name;
            }
            return Task.FromResult(result);
        }
    }
}
