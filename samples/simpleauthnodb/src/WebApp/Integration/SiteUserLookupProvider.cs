using cloudscribe.Web.SimpleAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class SiteUserLookupProvider : IUserLookupProvider
    {
        public SiteUserLookupProvider(SiteSettings tenant)
        {
            this.tenant = tenant;
        }

        private SiteSettings tenant;

        public SimpleAuthUser GetUser(string userName)
        {
            foreach (SimpleAuthUser u in tenant.Users)
            {
                if (u.UserName == userName) { return u; }
            }

            return null;
        }
    }
}
