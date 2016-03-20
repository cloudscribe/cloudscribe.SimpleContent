// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-18
// Last Modified:           2016-03-20
// 

using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public class JsonFileSystemPersister : IJsonPersister
    {
        public JsonFileSystemPersister(
            ProjectFilePathResolver pathResolver,
            ILogger<JsonFileSystemPersister> logger)
        {
            this.pathResolver = pathResolver;
            log = logger;
        }

        private ProjectFilePathResolver pathResolver;
        private ILogger log;

        public async Task SavePageFile(string projectId, string pageId, string json)
        {
            await pathResolver.EnsureInitialized(projectId).ConfigureAwait(false);

            var folderPath = pathResolver.GetPagesFolderPath();
            var filePath = folderPath + pageId + ".json";

            if (File.Exists(filePath)) { File.Delete(filePath); }

            try
            {
                using (StreamWriter s = File.CreateText(filePath))
                {
                    await s.WriteAsync(json);
                }
            }
            catch(IOException ex)
            {
                log.LogError("handled error, will try to ensure the folders exist and retry saving the file one time", ex);
                await pathResolver.EnsureFoldersExist(projectId);
                using (StreamWriter s = File.CreateText(filePath))
                {
                    await s.WriteAsync(json);
                }
            }
            
        }

        public async Task DeletePageFile(string projectId, string pageId)
        {
            await pathResolver.EnsureInitialized(projectId).ConfigureAwait(false);
            var folderPath = pathResolver.GetPagesFolderPath();
            var filePath = folderPath + pageId + ".json";

            File.Delete(filePath);
        }

    }
}
