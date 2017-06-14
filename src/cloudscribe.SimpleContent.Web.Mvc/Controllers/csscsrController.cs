// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2017-06-09
// 

using cloudscribe.Web.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace cloudscribe.SimpleContent.Web.Mvc.Controllers
{
    /// <summary>
    /// csscsr:cloudscribe SimpleContent static resource controller
    /// 
    /// </summary>
    public class csscsrController : Controller
    {
        public csscsrController(
            IResourceHelper resourceHelper,
            ILogger<csscsrController> logger)
        {
            this.resourceHelper = resourceHelper;
            log = logger;
        }
        
        private IResourceHelper resourceHelper;
        private ILogger log;


        private IActionResult GetResult(string resourceName, string contentType)
        {
            var assembly = typeof(csscsrController).GetTypeInfo().Assembly;
            resourceName = resourceHelper.ResolveResourceIdentifier(resourceName);
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                log.LogError("resource not found for " + resourceName);
                return NotFound();
            }

            log.LogDebug("resource found for " + resourceName);

            return new FileStreamResult(resourceStream, contentType);
        }


        // /csscsr/js/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult js()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.Mvc.js.";
            
            var requestPath = HttpContext.Request.Path.Value;
            log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/csscsr/js/".Length) return NotFound();

            var seg = requestPath.Substring("/csscsr/js/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = resourceHelper.GetMimeType(ext);

            return GetResult(baseSegment + seg, mimeType);
        }

        // /csscsr/css/
        [HttpGet]
        [AllowAnonymous]
        public IActionResult css()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.Mvc.css.";
            
            var requestPath = HttpContext.Request.Path.Value;
            log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/csscsr/css/".Length) return NotFound();

            var seg = requestPath.Substring("/csscsr/css/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = resourceHelper.GetMimeType(ext);

            return GetResult(
                baseSegment + seg,
                mimeType);
        }


        //TODO: caching, what are best practices for caching static resources ?
        // seems like for embedded we could set long cache in production since it cannot change
        //https://docs.asp.net/en/latest/performance/caching/response.html
        
    }
}
