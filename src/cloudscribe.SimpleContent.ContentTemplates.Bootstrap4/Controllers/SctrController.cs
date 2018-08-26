// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2018-07-16
// Last Modified:           2018-07-16
// 

using cloudscribe.Web.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace cloudscribe.SimpleContent.ContentTemplates.Bootstrap4.Controllers
{
    /// <summary>
    /// SimpleContentTemplateResourcesController - sctr
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SctrController : Controller
    {
        public SctrController(
            IResourceHelper resourceHelper,
            ILogger<SctrController> logger
            )
        {
            ResourceHelper = resourceHelper;
            Log = logger;
        }

        protected IResourceHelper ResourceHelper { get; private set; }
        protected ILogger Log { get; private set; }

        protected virtual IActionResult GetResult(string resourceName, string contentType)
        {
            var assembly = typeof(SctrController).GetTypeInfo().Assembly;
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

        // /sctr/js/
        [HttpGet]
        [Route("sctr/js/{slug}")]
        [AllowAnonymous]
        public virtual IActionResult Js()
        {
            var baseSegment = "cloudscribe.SimpleContent.ContentTemplates.Bootstrap4.js.";

            var requestPath = HttpContext.Request.Path.Value;
            Log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/sctr/js/".Length) return NotFound();

            var seg = requestPath.Substring("/sctr/js/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = ResourceHelper.GetMimeType(ext);

            return GetResult(baseSegment + seg, mimeType);
        }

        // /sctr/css/
        [HttpGet]
        [Route("sctr/css/{slug}")]
        [AllowAnonymous]
        public virtual IActionResult Css()
        {
            var baseSegment = "cloudscribe.SimpleContent.ContentTemplates.Bootstrap4.css.";

            var requestPath = HttpContext.Request.Path.Value;
            Log.LogDebug(requestPath + " requested");

            if (requestPath.Length < "/sctr/css/".Length) return NotFound();

            var seg = requestPath.Substring("/sctr/css/".Length);
            var ext = Path.GetExtension(requestPath);
            var mimeType = ResourceHelper.GetMimeType(ext);

            return GetResult(
                baseSegment + seg,
                mimeType);
        }

    }
}
