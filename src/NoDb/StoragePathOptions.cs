// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-23
// 

/*
 example query files by lastwrite:
var directory = new DirectoryInfo(your_dir);
DateTime from_date = DateTime.Now.AddMonths(-3);
DateTime to_date = DateTime.Now;
var files = directory.GetFiles()
  .Where(file=>file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
  */

using System.IO;

namespace NoDb
{
    public class StoragePathOptions
    {
        public string AppRootFolderPath { get; set; }
        public string BaseFolderVPath { get; set; } = "/cloudscribe_config/data_json";
        public string ProjectsFolderName { get; set; } = "projects";
        public string ProjectIdFolderName { get; set; } = "project1";
        

        public string FolderSeparator
        {
            get { return Path.DirectorySeparatorChar.ToString(); }
        } //= "\\"; // \ on windows  / on *nix

    }
}
