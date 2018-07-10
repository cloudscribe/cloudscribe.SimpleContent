using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

            var twoColumnWithImage = new ContentTemplate()
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
            _list.Add(twoColumnWithImage);

        }

        public Task<List<ContentTemplate>> GetAllTemplates()
        {
            if(_list == null)
            {
                BuildList();
            }

            return Task.FromResult(_list);
        }
    }
}
