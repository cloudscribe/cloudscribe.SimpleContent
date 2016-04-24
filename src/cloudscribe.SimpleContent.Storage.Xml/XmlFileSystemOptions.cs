// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-03-20
// 


namespace cloudscribe.SimpleContent.Storage.Xml
{
    public class XmlFileSystemOptions
    {
        public string AppRootFolderPath { get; set; }
        public string BaseFolderVPath { get; set; } = "/cloudscribe_config/data_xml";
        public string ProjectsFolderName { get; set; } = "projects";
        public string ProjectIdFolderName { get; set; } = "project1";
        public string PostsFolderName { get; set; } = "post";

        // not supporting xml for pages at this time, json is a better format, the main benefit
        // of xml format is compatibility with MiniBlog files and MiniBlog importer which can import other formats as well
        //public string PagesFolderName { get; set; } = "pages"; 
        public string FolderSeparator { get; set; } = "\\"; // \ on windows  / on *nix
    }
}
