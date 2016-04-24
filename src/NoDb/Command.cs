// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-23
// 


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
    public class Command<TObject> : ICommand<TObject> where TObject : class
    {
        public Command(
            ILogger<Command<TObject>> logger,
            IStoragePathResolver<TObject> pathResolver
            
            )
        {
            this.pathResolver = pathResolver;
            log = logger;
        }

        private IStoragePathResolver<TObject> pathResolver;
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

            
            var json = JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );
            
            var pathToFile = await pathResolver.ResolvePath(projectId, key, obj, true).ConfigureAwait(false);

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
            
            var json = JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );
            
            var pathToFile = await pathResolver.ResolvePath(projectId, key, obj, false).ConfigureAwait(false);

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
            
            var pathToFile = await pathResolver.ResolvePath(projectId, key).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) return false;

            File.Delete(pathToFile);

            return true;

        }


        


        //private TObject LoadObject(string pathToFile)
        //{
        //    using (StreamReader reader = File.OpenText(pathToFile))
        //    {
        //        var payload = reader.ReadToEnd();
        //        var result = JsonConvert.DeserializeObject<TObject>(payload);
        //        return result;
        //    }
        //}

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
