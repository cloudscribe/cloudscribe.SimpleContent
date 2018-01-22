// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					John Jacobs
// Created:					2017-12-22
// Last Modified:			2017-12-22
// 

namespace cloudscribe.SimpleContent.Models
{
    /// <summary>
    /// Specifies whether SimpleContent should show teasers for blog posts on index/listing views.
    /// The default is OFF (show entire post).
    /// </summary>
    public enum AutoTeaserMode : byte
    {
        /// <summary>
        /// (Default) Auto-teaser mode OFF - show entire post.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Auto-teaser mode ON - truncate post for listings.
        /// </summary>
        On
    }
}