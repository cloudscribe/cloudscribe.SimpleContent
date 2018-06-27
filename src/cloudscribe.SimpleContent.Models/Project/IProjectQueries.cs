// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-07
// Last Modified:           2016-09-08
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    /// <summary>
    /// there are 2 "project" concepts that are kind of overlayed on top of each other
    /// there is the "Content Project" with settings encapsulated in ProjectSettings class
    /// this class is for querying those.
    /// there is also the concept in NoDb of a projectid which corresponds to the folder where obejcts are stored on disk
    /// in some cases the same projectid may be used for both NoDb and ProjectSettings.ProjectId
    /// 
    /// in SimpleContent the ProjectSettings.ProjectId always corresponds to the NoDb projectid aka folder where
    /// posts and pages will be stored
    /// </summary>
    public interface IProjectQueries
    {
        Task<IProjectSettings> GetProjectSettings(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
