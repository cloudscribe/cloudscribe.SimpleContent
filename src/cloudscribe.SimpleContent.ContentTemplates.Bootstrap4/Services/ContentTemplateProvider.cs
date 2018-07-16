// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-07-10
// Last Modified:           2018-07-10
// 

using cloudscribe.SimpleContent.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Services
{
    public class ContentTemplateProvider : IContentTemplateProvider
    {
        public ContentTemplateProvider()
        {

        }

        private List<ContentTemplate> _list = null;

        

        private void BuildList()
        {
            if(_list != null) { return; }

            _list = new List<ContentTemplate>();
            _list.Add(BuildTwoColumnWithImage());
            _list.Add(BuildCarouselWithContent());
            _list.Add(BuildGalleryWithContent());

        }

        public Task<List<ContentTemplate>> GetAllTemplates()
        {
            if(_list == null)
            {
                BuildList();
            }

            return Task.FromResult(_list);
        }

        private ContentTemplate BuildGalleryWithContent()
        {
            var template = new ContentTemplate()
            {
                Key = "sct-GalleryWithContent",
                Title = "Image gallery with content above and below",
                Description = "",
                EditView = "ContentTemplates/GalleryWithContentEdit",
                RenderView = "ContentTemplates/GalleryWithContentRender",
                ScreenshotUrl = "",
                ProjectId = "*",
                AvailbleForFeature = "*",
                Enabled = true,
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.SimpleGalleryViewModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
                FormParserName = "DefaultModelFormParser",
                SerializerName = "Json",
                ValidatorName = "DefaultTemplateModelValidator",

                EditCss = new List<CssFile>()
                {
                    new CssFile
                    {
                        Url = "/cr/css/dropzone.min.css",
                        Environment = "any",
                        Sort = 1
                    },
                    new CssFile
                    {
                        Url = "/cr/css/croppie.min.css",
                        Environment = "any",
                        Sort = 2
                    },
                    new CssFile
                    {
                        Url = "/cr/css/croppie-cloudscribe.css",
                        Environment = "any",
                        Sort = 3
                    }
                },
                EditScripts = new List<ScriptFile>()
                {
                    new ScriptFile()
                    {
                        Url = "/cr/js/dropzone.min.js",
                        Environment = "any",
                        Sort = 1
                    },
                    new ScriptFile()
                    {
                        Url = "/cr/js/croppie.min.js",
                        Environment = "any",
                        Sort = 2
                    },
                    new ScriptFile()
                    {
                        Url = "/js/cloudscribe-unobtrusive-file-drop.js",
                        Environment = "any",
                        Sort = 3
                    },
                    new ScriptFile()
                    {
                        Url = "/js/knockout-3.4.2.js",
                        Environment = "any",
                        Sort = 4
                    },
                    new ScriptFile()
                    {
                        Url = "/js/cst-basic-list.js",
                        Environment = "any",
                        Sort = 5
                    }
                },
                
                RenderCss = new List<CssFile>()
                {
                    new CssFile
                    {
                        Url = "/cr/css/baguetteBox.min.css",
                        Environment = "any",
                        Sort = 3
                    },
                    new CssFile
                    {
                        Url = "/sctr/css/simple-gallery.min.css",
                        Environment = "any",
                        Sort = 4
                    }
                },
                RenderScripts = new List<ScriptFile>()
                {
                    new ScriptFile()
                    {
                        Url = "/cr/js/baguetteBox.min.js",
                        Environment = "any",
                        Sort = 2
                    },
                    new ScriptFile()
                    {
                        Url = "/sctr/js/simple-gallery.js",
                        Environment = "any",
                        Sort = 3
                    }
                }
                
            };

            return template;

        }

        private ContentTemplate BuildCarouselWithContent()
        {
            var template = new ContentTemplate()
            {
                Key = "sct-CarouselWithContent",
                Title = "A carousel of images with content above and below",
                Description = "",
                EditView = "ContentTemplates/CarouselWithContentEdit",
                RenderView = "ContentTemplates/CarouselWithContentRender",
                ScreenshotUrl = "",
                ProjectId = "*",
                AvailbleForFeature = "*",
                Enabled = true,
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.ListWithContentModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
                FormParserName = "DefaultModelFormParser",
                SerializerName = "Json",
                ValidatorName = "DefaultTemplateModelValidator",

                EditCss = new List<CssFile>()
                {
                    new CssFile
                    {
                        Url = "/cr/css/dropzone.min.css",
                        Environment = "any",
                        Sort = 1
                    },
                    new CssFile
                    {
                        Url = "/cr/css/croppie.min.css",
                        Environment = "any",
                        Sort = 2
                    },
                    new CssFile
                    {
                        Url = "/cr/css/croppie-cloudscribe.css",
                        Environment = "any",
                        Sort = 3
                    }
                },
                EditScripts = new List<ScriptFile>()
                {
                    new ScriptFile()
                    {
                        Url = "/cr/js/dropzone.min.js",
                        Environment = "any",
                        Sort = 1
                    },
                    new ScriptFile()
                    {
                        Url = "/cr/js/croppie.min.js",
                        Environment = "any",
                        Sort = 2
                    },
                    new ScriptFile()
                    {
                        Url = "/js/cloudscribe-unobtrusive-file-drop.js",
                        Environment = "any",
                        Sort = 3
                    },
                    new ScriptFile()
                    {
                        Url = "/js/knockout-3.4.2.js",
                        Environment = "any",
                        Sort = 4
                    },
                    new ScriptFile()
                    {
                        Url = "/js/cst-basic-list.js",
                        Environment = "any",
                        Sort = 5
                    }
                },
                
                RenderCss = new List<CssFile>()
                {

                },
                RenderScripts = new List<ScriptFile>()
                {

                }
                
            };

            return template;

        }


        private ContentTemplate BuildTwoColumnWithImage()
        {
            var template = new ContentTemplate()
            {
                Key = "TwoColumnWithImage",
                Title = "Two ColumnsWith Image Above",
                Description = "A two column layout with an image above.",
                EditView = "ContentTemplates/TwoColumnWithImageEdit",
                RenderView = "ContentTemplates/TwoColumnWithImageRender",
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.TwoColumnWithImageViewModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
                ScreenshotUrl = "",
                ProjectId = "*",
                AvailbleForFeature = "*",
                Enabled = true,
                FormParserName = "DefaultModelFormParser",
                SerializerName = "Json",
                ValidatorName = "DefaultTemplateModelValidator",

                EditCss = new List<CssFile>()
                {
                    new CssFile
                    {
                        Url = "/cr/css/dropzone.min.css",
                        Environment = "any",
                        Sort = 1
                    },
                    new CssFile
                    {
                        Url = "/cr/css/croppie.min.css",
                        Environment = "any",
                        Sort = 2
                    },
                    new CssFile
                    {
                        Url = "/cr/css/croppie-cloudscribe.css",
                        Environment = "any",
                        Sort = 3
                    }
                },
                EditScripts = new List<ScriptFile>()
                {
                    new ScriptFile()
                    {
                        Url = "/cr/js/dropzone.min.js",
                        Environment = "any",
                        Sort = 1
                    },
                    new ScriptFile()
                    {
                        Url = "/cr/js/croppie.min.js",
                        Environment = "any",
                        Sort = 2
                    },
                    new ScriptFile()
                    {
                        Url = "/filemanager/js/cloudscribe-unobtrusive-file-drop.min.js",
                        Environment = "any",
                        Sort = 3
                    }
                },
                
                RenderCss = new List<CssFile>()
                {

                },
                RenderScripts = new List<ScriptFile>()
                {

                }
                
            };

            return template;

        }
    }
}
