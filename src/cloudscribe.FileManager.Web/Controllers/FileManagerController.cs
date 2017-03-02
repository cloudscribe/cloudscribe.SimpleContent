// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-14
// Last Modified:           2017-02-24
// 

using cloudscribe.FileManager.Web.Filters;
using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Controllers
{
    public class FileManagerController : Controller
    {
        public FileManagerController(
            FileManagerService fileManagerService,
            IAuthorizationService authorizationService,
            IOptions<AutomaticUploadOptions> autoUploadOptionsAccessor,
            IAntiforgery antiforgery,
            ILogger<FileManagerController> logger
            )
        {
            this.fileManagerService = fileManagerService;
            this.authorizationService = authorizationService;
            autoUploadOptions = autoUploadOptionsAccessor.Value;
            this.antiforgery = antiforgery;
            log = logger;
        }

        private FileManagerService fileManagerService;
        private IAuthorizationService authorizationService;
        private AutomaticUploadOptions autoUploadOptions;
        private readonly IAntiforgery antiforgery;
        // Get the default form options so that we can use them to set the default limits for
        // request body data
        private static readonly FormOptions defaultFormOptions = new FormOptions();
        private ILogger log;

        [HttpGet]
        //[GenerateAntiforgeryTokenCookieForAjax]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> CkFileDialog(CkBrowseModel model)
        {
            model.InitialVirtualPath = await fileManagerService.GetRootVirtualPath().ConfigureAwait(false);
            model.FileTreeServiceUrl = Url.Action("GetFileTreeJson","FileManager");
            model.UploadServiceUrl = Url.Action("Upload", "FileManager");
            model.CreateFolderServiceUrl = Url.Action("CreateFolder", "FileManager");
            model.DeleteFolderServiceUrl = Url.Action("DeleteFolder", "FileManager");
            model.RenameFolderServiceUrl = Url.Action("RenameFolder", "FileManager");
            model.DeleteFileServiceUrl = Url.Action("DeleteFile", "FileManager");
            model.RenameFileServiceUrl = Url.Action("RenameFile", "FileManager");
            model.CanDelete = await authorizationService.AuthorizeAsync(User, "FileManagerDeletePolicy");

            model.AllowedFileExtensionsRegex = @"/(\.|\/)(gif|GIF|jpg|JPG|jpeg|JPEG|png|PNG|flv|FLV|swf|SWF|wmv|WMV|mp3|MP3|mp4|MP4|m4a|M4A|m4v|M4V|oga|OGA|ogv|OGV|webma|WEBMA|webmv|WEBMV|webm|WEBM|wav|WAV|fla|FLA|tif|TIF|asf|ASF|asx|ASX|avi|AVI|mov|MOV|mpeg|MPEG|mpg|MPG|zip|ZIP|pdf|PDF|doc|DOC|docx|DOCX|xls|XLS|xlsx|XLSX|ppt|PPT|pptx|PPTX|pps|PPS|csv|CSV|txt|TXT|htm|HTM|html|HTML|css|CSS)$/i";


            return View(model);
        }


        //https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads


        /// <summary>
        /// this method is called from the simplecontentdropfile plugin for ckeditor when
        /// a file is dropped into the editor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "FileManagerPolicy")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Upload(
            //List<IFormFile> files
            bool? resizeImages,
            int? maxWidth,
            int? maxHeight,
            string currentDir = "",
            string croppedFileName = ""
            )
        {
            var theFiles = HttpContext.Request.Form.Files;
            var imageList = new List<UploadResult>();
            if(resizeImages.HasValue)
            {
                if(resizeImages.Value == false)
                {
                    if(Path.HasExtension(currentDir)) //this will be true for cropped
                    {
                        currentDir = currentDir.Substring(0, currentDir.LastIndexOf("/"));
                    }
                }
            }
            string newFileName = string.Empty; ;
            if(theFiles.Count == 1 && !string.IsNullOrEmpty(croppedFileName))
            {
                newFileName = croppedFileName;
            }
            
            foreach (var formFile in theFiles)
            {
                try
                {
                    if (formFile.Length > 0)
                    {
                        var uploadResult = await fileManagerService.ProcessFile(
                            formFile,
                            autoUploadOptions,
                            resizeImages,
                            maxWidth,
                            maxHeight,
                            currentDir,
                            newFileName
                            ).ConfigureAwait(false);
                        
                        imageList.Add(uploadResult);

                    }
                }
                catch (Exception ex)
                {
                    log.LogError(MediaLoggingEvents.FILE_PROCCESSING, ex, ex.StackTrace);
                }

            }

            return Json(imageList);
        }

        //[HttpPost]
        //[Authorize(Policy = "FileManagerPolicy")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UploadBase64(
        //    //List<IFormFile> files
        //    bool? resizeImages,
        //    int? maxWidth,
        //    int? maxHeight,
        //    string currentDir = "",
        //    string croppedFileName = ""
        //    )
        //{
        //    var theFiles = HttpContext.Request.Form.Files;
        //    var imageList = new List<UploadResult>();
        //    if (resizeImages.HasValue)
        //    {
        //        if (resizeImages.Value == false)
        //        {
        //            if (Path.HasExtension(currentDir)) //this will be true for cropped
        //            {
        //                currentDir = currentDir.Substring(currentDir.LastIndexOf("/"));
        //            }
        //        }
        //    }

        //    foreach (var formFile in theFiles)
        //    {
        //        try
        //        {
        //            if (formFile.Length > 0)
        //            {
        //                var uploadResult = await fileManagerService.ProcessFile(
        //                    formFile,
        //                    autoUploadOptions,
        //                    resizeImages,
        //                    maxWidth,
        //                    maxHeight,
        //                    currentDir
        //                    ).ConfigureAwait(false);

        //                imageList.Add(uploadResult);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.LogError(MediaLoggingEvents.FILE_PROCCESSING, ex, ex.StackTrace);
        //        }

        //    }

        //    return Json(imageList);
        //}

        [HttpPost]
        [Authorize(Policy = "FileManagerPolicy")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> CreateFolder(string currentVirtualPath, string newFolderName)
        {
            var result = await fileManagerService.CreateFolder(currentVirtualPath, newFolderName).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFolder(string folderToDelete)
        {
            var result = await fileManagerService.DeleteFolder(folderToDelete).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenameFolder(string folderToRename, string newNameSegment)
        {
            var result = await fileManagerService.RenameFolder(folderToRename, newNameSegment).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile(string fileToDelete)
        {
            var result = await fileManagerService.DeleteFile(fileToDelete).ConfigureAwait(false);
            return Json(result);

        }

        [HttpPost]
        [Authorize(Policy = "FileManagerDeletePolicy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenameFile(string fileToRename, string newNameSegment)
        {
            var result = await fileManagerService.RenameFile(fileToRename, newNameSegment).ConfigureAwait(false);
            return Json(result);

        }

        // 1. Disable the form value model binding here to take control of handling 
        //    potentially large files.
        // 2. Typically antiforgery tokens are sent in request body, but since we 
        //    do not want to read the request body early, the tokens are made to be 
        //    sent via headers. The antiforgery token filter first looks for tokens
        //    in the request header and then falls back to reading the body.
        [HttpPost]
        [DisableFormValueModelBinding]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StreamedUpload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        targetFilePath = Path.GetTempFileName();
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);

                            log.LogInformation($"Copied the uploaded file '{targetFilePath}'");
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key, value);

                            if (formAccumulator.ValueCount > defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            //var user = new User();
            //var formValueProvider = new FormValueProvider(
            //    BindingSource.Form,
            //    new FormCollection(formAccumulator.GetResults()),
            //    CultureInfo.CurrentCulture);

            //var bindingSuccessful = await TryUpdateModelAsync(user, prefix: "",
            //    valueProvider: formValueProvider);
            //if (!bindingSuccessful)
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }
            //}

            //var uploadedData = new UploadedData()
            //{
            //    Name = user.Name,
            //    Age = user.Age,
            //    Zipcode = user.Zipcode,
            //    FilePath = targetFilePath
            //};
            //return Json(uploadedData);
            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public async Task<IActionResult> GetFileTreeJson(string virtualStartPath = "")
        {
            var list = await fileManagerService.GetFileTree(virtualStartPath).ConfigureAwait(false);

            return Json(list);
        }

        [HttpGet]
        [Authorize(Policy = "FileManagerPolicy")]
        public IActionResult Get()
        {
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);

            return new ObjectResult(new
            {
                token = tokens.RequestToken,
                tokenName = tokens.HeaderName
            });
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}
