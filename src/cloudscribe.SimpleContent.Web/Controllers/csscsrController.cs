// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2016-05-29
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    /// <summary>
    /// csscsr:cloudscribe SimpleContent static resource controller
    /// </summary>
    public class csscsrController : Controller
    {
        public csscsrController()
        {

        }

        private ContentResult GetContentResult(string resourceName, string contentType)
        {
            var assembly = typeof(csscsrController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
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
        public ContentResult editorjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.content-editor.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult editorjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.content-editor.min.js",
                "text/javascript");
            
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ContentResult commentjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.blog-comments.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult commentjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.blog-comments.min.js",
                "text/javascript");
 
        }


        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapwysiwygjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapwysiwygjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickerjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-datetimepicker.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickerjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.bootstrap-datetimepicker.min.js",
                "text/javascript");  
        }

        

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jquerycookie()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.cookie.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jquerycookiemin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.cookie.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jqueryhotkeys()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.hotkeys.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jqueryhotkeysmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.jquery.hotkeys.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult momentwithlocalesjs()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.moment-with-locales.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult momentwithlocalesjsmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.js.moment-with-locales.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult blogcommoncss()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.blog-common.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult blogcommoncssmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.blog-common.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult contentadmincss()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.content-admin.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult contentadmincssmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.content-admin.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickercss()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.bootstrap-datetimepicker.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickercssmin()
        {
            return GetContentResult(
                "cloudscribe.SimpleContent.Web.css.bootstrap-datetimepicker.min.css",
                "text/css");
        }


    }
}
