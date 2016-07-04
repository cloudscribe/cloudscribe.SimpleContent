using System.Collections.Generic;

namespace WebApp
{
    public class MultiTenancyOptions
    {
        public List<SiteSettings> Tenants { get; set; }
    }
}
