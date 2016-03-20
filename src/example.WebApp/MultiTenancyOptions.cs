using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.WebApp
{
    public class MultiTenancyOptions
    {
        public List<SiteSettings> Tenants { get; set; }
    }
}
