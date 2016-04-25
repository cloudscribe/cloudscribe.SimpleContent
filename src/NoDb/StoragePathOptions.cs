// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-24
// 



using System.IO;

namespace NoDb
{
    public class StoragePathOptions
    {
        public string AppRootFolderPath { get; set; }
        public string BaseFolderVPath { get; set; } = "/nodb_storage";
        public string ProjectsFolderName { get; set; } = "projects";
        
        public string FolderSeparator
        {
            get { return Path.DirectorySeparatorChar.ToString(); }
        } //= "\\"; // \ on windows  / on *nix

    }
}
