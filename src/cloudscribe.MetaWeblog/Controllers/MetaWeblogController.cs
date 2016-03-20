// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-07
// Last Modified:           2016-02-17
// 

using cloudscribe.MetaWeblog.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace cloudscribe.MetaWeblog.Controllers
{
    [Route("api/[controller]")]
    public class MetaWeblogController : Controller
    {
        public MetaWeblogController(
            IApplicationEnvironment appEnv,
            IMetaWeblogRequestParser metaWeblogRequestParser,
            IMetaWeblogRequestProcessor metaWeblogProcessor,
            IMetaWeblogResultFormatter metaWeblogResultFormatter,
            IMetaWeblogSecurity metaWeblogSecurity,
            IMetaWeblogRequestValidator metaWebLogRequestValidator,
            ILogger<MetaWeblogController> logger,
            IOptions<ApiOptions> optionsAccessor = null)
        {
            this.appEnv = appEnv;
            parser = metaWeblogRequestParser;
            processor = metaWeblogProcessor;
            formatter = metaWeblogResultFormatter;
            security = metaWeblogSecurity;
            validator = metaWebLogRequestValidator;
            log = logger;
            if(optionsAccessor != null)
            {
               options = optionsAccessor.Value;
            }
            else
            {
                options = new ApiOptions(); // just use the default options
            }
           
        }

        private IApplicationEnvironment appEnv;
        private ApiOptions options;
        private IMetaWeblogSecurity security;
        private IMetaWeblogRequestProcessor processor;
        private IMetaWeblogRequestParser parser;
        private IMetaWeblogResultFormatter formatter;
        private IMetaWeblogRequestValidator validator;
        private ILogger log;


        //http://stackoverflow.com/questions/7666835/xdocument-load-error


        //public void Post([FromBody]string value)
        //TODO: could we remove the value param and just get if from the request body
        //XDocument postedXml = XDocument.Parse(value);

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            CancellationToken cancellationToken = HttpContext?.RequestAborted ?? CancellationToken.None;

            var dumpFileBasePath = appEnv.ApplicationBasePath
                + options.AppRootDumpFolderVPath.Replace('/', Path.DirectorySeparatorChar);

            XDocument postedXml = null;
            XDocument resultXml;
            MetaWeblogResult outCome;
            FaultStruct faultStruct;
            try
            {
                using (HttpContext.Request.Body)
                {
                    postedXml = XDocument.Load(HttpContext.Request.Body);
                }
            }
            catch(Exception ex)
            {
                log.LogError("oops", ex);

                if (options.DumpRequestXmlToDisk)
                {
                    var requestFileName = dumpFileBasePath + "request-with-error" + Utils.GetDateTimeStringForFileName(true) + ".txt";
                    using (StreamWriter s = System.IO.File.CreateText(requestFileName))
                    {
                        //postedXml.Save(s, SaveOptions.None);
                        await HttpContext.Request.Body.CopyToAsync(s.BaseStream);
                    }
                }

                outCome = new MetaWeblogResult();
                faultStruct = new FaultStruct();
                faultStruct.faultCode = "802"; // invalid access
                faultStruct.faultString = "invalid access";
                outCome.Fault = faultStruct;
                resultXml = formatter.Format(outCome);
                return new XmlResult(resultXml);
            }
            

            var metaWeblogRequest = parser.ParseRequest(postedXml);

            if (options.DumpRequestXmlToDisk)
            {
                var requestFileName = dumpFileBasePath + "request-" 
                    + Utils.GetDateTimeStringForFileName(true)  + "-"
                    + metaWeblogRequest.MethodName.Replace(".","-")
                    + ".xml";
                using (StreamWriter s = System.IO.File.CreateText(requestFileName))
                {
                    postedXml.Save(s, SaveOptions.None);
                }
            }

            var permissions = await security.ValiatePermissions(metaWeblogRequest, cancellationToken);

            if(string.IsNullOrEmpty(metaWeblogRequest.BlogId))
            {
                metaWeblogRequest.BlogId = permissions.BlogId;
            }
            
            if(!permissions.IsAllowed)
            {
                outCome = new MetaWeblogResult();
                faultStruct = new FaultStruct();
                faultStruct.faultCode = "11"; // invalid access
                faultStruct.faultString = "Authentication Failed";
                outCome.Fault = faultStruct;
                resultXml = formatter.Format(outCome);
                return new XmlResult(resultXml);
            }

            var isValid = await validator.IsValid(metaWeblogRequest, cancellationToken);
            if (!isValid)
            {
                outCome = new MetaWeblogResult();
                faultStruct = new FaultStruct();
                faultStruct.faultCode = "802"; // invalid access
                faultStruct.faultString = "invalid request";
                outCome.Fault = faultStruct;
                resultXml = formatter.Format(outCome);
                return new XmlResult(resultXml);
            }

            outCome = await processor.ProcessRequest(
                metaWeblogRequest, 
                permissions, 
                cancellationToken);

            resultXml = formatter.Format(outCome);

            if (options.DumpResponseXmlToDisk)
            {
                var reseponseFileName = dumpFileBasePath + "response-" 
                    + Utils.GetDateTimeStringForFileName(true) + "-"
                    + outCome.Method.Replace(".", "-") 
                    + ".xml";

                using (StreamWriter s = System.IO.File.CreateText(reseponseFileName))
                {
                    resultXml.Save(s, SaveOptions.None);
                }
            }

            return new XmlResult(resultXml);
            
        }

        
    }
}
