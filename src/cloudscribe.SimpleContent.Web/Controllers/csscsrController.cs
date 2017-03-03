// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2016-08-15
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    /// <summary>
    /// csscsr:cloudscribe SimpleContent static resource controller
    /// </summary>
    public class csscsrController : Controller
    {
        public csscsrController(
            //IContentTypeProvider contentTypeProvider
            )
        {
            //this.contentTypeProvider = contentTypeProvider;
        }

        //private IContentTypeProvider contentTypeProvider;

        private IActionResult GetContentResult(string resourceName, string contentType)
        {
            var assembly = typeof(csscsrController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (contentType.StartsWith("image"))
            {
                return new FileStreamResult(resourceStream, contentType);
            }

            string payload;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                payload = reader.ReadToEnd();
            }
            
            return new ContentResult
            {
                ContentType = contentType,
                Content = payload,
                StatusCode = 200
            };
        }

        //TODO: caching, what are best practices for caching static resources ?
        // seems like for embedded we could set long cache in production since it cannot change
        //https://docs.asp.net/en/latest/performance/caching/response.html

        [HttpGet]
        [AllowAnonymous]
        public IActionResult editorjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.content-editor.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ckjs()
        {
            var baseSegment = "cloudscribe.SimpleContent.Web.js.ckeditor461.";
            // /ckjs/ckeditor.js
            var requestPath = HttpContext.Request.Path.Value;
            var seg = requestPath.Substring(6).Replace("/", ".").Replace("-","_");
            var ext = Path.GetExtension(requestPath);
            var mimeType = GetMimeType(ext);

            return GetContentResult(
                baseSegment + seg,
                mimeType);
        }

        private string GetMimeType(string extension)
        {
            switch(extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";

                case ".gif":
                    return "image/gif";

                case ".png":
                    return "image/png";

                case ".css":
                    return "text/css";

                case ".js":
                default:
                    return "text/javascript";

            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult editorjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.content-editor.min.js",
                "text/javascript");
            
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult commentjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.blog-comments.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult commentjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.blog-comments.min.js",
                "text/javascript");
 
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapwysiwygjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapwysiwygjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickerjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-datetimepicker.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickerjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-datetimepicker.min.js",
                "text/javascript");  
        }

        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jquerycookie()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.cookie.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jquerycookiemin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.cookie.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jqueryhotkeys()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.hotkeys.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jqueryhotkeysmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.hotkeys.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult jqueryajax()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.unobtrusive-ajax.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult momentwithlocalesjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.moment-with-locales.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult momentwithlocalesjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.moment-with-locales.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult blogcommoncss()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.blog-common.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult blogcommoncssmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.blog-common.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult contentadmincss()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.content-admin.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult contentadmincssmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.content-admin.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickercss()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.bootstrap-datetimepicker.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult bootstrapdatetimepickercssmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.bootstrap-datetimepicker.min.css",
                "text/css");
        }


    }
}
