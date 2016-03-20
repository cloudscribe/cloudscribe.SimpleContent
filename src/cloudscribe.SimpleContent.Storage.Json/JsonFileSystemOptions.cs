// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-17
// Last Modified:           2016-03-20
// 

using System.IO;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public class JsonFileSystemOptions
    {
        public string AppRootFolderPath { get; set; }
        public string BaseFolderVPath { get; set; } = "/cloudscribe_config/data_json";
        public string ProjectsFolderName { get; set; } = "projects";
        public string ProjectIdFolderName { get; set; } = "project1";
        public string PagesFolderName { get; set; } = "pages";
        public string PostsFolderName { get; set; } = "posts";

        public string FolderSeparator
        {
           get { return Path.DirectorySeparatorChar.ToString(); }
        } //= "\\"; // \ on windows  / on *nix
    }
}
