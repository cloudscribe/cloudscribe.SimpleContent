using cloudscribe.MetaWeblog;
using cloudscribe.MetaWeblog.Controllers;
using cloudscribe.MetaWeblog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Controllers
{
    public class FolderMetaweblogController : MetaWeblogController
    {
        public FolderMetaweblogController(
            IWebHostEnvironment appEnv,
            IMetaWeblogRequestParser metaWeblogRequestParser,
            IMetaWeblogRequestProcessor metaWeblogProcessor,
            IMetaWeblogResultFormatter metaWeblogResultFormatter,
            IMetaWeblogSecurity metaWeblogSecurity,
            IMetaWeblogRequestValidator metaWebLogRequestValidator,
            ILogger<MetaWeblogController> logger,
            IOptions<ApiOptions> optionsAccessor = null
            ):base(appEnv, metaWeblogRequestParser, metaWeblogProcessor, metaWeblogResultFormatter, metaWeblogSecurity, metaWebLogRequestValidator, logger, optionsAccessor)
        {

        }

        [HttpPost]
        //[Route("{folder:sitefolder}/api/metaweblog")]
        public override async Task<IActionResult> Index()
        {
            return await base.Index();
        }


    }
}
