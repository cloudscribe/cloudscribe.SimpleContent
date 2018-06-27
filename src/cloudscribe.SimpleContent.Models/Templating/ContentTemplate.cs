// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-20
// Last Modified:           2018-06-27
// 

using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class ContentTemplate
    {
        public ContentTemplate()
        {
            EditScripts = new List<EditScript>();
            EditCss = new List<EditStyle>();
            RenderScripts = new List<EditScript>();
            RenderCss = new List<EditStyle>();

        }

        public string ProjectId { get; set; } = "*";
        public string AvailbleForFeature { get; set; } = "*"; // * = any, Blog or Page
        public string Key { get; set; }
        public bool Enabled { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ModelType { get; set; }
        public string EditView { get; set; }
        public string RenderView { get; set; }
        public string ScreenshotUrl { get; set; }

        public List<EditScript> EditScripts { get; set; }
        public List<EditStyle> EditCss { get; set; }

        public List<EditScript> RenderScripts { get; set; }
        public List<EditStyle> RenderCss { get; set; }

        public string SerializerName { get; set; } = "Json";

        public string FormParserName { get; set; } = "DefaultModelFormParser";
        public string ValidatorName { get; set; } = "DefaultTemplateModelValidator";
    }
}
