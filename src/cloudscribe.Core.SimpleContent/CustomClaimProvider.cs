

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using cloudscribe.Core.Identity;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class CustomClaimProvider : ICustomClaimProvider
    {
        public CustomClaimProvider(
            IOptions<List<CustomClaimMap>> claimMapsAccessor
            )
        {
            claimMaps = claimMapsAccessor.Value;
        }

        private List<CustomClaimMap> claimMaps;

        public Task AddClaims(SiteUser user, ClaimsIdentity identity)
        {
            foreach(var map in claimMaps)
            {
                if(map.UserEmail == user.Email)
                {
                    foreach(var c in map.Claims)
                    {
                        identity.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
                    }
                }
            }
            
            return Task.FromResult(0);
        }

        
    }

    public class CustomClaimMap
    {
        public string UserEmail { get; set; }
        
        public List<CustomClaim> Claims { get; set; }
    }

    public class CustomClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
