// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2018-04-05
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
    public class CsscsrController : Controller
    {
        public CsscsrController(
            IResourceHelper resourceHelper,
            ILogger<CsscsrController> logger)
        {
            ResourceHelper = resourceHelper;
            Log = logger;
        }
        
        protected IResourceHelper ResourceHelper { get; private set; }
        protected ILogger Log { get; private set; }


        protected virtual IActionResult GetResult(string resourceName, string contentType)
        {
            var assembly = typeof(CsscsrController).GetTypeInfo().Assembly;
            resourceName = ResourceHelper.ResolveResourceIdentifier(resourceName);
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                Log.LogError("resource not found for " + resourceName);
                return NotFound();
            }

            Log.LogDebug("resource found for " + resourceName);

            return new FileStreamResult(resourceStream, contentType);
        }


        // /csscsr/js/
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult Js()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.js.";
            
            var requestPath = HttpContext.Request.Path.Value;
            Log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/csscsr/js/".Length) return NotFound();

            var seg = requestPath.Substring("/csscsr/js/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = ResourceHelper.GetMimeType(ext);

            return GetResult(baseSegment + seg, mimeType);
        }

        // /csscsr/css/
        [HttpGet]
        [AllowAnonymous]
        public virtual IActionResult Css()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.css.";
            
            var requestPath = HttpContext.Request.Path.Value;
            Log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/csscsr/css/".Length) return NotFound();

            var seg = requestPath.Substring("/csscsr/css/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = ResourceHelper.GetMimeType(ext);

            return GetResult(
                baseSegment + seg,
                mimeType);
        }


        //TODO: caching, what are best practices for caching static resources ?
        // seems like for embedded we could set long cache in production since it cannot change
        //https://docs.asp.net/en/latest/performance/caching/response.html
        
    }
}
