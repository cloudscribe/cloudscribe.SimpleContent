namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public interface ISimpleContentTableNames
    {
        string PageCategoryTableName { get; set; }
        string PageCommentTableName { get; set; }
        string PageTableName { get; set; }
        string PostCategoryTableName { get; set; }
        string PostCommentTableName { get; set; }
        string PostTableName { get; set; }
        string ProjectTableName { get; set; }
        string TablePrefix { get; set; }
    }
}