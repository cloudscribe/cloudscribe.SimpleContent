using Microsoft.AspNetCore.Http;

namespace cloudscribe.SimpleContent.Web
{
    public static class RequestExtensions
    {
        public static string GetCurrentFullUrl(this HttpRequest request)
        {

            return request.Scheme + "://" + request.Host.ToUriComponent() + request.Path.Value;
        }
    }
}
