// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-04-25
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class NoDbPostRepository
    {
        public NoDbPostRepository(
            IBasicCommands<Post> postCommands,
            IBasicQueries<Post> postQueries,
            ILogger<NoDbPostRepository> logger)
        {
            commands = postCommands;
            query = postQueries;
            log = logger;
        }

        private IBasicCommands<Post> commands;
        private IBasicQueries<Post> query;
        private ILogger log;

    }
}
