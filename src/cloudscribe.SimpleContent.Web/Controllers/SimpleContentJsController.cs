// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2016-05-27
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    public class SimpleContentJsController : Controller
    {
        public SimpleContentJsController()
        {

        }
        //TODO: caching - can we have a cache dependency on file changes?
        [HttpGet]
        [AllowAnonymous]
        public async Task editor()
        {
            var assembly = typeof(SimpleContentJsController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("cloudscribe.SimpleContent.Web.js.content-editor.js");
            string jsResult;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                jsResult = reader.ReadToEnd();
            }

            HttpContext.Response.ContentType = "text/javascript";
            await HttpContext.Response.WriteAsync(jsResult);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task bootstrapwysiwyg()
        {
            var assembly = typeof(SimpleContentJsController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("cloudscribe.SimpleContent.Web.js.bootstrap-wysiwyg.js");
            string jsResult;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                jsResult = reader.ReadToEnd();
            }

            HttpContext.Response.ContentType = "text/javascript";
            await HttpContext.Response.WriteAsync(jsResult);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task comment()
        {
            var assembly = typeof(SimpleContentJsController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream("cloudscribe.SimpleContent.Web.js.blog-comments.js");
            string jsResult;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                jsResult = reader.ReadToEnd();
            }

            HttpContext.Response.ContentType = "text/javascript";
            await HttpContext.Response.WriteAsync(jsResult);

        }
    }
}
