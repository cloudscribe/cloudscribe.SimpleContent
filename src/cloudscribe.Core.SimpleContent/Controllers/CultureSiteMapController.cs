using cloudscribe.Web.SiteMap;
using cloudscribe.Web.SiteMap.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Controllers
{
    public class CultureSiteMapController : SiteMapController
    {
        public CultureSiteMapController(
            ILogger<SiteMapController> logger,
            IEnumerable<ISiteMapNodeService> nodeProviders = null
            ):base(logger, nodeProviders)
        {

        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "SiteMapCacheProfile")]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

    }
}
