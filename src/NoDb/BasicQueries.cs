// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-25
// 

/*
 example query files by lastwrite:
var directory = new DirectoryInfo(your_dir);
DateTime from_date = DateTime.Now.AddMonths(-3);
DateTime to_date = DateTime.Now;
var files = directory.GetFiles() 
  .Where(file=>file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
  */

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public class BasicQueries<T> : IBasicQueries<T> where T : class
    {
        public BasicQueries(
            ILogger<BasicQueries<T>> logger,
            IStoragePathResolver<T> pathResolver,
            IStringSerializer<T> serializer = null
            )
        {
            this.serializer = serializer ?? new StringSerializer<T>();
            this.pathResolver = pathResolver;
            log = logger;
        }

        private IStringSerializer<T> serializer;
        private IStoragePathResolver<T> pathResolver;
        private ILogger log;
        
        public virtual async Task<T> FetchAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                projectId, 
                key, 
                serializer.ExpectedFileExtension
                ).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) return null;

            return LoadObject(pathToFile, key);

        }

        public virtual async Task<int> GetCountAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
           
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath(projectId).ConfigureAwait(false);
            if (!Directory.Exists(pathToFolder)) return 0;

            var directory = new DirectoryInfo(pathToFolder);
            return directory.GetFileSystemInfos("*" + serializer.ExpectedFileExtension).Length;
        }


        public virtual async Task<List<T>> GetAllAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath(projectId).ConfigureAwait(false);

            var list = new List<T>();
            if (!Directory.Exists(pathToFolder)) return list;
            foreach (string file in Directory.EnumerateFiles(
                pathToFolder, 
                "*" + serializer.ExpectedFileExtension, 
                SearchOption.AllDirectories) // this is needed for blog posts which are nested in year/month folders
                )
            {
                var key = Path.GetFileNameWithoutExtension(file);
                var obj = LoadObject(file, key);
                list.Add(obj);
            }

            return list;

        }


        protected T LoadObject(string pathToFile, string key)
        {
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                var payload = reader.ReadToEnd();
                var result = serializer.Deserialize(payload, key);
                return result;
            }
        }

        private bool _disposed;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }



    }
}
