// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-08-31
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cloudscribe.SimpleContent.Storage.EF
{
    public interface ISimpleContentModelMapper
    {
        void Map(EntityTypeBuilder<ProjectSettings> entity);

        void Map(EntityTypeBuilder<Post> entity);

        void Map(EntityTypeBuilder<Page> entity);

        void Map(EntityTypeBuilder<Comment> entity);

    }
}
