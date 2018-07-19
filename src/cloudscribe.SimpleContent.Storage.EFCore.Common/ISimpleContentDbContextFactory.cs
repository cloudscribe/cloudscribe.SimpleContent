namespace cloudscribe.SimpleContent.Storage.EFCore.Common
{
    public interface ISimpleContentDbContextFactory
    {
        ISimpleContentDbContext CreateContext();
    }
}
