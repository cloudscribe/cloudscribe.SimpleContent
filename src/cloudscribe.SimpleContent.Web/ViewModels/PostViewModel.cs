// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-12
// Last Modified:           2016-02-23
// 


namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PostViewModel
    {
        
        public string Id { get; set; } = string.Empty;
   
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        
        public bool IsPublished { get; set; } = false;

        public string Categories { get; set; }

        public string PubDate { get; set; } = string.Empty;


    }
}
