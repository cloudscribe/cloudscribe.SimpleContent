namespace cloudscribe.PwaKit.Interfaces
{
    public interface IPwaRouteNameProvider
    {
        string GetServiceWorkerRouteName();
        string GetServiceWorkerScope();

        string GetOfflinePageRouteName();
    }
}
