



using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public static class HttpRequestExtensions
    {
        public static string RawUrl(this HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Scheme))
            {
                throw new InvalidOperationException("Missing Scheme");
            }
            if (!request.Host.HasValue)
            {
                throw new InvalidOperationException("Missing Host");
            }
            string path = (request.PathBase.HasValue || request.Path.HasValue) ? (request.PathBase + request.Path).ToString() : "/";
            return request.Scheme + "://" + request.Host + path + request.Query.ToString();
        }

        public static string AppBaseUrl(this HttpRequest request)
        {
            if (string.IsNullOrEmpty(request.Scheme))
            {
                throw new InvalidOperationException("Missing Scheme");
            }
            if (!request.Host.HasValue)
            {
                throw new InvalidOperationException("Missing Host");
            }
            //string path = (request.PathBase.HasValue || request.Path.HasValue) ? (request.PathBase + request.Path).ToString() : "/";
            return request.Scheme + "://" + request.Host;
        }
    }
}
