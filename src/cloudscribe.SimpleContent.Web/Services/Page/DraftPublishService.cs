using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.Web.Navigation.Caching;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class DraftPublishService
    {
        public DraftPublishService(
            IProjectService projectService,
            IPageQueries pageQueries,
            IPageCommands pageCommands,
            PageEvents eventHandlers,
            IContentHistoryCommands historyCommands,
            ITreeCache treeCache,
            ILogger<DraftPublishService> logger
            )
        {

            _projectService = projectService;
            _pageQueries = pageQueries;
            _pageCommands = pageCommands;
            _eventHandlers = eventHandlers;
            _historyCommands = historyCommands;
            _navigationCache = treeCache;
            _log = logger;

        }

        private readonly IPageQueries _pageQueries;
        private readonly IPageCommands _pageCommands;
        private readonly IProjectService _projectService;
        private readonly PageEvents _eventHandlers;
        private readonly IContentHistoryCommands _historyCommands;
        private readonly ITreeCache _navigationCache;
        private readonly ILogger _log;

        public async Task PublishReadyDrafts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var settings = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);

            try
            {
                var drafts = await _pageQueries.GetPagesReadyForPublish(settings.Id, cancellationToken);
                foreach (var page in drafts)
                {
                    page.Content = page.DraftContent;
                    page.Author = page.DraftAuthor;
                    page.PubDate = page.DraftPubDate.Value;
                    page.SerializedModel = page.DraftSerializedModel;
                    page.IsPublished = true;

                    page.DraftAuthor = null;
                    page.DraftContent = null;
                    page.DraftSerializedModel = null;
                    page.DraftPubDate = null;

                    await Update(page, settings);

                    await _eventHandlers.HandlePublished(page.ProjectId, page).ConfigureAwait(false);
                    await _historyCommands.DeleteDraftHistory(page.ProjectId, page.Id).ConfigureAwait(false);

                    await _navigationCache.ClearTreeCache();

                    _log.LogDebug($"auto published draft for page {page.Title}");
                }
            }
            catch (OperationCanceledException)
            {
                _log.LogDebug("PublishReadyDrafts for page cancelled");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "PublishReadyDrafts for page threw exception");
            }
        }

        private async Task Update(IPage page, IProjectSettings settings)
        {
            
            await _eventHandlers.HandlePreUpdate(settings.Id, page.Id).ConfigureAwait(false);
            await _pageCommands.Update(settings.Id, page).ConfigureAwait(false);
            await _eventHandlers.HandleUpdated(settings.Id, page).ConfigureAwait(false);
        }

    }
}
