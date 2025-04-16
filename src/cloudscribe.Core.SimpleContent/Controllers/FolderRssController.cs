using cloudscribe.Syndication.Models.Rss;
using cloudscribe.Syndication.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Controllers
{
    public class FolderRssController : RssController
    {
        public FolderRssController(
            ILogger<RssController> logger,
            IWebHostEnvironment env,
            IEnumerable<IChannelProvider> channelProviders = null,
            IChannelProviderResolver channelResolver = null,
            IXmlFormatter xmlFormatter = null
            ):base(logger, env, channelProviders, channelResolver, xmlFormatter)
        {

        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "RssCacheProfile")]
        //[Route("{folder:sitefolder}/api/rss")]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }

    }
}
