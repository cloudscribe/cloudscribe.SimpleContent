// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2018-07-03
// Last Modified:           2018-07-03
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    public class ContentHistoryController : Controller
    {
        public ContentHistoryController(
            IProjectService projectService,
            IContentHistoryQueries historyQueries,
            ILogger<ContentHistoryController> logger
            )
        {
            ProjectService = projectService;
            HistoryQueries = historyQueries;
            Log = logger;
        }

        protected IProjectService ProjectService { get; private set; }
        protected IContentHistoryQueries HistoryQueries { get; private set; }
        protected ILogger Log { get; private set; }

        [Authorize(Policy = "ViewContentHistoryPolicy")]
        public virtual async Task<IActionResult> Index(
            CancellationToken cancellationToken,
            int pageNumber = 1,
            int pageSize = 10,
            int sortMode = 0,
            string contentSource = null,
            string editor = null

            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();
            if (project == null)
            {
                Log.LogError("project settings not found returning 404");
                return NotFound();
            }

            var model = new ContentHistoryViewModel()
            {
                History = await HistoryQueries.GetList(
                project.Id,
                contentSource,
                editor,
                pageNumber,
                pageSize,
                sortMode,
                cancellationToken),
                ContentSource = contentSource,
                Editor = editor,
                SortMode = sortMode
            };
            
            return View(model);
        }

    }
}
