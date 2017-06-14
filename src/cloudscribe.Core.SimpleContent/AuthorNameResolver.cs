using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class AuthorNameResolver : IAuthorNameResolver
    {
        public AuthorNameResolver(IUserContextResolver userContextResolver)
        {
            userResolver = userContextResolver;
        }

        private IUserContextResolver userResolver;

        public async Task<string> GetAuthorName(ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dbUser = await userResolver.GetCurrentUser(cancellationToken);
            string result;
            if(dbUser == null)
            {
                result = user.GetUserDisplayName();
                if (string.IsNullOrEmpty(result))
                {
                    result = user.Identity.Name;
                }
                return result;

            }

            if(!string.IsNullOrWhiteSpace(dbUser.FirstName))
            {
                if(!string.IsNullOrWhiteSpace(dbUser.LastName))
                {
                    return dbUser.FirstName + " " + dbUser.LastName;
                }
            }

            return dbUser.DisplayName;
            
        }


    }
}
