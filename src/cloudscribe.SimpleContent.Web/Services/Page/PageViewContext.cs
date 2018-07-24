using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageViewContext
    {
        public PageViewContext(
            IProjectSettings project,
            IPage currentPage,
            ContentHistory history,
            bool canEdit,
            bool hasDraft,
            bool hasPublishedVersion,
            bool didReplaceDraft,
            bool didRestoreDeleted,
            bool showingDraft,
            int rootPageCount
            )
        {
            Project = project;
            CurrentPage = currentPage;
            History = history;
            HasDraft = hasDraft;
            CanEdit = canEdit;
            HasPublishedVersion = hasPublishedVersion;
            DidReplaceDraft = didReplaceDraft;
            DidRestoreDeleted = didRestoreDeleted;
            ShowingDraft = showingDraft;
            RootPageCount = rootPageCount;

        }

        public IProjectSettings Project { get; private set; } = null;
        public IPage CurrentPage { get; private set; } = null;
        public bool CanEdit { get; private set; }
        public ContentHistory History { get; private set; } = null;
        public bool HasDraft { get; private set; }
        public bool HasPublishedVersion { get; private set; }
        public bool DidReplaceDraft { get; private set; }
        public bool ShowingDraft { get; private set; }
        public bool DidRestoreDeleted { get; private set; }
        public int RootPageCount { get; private set; } = -1;

    }
}
