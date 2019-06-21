using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace cloudscribe.PwaKit
{
    internal class ServiceWorkerTagHelperComponent : TagHelperComponent
    {
        
        private IHostingEnvironment _env;
        private IHttpContextAccessor _contextAccessor;
        private PwaOptions _options;
        private IUrlHelperFactory _urlHelperFactory;
        private IActionContextAccessor _actionContextAccesor;

        public ServiceWorkerTagHelperComponent(
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IHostingEnvironment env, 
            IHttpContextAccessor accessor, 
            IOptions<PwaOptions> pwaOptionsAccessor
            )
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _env = env;
            _contextAccessor = accessor;
            _options = pwaOptionsAccessor.Value;

        }

        /// <inheritdoc />
        public override int Order => -1;

        private string BuildScript()
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            var url = urlHelper.Action("ServiceWorkerInit", "Pwa");

            var script = "\r\n\t<script" + (_options.EnableCspNonce ? PwaConstants.CspNonce : string.Empty) + " src='" + url + "'></script>";

            return script;
        }

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(!string.IsNullOrWhiteSpace(_options.ExcludedPathsCsv))
            {
                var currentPath = _contextAccessor.HttpContext.Request.Path.ToString();
                var paths = _options.ExcludedPathsCsv.Split(',').ToList();
                if(paths.Any(x => currentPath.StartsWith(x)))
                {
                    return;
                }
                
            }

            if(_contextAccessor.HttpContext.Request.Method == "POST")
            {
                return;
            }

            if (!_options.RegisterServiceWorker)
            {
                return;
            }

            if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
            {
                if ((_options.AllowHttp || _contextAccessor.HttpContext.Request.IsHttps) || _env.IsDevelopment())
                {
                    output.PostContent.AppendHtml(BuildScript());
                }
            }
        }
    }
}
