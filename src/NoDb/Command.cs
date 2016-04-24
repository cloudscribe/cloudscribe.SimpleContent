// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-24
// 


using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public class Command<T> : ICommand<T> where T : class
    {
        public Command(
            ILogger<Command<T>> logger,
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
        
        public async Task<bool> CreateAsync(
            string projectId,
            string key,
            T obj, 
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var serialized = serializer.Serialize(obj);
            var pathToFile = await pathResolver.ResolvePath(projectId, key, obj, true).ConfigureAwait(false);

            if (File.Exists(pathToFile)) throw new InvalidOperationException("can't create file that already exists: " + pathToFile);
            
            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(serialized);
            }
            
            return true;
        }

        public async Task<bool> UpdateAsync(
            string projectId,
            string key,
            T obj,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var serialized = serializer.Serialize(obj);
            var pathToFile = await pathResolver.ResolvePath(projectId, key, obj, false).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) throw new InvalidOperationException("can't update file that doesn't exist: " + pathToFile);

            File.Delete(pathToFile); // delete the old version

            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(serialized);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            var pathToFile = await pathResolver.ResolvePath(projectId, key).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) return false;

            File.Delete(pathToFile);

            return true;

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
