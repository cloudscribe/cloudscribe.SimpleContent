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

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public class Storage<TObject> : IStorage<TObject> where TObject : class
    {
        public Storage(
            ILogger<Storage<TObject>> logger,
            IStoragePathOptionsResolver storageOptionsResolver,
            IStoragePathResolver pathResolver = null
            )
        {
            pathOptionsResolver = storageOptionsResolver;
            this.pathResolver = pathResolver ?? new StoragePathResolver();
            log = logger;
        }

        private IStoragePathOptionsResolver pathOptionsResolver;
        private IStoragePathResolver pathResolver;
        private ILogger log;
        
        public async Task<bool> CreateAsync(
            string projectId,
            string key,
            TObject obj, 
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathOptions = await pathOptionsResolver.Resolve(projectId).ConfigureAwait(false);

            var json = JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );
            
            var pathToFile = pathResolver.ResolvePath(pathOptions, typeof(TObject).Name.ToLowerInvariant(), key, true);
            if (File.Exists(pathToFile)) throw new InvalidOperationException("can't create file that already exists: " + pathToFile);
            
            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(json);
            }
            
            return true;
        }

        public async Task<bool> UpdateAsync(
            string projectId,
            string key,
            TObject obj,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathOptions = await pathOptionsResolver.Resolve(projectId).ConfigureAwait(false);

            var json = JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );
            
            var pathToFile = pathResolver.ResolvePath(pathOptions, typeof(TObject).Name.ToLowerInvariant(), key);
            if (!File.Exists(pathToFile)) throw new InvalidOperationException("can't update file that doesn't exist: " + pathToFile);

            File.Delete(pathToFile); // delete the old version

            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(json);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(
            string projectId,
            string key,
            Type type,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (type == null) throw new ArgumentException("type must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var pathOptions = await pathOptionsResolver.Resolve(projectId).ConfigureAwait(false);
            var pathToFile = pathResolver.ResolvePath(pathOptions, type.Name.ToLowerInvariant(), key);

            if (!File.Exists(pathToFile)) return false;

            File.Delete(pathToFile);

            return true;

        }


        public async Task<TObject> FetchAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var pathOptions = await pathOptionsResolver.Resolve(projectId).ConfigureAwait(false);
            var pathToFile = pathResolver.ResolvePath(pathOptions, typeof(TObject).Name.ToLowerInvariant(), key);

            if (!File.Exists(pathToFile)) return null;

            return LoadObject(pathToFile);
            
        }
        
        public async Task<int> GetCountAsync(
            string projectId,
            Type type,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (type == null) throw new ArgumentException("type must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var pathOptions = await pathOptionsResolver.Resolve(projectId).ConfigureAwait(false);
            var pathToFolder = pathResolver.ResolvePath(pathOptions, type.Name.ToLowerInvariant(), string.Empty);
            if (!Directory.Exists(pathToFolder)) return 0;

            var directory = new DirectoryInfo(pathToFolder);
            return directory.GetFileSystemInfos().Length;
        }


        public async Task<List<TObject>> GetAllAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
           
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var pathOptions = await pathOptionsResolver.Resolve(projectId).ConfigureAwait(false);
            var pathToFolder = pathResolver.ResolvePath(pathOptions, typeof(TObject).Name.ToLowerInvariant(), string.Empty);

            var list = new List<TObject>();
            if (!Directory.Exists(pathToFolder)) return list;
            foreach (string file in Directory.EnumerateFiles(pathToFolder, "*.json", SearchOption.TopDirectoryOnly))
            {
                var obj = LoadObject(file);
                list.Add(obj);
            }

           return list;

        }


        private TObject LoadObject(string pathToFile)
        {
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                var payload = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject<TObject>(payload);
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
