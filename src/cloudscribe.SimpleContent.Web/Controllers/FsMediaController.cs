using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Linq;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    public class FsMediaController : Controller
    {
        public FsMediaController(
            IHostingEnvironment environment,
            IProjectService projectService,
            ILogger<FsMediaController> logger
            )
        {
            this.hosting = environment;
            this.projectService = projectService;
            log = logger;
        }

        private IHostingEnvironment hosting;
        private IProjectService projectService;
        private IProjectSettings settings = null;
        private ILogger log;

        private async Task EnsureProjectSettings()
        {
            if (settings != null) { return; }
            settings = await projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if (settings != null) { return; }

        }

        [HttpPost]
        public async Task<IActionResult> AutomaticUpload(
            //List<IFormFile> files
            )
        {
            var files = HttpContext.Request.Form.Files;

            await EnsureProjectSettings().ConfigureAwait(false);

            var imageList = new List<ImageUploadResult>();
            
            foreach (var formFile in files)
            {
                try
                {
                    if (formFile.Length > 0)
                    {
                        var newName = formFile.FileName.ToCleanFileName();
                        var newUrl = settings.LocalMediaVirtualPath + newName;
                        var fsPath = hosting.WebRootPath + newUrl.Replace('/', Path.DirectorySeparatorChar);

                        using (var stream = new FileStream(fsPath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        imageList.Add(new ImageUploadResult
                        {
                            OriginalSizeUrl = newUrl,
                            Name = newName,
                            Length = formFile.Length,
                            Type = formFile.ContentType

                        });

                    }
                }
                catch(Exception ex)
                {
                    log.LogError(MediaLoggingEvents.AUTOMATIC_UPLOAD, ex, ex.StackTrace);
                }
                
            }
            
            return Json(imageList);
        }
    }
}
