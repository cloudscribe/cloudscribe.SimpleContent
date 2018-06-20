// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					
// Last Modified:			2018-02-06
// 
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPost : IContentItem
    {
        string Author { get; set; }
        string BlogId { get; set; }
        List<string> Categories { get; set; }
        List<IComment> Comments { get; set; }
        
        bool IsPublished { get; set; }
        DateTime LastModified { get; set; }
        
        /// <summary>
        /// the idea is to serialize an object with list of metadata and meta links
        /// whenever this model is modified it is reserialized here then
        /// we can generate the output html and store it on MetaHtml
        /// </summary>
        string MetaJson { get; set; }
        /// <summary>
        /// an automatically generated html meta data
        /// to be generated from the model seriaized in MetaJson
        /// </summary>
        string MetaHtml { get; set; }
        
        
        string CorrelationKey { get; set; }
        bool IsFeatured { get; set; }
        string ImageUrl { get; set; }
        string ThumbnailUrl { get; set; }

        /// <summary>
        /// If not null or whitespace, displays this teaser on blog index/listing views regardless of <see cref="TeaserMode"/> settings.
        /// </summary>
        string TeaserOverride { get; set; }

        /// <summary>
        /// If true, will display an entire blog post on index/listing views regardless of <see cref="TeaserMode"/> settings.
        /// </summary>
        bool SuppressTeaser { get; set; }

        // new fields 2018-06-20
        DateTime CreatedUtc { get; set; }

        string CreatedByUser { get; set; }
        string LastModifiedByUser { get; set; }
    }
}