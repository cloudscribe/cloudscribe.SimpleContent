// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2017-04-24
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    /// <summary>
    /// csscsr:cloudscribe SimpleContent static resource controller
    /// 
    /// </summary>
    public class csscsrController : Controller
    {
        public csscsrController(ILogger<csscsrController> logger)
        {
            log = logger;
        }

        private ILogger log;

    
        private IActionResult GetResult(string resourceName, string contentType)
        {
            var assembly = typeof(csscsrController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                resourceStream
                = assembly.GetManifestResourceStream(resourceName.Replace("_", "-"));
                if (resourceStream == null)
                {
                    log.LogError("resource not found for " + resourceName);
                    return NotFound();
                }
                log.LogDebug("resource found for " + resourceName);
            }
            else
            {

                log.LogDebug("resource found for " + resourceName);

            }

            return new FileStreamResult(resourceStream, contentType);



        }

        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult js()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.js.";
            // /csscsr/js/
            var requestPath = HttpContext.Request.Path.Value;
            log.LogDebug(requestPath + " requested");

            if (requestPath.Length < 11) return NotFound();

            var seg = requestPath.Substring(11).Replace("/", ".").Replace("-", "_");
            var ext = Path.GetExtension(requestPath);
            var mimeType = GetMimeType(ext);

            return GetResult(
                baseSegment + seg,
                mimeType);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult css()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.css.";
            // /csscsr/css/
            var requestPath = HttpContext.Request.Path.Value;
            log.LogDebug(requestPath + " requested");

            if (requestPath.Length < 12) return NotFound();

            var seg = requestPath.Substring(12).Replace("/", ".").Replace("-", "_");
            var ext = Path.GetExtension(requestPath);
            var mimeType = GetMimeType(ext);

            return GetResult(
                baseSegment + seg,
                mimeType);
        }

        

        //TODO: caching, what are best practices for caching static resources ?
        // seems like for embedded we could set long cache in production since it cannot change
        //https://docs.asp.net/en/latest/performance/caching/response.html

        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ckjs()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.js.ckeditor461.";
            // /ckjs/ckeditor.js
            var requestPath = HttpContext.Request.Path.Value;
            log.LogDebug(requestPath + " requested");
            var seg = requestPath.Substring(6).Replace("/", ".").Replace("-","_");
            var ext = Path.GetExtension(requestPath);
            var mimeType = GetMimeType(ext);

            return GetResult(
                baseSegment + seg,
                mimeType);
        }

        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult editorjs()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.editor-ck.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult editorjsmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.editor-ck.min.js",
                "text/javascript");
            
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult commentjs()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.blog-comments.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult commentjsmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.blog-comments.min.js",
                "text/javascript");
 
        }


        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult bootstrapwysiwygjs()
        //{
        //    return GetResult(
        //        "cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.js",
        //        "text/javascript");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult bootstrapwysiwygjsmin()
        //{
        //    return GetResult(
        //        "cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.min.js",
        //        "text/javascript");
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickerjs()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-datetimepicker.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickerjsmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-datetimepicker.min.js",
                "text/javascript");  
        }

        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jquerycookie()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.jquery.cookie.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jquerycookiemin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.jquery.cookie.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jqueryhotkeys()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.jquery.hotkeys.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jqueryhotkeysmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.jquery.hotkeys.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jqueryajax()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.jquery.unobtrusive-ajax.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult momentwithlocalesjs()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.moment-with-locales.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult momentwithlocalesjsmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.js.moment-with-locales.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult blogcommoncss()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.css.blog-common.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult blogcommoncssmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.css.blog-common.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult contentadmincss()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.css.content-admin.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult contentadmincssmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.css.content-admin.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickercss()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.css.bootstrap-datetimepicker.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickercssmin()
        {
            return GetResult(
                "cloudscribe.SimpleContent.Web.css.bootstrap-datetimepicker.min.css",
                "text/css");
        }

        private string GetMimeType(string extension)
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";

                case ".gif":
                    return "image/gif";

                case ".png":
                    return "image/png";

                case ".otf":
                    return "font/otf";

                case ".eot":
                    return "application/vnd.ms-fontobject";

                case ".svg":
                    return "image/svg+xml";

                case ".ttf":
                    return "application/octet-stream";

                case ".woff":
                case ".woff2":
                    return "application/font-woff";

                case ".css":
                    return "text/css";

                case ".js":
                default:
                    return "text/javascript";

            }
        }

    }
}
