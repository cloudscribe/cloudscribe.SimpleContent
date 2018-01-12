// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					John Jacobs
// Created:					2017-12-22
// Last Modified:			2017-12-22
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Models
{
    /// <summary>
    /// Specifies how SimpleContent will truncate blog posts to create teasers for index/listing views.
    /// </summary>
    public enum TeaserTruncationMode : byte
    {
        /// <summary>
        /// (Default) Truncate the post based on number of words.
        /// </summary>
        Word = 0,
        /// <summary>
        /// Truncate the post to a fixed length.
        /// </summary>
        Length,
        /// <summary>
        /// Truncate the post based on number of characters.
        /// </summary>
        Character
    }
}