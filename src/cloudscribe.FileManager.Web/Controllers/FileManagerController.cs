// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-14
// Last Modified:           2017-02-15
// 

using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Controllers
{
    public class FileManagerController : Controller
    {
        public FileManagerController(
            FileManagerService fileManagerService,
            IOptions<AutomaticUploadOptions> autoUploadOptionsAccessor,
            ILogger<FileManagerController> logger
            )
        {
            this.fileManagerService = fileManagerService;
            autoUploadOptions = autoUploadOptionsAccessor.Value;
            log = logger;
        }

        private FileManagerService fileManagerService;
        private AutomaticUploadOptions autoUploadOptions;
        private ILogger log;

        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public IActionResult FileDialog()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> AutomaticUpload(
            //List<IFormFile> files
            )
        {
            var files = HttpContext.Request.Form.Files;
            var imageList = new List<ImageUploadResult>();
            
            foreach (var formFile in files)
            {
                try
                {
                    if (formFile.Length > 0)
                    {
                        var uploadResult = await fileManagerService.ProcessFile(
                            formFile,
                            autoUploadOptions,
                            MediaLoggingEvents.AUTOMATIC_UPLOAD
                            ).ConfigureAwait(false);
                        
                        imageList.Add(uploadResult);

                    }
                }
                catch (Exception ex)
                {
                    log.LogError(MediaLoggingEvents.AUTOMATIC_UPLOAD, ex, ex.StackTrace);
                }

            }

            return Json(imageList);
        }

        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> GetFileTreeJson(string virtualStartPath = "")
        {
            var list = await fileManagerService.GetFileTree(virtualStartPath).ConfigureAwait(false);

            return Json(list);
        }

    }
}
