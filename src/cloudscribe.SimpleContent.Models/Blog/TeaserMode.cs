// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Authors:					John Jacobs/Joe Audette
// Created:					2017-12-22
// Last Modified:			2018-02-06
// 


namespace cloudscribe.SimpleContent.Models
{
    /// <summary>
    /// Specifies whether SimpleContent should show teasers for blog posts on index/listing views.
    /// The default is OFF (show entire post).
    /// </summary>
    public enum TeaserMode : byte
    {
        /// <summary>
        /// (Default) No teaser - show entire post.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Use teasers for post lists and feed.
        /// </summary>
        ListsAndFeed,
        /// <summary>
        /// Use teasers only for the blog feed
        /// </summary>
        FeedOnly,
        /// <summary>
        /// Use teasers only for the post lists
        /// </summary>
        ListOnly
    }
}