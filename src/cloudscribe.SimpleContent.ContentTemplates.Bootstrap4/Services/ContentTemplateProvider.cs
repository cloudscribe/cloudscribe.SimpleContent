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

        }

        public Task<List<ContentTemplate>> GetAllTemplates()
        {
            if(_list == null)
            {
                BuildList();
            }

            return Task.FromResult(_list);
        }

        private ContentTemplate BuildCarouselWithContent()
        {
            var template = new ContentTemplate()
            {
                AvailbleForFeature = "*",
                Description = "",
                EditCss = new List<EditStyle>()
                {
                    new EditStyle
                    {
                        Url = "/cr/css/dropzone.min.css",
                        Environment = "any",
                        Sort = 1
                    },
                    new EditStyle
                    {
                        Url = "/cr/css/croppie.min.css",
                        Environment = "any",
                        Sort = 2
                    },
                    new EditStyle
                    {
                        Url = "/cr/css/croppie-cloudscribe.css",
                        Environment = "any",
                        Sort = 3
                    }
                },
                EditScripts = new List<EditScript>()
                {
                    new EditScript()
                    {
                        Url = "/cr/js/dropzone.min.js",
                        Environment = "any",
                        Sort = 1
                    },
                    new EditScript()
                    {
                        Url = "/cr/js/croppie.min.js",
                        Environment = "any",
                        Sort = 2
                    },
                    new EditScript()
                    {
                        Url = "/js/cloudscribe-unobtrusive-file-drop.js",
                        Environment = "any",
                        Sort = 3
                    },
                    new EditScript()
                    {
                        Url = "/js/knockout-3.4.2.js",
                        Environment = "any",
                        Sort = 4
                    },
                    new EditScript()
                    {
                        Url = "/js/cst-carousel.js",
                        Environment = "any",
                        Sort = 5
                    }
                },
                EditView = "ContentTemplates/CarouselWithContentEdit",
                Enabled = true,
                Key = "sct-CarouselWithContent",
                FormParserName = "DefaultModelFormParser",
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.CarouselWithContent, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
                ProjectId = "*",
                RenderCss = new List<EditStyle>()
                {

                },
                RenderScripts = new List<EditScript>()
                {

                },
                RenderView = "ContentTemplates/CarouselWithContentRender",
                ScreenshotUrl = "",
                SerializerName = "Json",
                Title = "A carousel of images with content above and below",
                ValidatorName = "DefaultTemplateModelValidator"

            };

            return template;

        }


        private ContentTemplate BuildTwoColumnWithImage()
        {
            var template = new ContentTemplate()
            {
                AvailbleForFeature = "*",
                Description = "A two column layout with an image above.",
                EditCss = new List<EditStyle>()
                {
                    new EditStyle
                    {
                        Url = "/cr/css/dropzone.min.css",
                        Environment = "any",
                        Sort = 1
                    },
                    new EditStyle
                    {
                        Url = "/cr/css/croppie.min.css",
                        Environment = "any",
                        Sort = 2
                    },
                    new EditStyle
                    {
                        Url = "/cr/css/croppie-cloudscribe.css",
                        Environment = "any",
                        Sort = 3
                    }
                },
                EditScripts = new List<EditScript>()
                {
                    new EditScript()
                    {
                        Url = "/cr/js/dropzone.min.js",
                        Environment = "any",
                        Sort = 1
                    },
                    new EditScript()
                    {
                        Url = "/cr/js/croppie.min.js",
                        Environment = "any",
                        Sort = 2
                    },
                    new EditScript()
                    {
                        Url = "/filemanager/js/cloudscribe-unobtrusive-file-drop.min.js",
                        Environment = "any",
                        Sort = 3
                    }
                },
                EditView = "ContentTemplates/TwoColumnWithImageEdit",
                Enabled = true,
                Key = "TwoColumnWithImage",
                FormParserName = "DefaultModelFormParser",
                ModelType = "cloudscribe.SimpleContent.ContentTemplates.ViewModels.TwoColumnWithImageViewModel, cloudscribe.SimpleContent.ContentTemplates.Bootstrap4",
                ProjectId = "*",
                RenderCss = new List<EditStyle>()
                {

                },
                RenderScripts = new List<EditScript>()
                {

                },
                RenderView = "ContentTemplates/TwoColumnWithImageRender",
                ScreenshotUrl = "",
                SerializerName = "Json",
                Title = "Two ColumnsWith Image Above",
                ValidatorName = "DefaultTemplateModelValidator"

            };

            return template;

        }
    }
}
