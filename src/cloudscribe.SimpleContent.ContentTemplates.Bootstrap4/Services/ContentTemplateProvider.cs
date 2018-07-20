// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-07-10
// Last Modified:           2018-07-18
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Services
{
    public class ContentTemplateProvider : IContentTemplateProvider
    {
        public ContentTemplateProvider(
            IStringLocalizer<ContentTemplateResources> stringLocalizer
            )
        {
            _sr = stringLocalizer;
        }

        private List<ContentTemplate> _list = null;
        private IStringLocalizer _sr;



        private void BuildList()
        {
            if(_list != null) { return; }

            _list = new List<ContentTemplate>();
            _list.Add(BuildSectionsWithImages());
            _list.Add(BuildImageWithContent());
            _list.Add(BuildGalleryWithContent());
            _list.Add(BuildListOfLinks());
            _list.Add(BuildBingLocationMap());

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
                Title = _sr["Simple image gallery with optional content above and below"],
                Description = _sr["The gallery has 6 layouts to choose from: cards, compact, grid, carousel full width, carousel left with wrapped content, and carousel right with wrapped content."],
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
                        Url = "/filemanager/js/cloudscribe-unobtrusive-file-drop.min.js",
                        Environment = "any",
                        Sort = 3
                    },
                    new ScriptFile()
                    {
                        Url = "/cr/js/knockout-3.4.2.js",
                        Environment = "any",
                        Sort = 4
                    },
                    new ScriptFile()
                    {
                        Url = "/sctr/js/cst-basic-list.min.js",
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

        private ContentTemplate BuildListOfLinks()
        {
            var template = new ContentTemplate()
            {
                Key = "sct-ListOfLinks",
                Title = _sr["A list of links with optional thumbnail per link and content above and below."],
                Description = _sr["This is a simple non-paginated list, suitable for modestly sized lists that can fit on a single page."],
                EditView = "ContentTemplates/ListOfLinksEdit",
                RenderView = "ContentTemplates/ListOfLinksRender",
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
                        Url = "/filemanager/js/cloudscribe-unobtrusive-file-drop.min.js",
                        Environment = "any",
                        Sort = 3
                    },
                    new ScriptFile()
                    {
                        Url = "/cr/js/knockout-3.4.2.js",
                        Environment = "any",
                        Sort = 4
                    },
                    new ScriptFile()
                    {
                        Url = "/sctr/js/cst-link-list.min.js",
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


        private ContentTemplate BuildSectionsWithImages()
        {
            var template = new ContentTemplate()
            {
                Key = "sct-ColumnsWithImages",
                Title = _sr["One to four sections with an optional image per section"],
                Description = _sr["There are 2 layouts to choose from, responsive columns with images on top, or images floated to alternate sides per section to wrap content around the images."],
                EditView = "ContentTemplates/SectionsWithImageEdit",
                RenderView = "ContentTemplates/SectionsWithImageRender",
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.SectionsWithImageViewModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
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
                    },
                    new ScriptFile()
                    {
                        Url = "/cr/js/unsaved-changes-prompt.min.js",
                        Environment = "any",
                        Sort = 4
                    }

                },
                
                RenderCss = new List<CssFile>()
                {
                    new CssFile
                    {
                        Url = "/sctr/css/simple-image.min.css",
                        Environment = "any",
                        Sort = 2
                    }
                },
                RenderScripts = new List<ScriptFile>()
                {

                }
                
            };

            return template;

        }

        private ContentTemplate BuildImageWithContent()
        {
            var template = new ContentTemplate()
            {
                Key = "sct-ImageWithContent",
                Title = _sr["Image with wrapped content"],
                Description = _sr["A simple image floated left or right so that content wraps around it."],
                EditView = "ContentTemplates/ImageWithContentEdit",
                RenderView = "ContentTemplates/ImageWithContentRender",
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.ImageWithContentViewModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
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
                    new CssFile
                    {
                        Url = "/sctr/css/simple-image.min.css",
                        Environment = "any",
                        Sort = 2
                    }
                },
                RenderScripts = new List<ScriptFile>()
                {

                }

            };

            return template;

        }


        private ContentTemplate BuildBingLocationMap()
        {
            var template = new ContentTemplate()
            {
                Key = "sct-bing-location-map",
                Title = _sr["Bing address location map with content above and below"],
                Description = _sr["The map will be centered on the address you provide and users can get directions to the location. Optional content can be provided above and below the map. Requires a Bing Maps API key."],
                EditView = "ContentTemplates/BingMapEdit",
                RenderView = "ContentTemplates/BingMapRender",
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.BingMapViewModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
                ScreenshotUrl = "",
                ProjectId = "*",
                AvailbleForFeature = "*",
                Enabled = true,
                FormParserName = "DefaultModelFormParser",
                SerializerName = "Json",
                ValidatorName = "DefaultTemplateModelValidator",

                EditCss = new List<CssFile>()
                {
                    //new CssFile
                    //{
                    //    Url = "/cr/css/dropzone.min.css",
                    //    Environment = "any",
                    //    Sort = 1
                    //},
                    //new CssFile
                    //{
                    //    Url = "/cr/css/croppie.min.css",
                    //    Environment = "any",
                    //    Sort = 2
                    //},
                    //new CssFile
                    //{
                    //    Url = "/cr/css/croppie-cloudscribe.css",
                    //    Environment = "any",
                    //    Sort = 3
                    //}
                },
                EditScripts = new List<ScriptFile>()
                {
                    //new ScriptFile()
                    //{
                    //    Url = "/js/simple-googlemap-edit.js",
                    //    Environment = "any",
                    //    Sort = 1
                    //}
                },

                RenderCss = new List<CssFile>()
                {

                },
                RenderScripts = new List<ScriptFile>()
                {
                    new ScriptFile()
                    {
                        Url = "/cr/js/cloudscribe-unobtrusive-bing-location-map.min.js",
                        Environment = "any",
                        Sort = 1
                    }
                }

            };

            return template;

        }
    }
}
