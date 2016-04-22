

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Razor;
using Microsoft.AspNet.Routing;

//http://stackoverflow.com/questions/30362156/render-razor-view-to-string-in-asp-net-5
//http://stackoverflow.com/questions/34979566/how-to-call-a-partialview-from-outside-the-controller-in-asp-net-5/35012862#35012862

namespace cloudscribe.SimpleContent.Services
{
    public class ViewRenderer
    {
        public ViewRenderer(
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IHttpContextAccessor contextAccesor)
        {
           // services = serviceProvider;
            //this.templateEngine = templateEngine;
            this.viewEngine = viewEngine;
            this.tempDataProvider = tempDataProvider;
            this.contextAccesor = contextAccesor;
        }

        //private IServiceProvider services;
        private ICompositeViewEngine viewEngine;
        private ITempDataProvider tempDataProvider;
        private IHttpContextAccessor contextAccesor;
        //private RazorTemplateEngine templateEngine;

        public async Task<string> RenderPartialViewToString<TModel>(string viewName, TModel model)
        {
            //if (string.IsNullOrEmpty(viewName))
            //    viewName = ActionContext.ActionDescriptor.Name;

            var viewData = new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
            {
                Model = model
            };

            var actionContext = GetActionContext();
            var tempData = new TempDataDictionary(contextAccesor, tempDataProvider);

            //ParserResults res = templateEngine.ParseTemplate();
            //res.Document.
            //viewEngine.


            using (StringWriter sw = new StringWriter())
            {
                
                ViewEngineResult viewResult = viewEngine.FindView(actionContext, viewName);
                //RazorView view = new RazorView()

                ViewContext viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewData,
                    tempData,
                    sw,
                    new HtmlHelperOptions() 
                );

                await viewResult.View.RenderAsync(viewContext);
                //t.Wait();

                return sw.GetStringBuilder().ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            //var httpContext = new DefaultHttpContext();
            //httpContext.RequestServices = services;
            return new ActionContext(contextAccesor.HttpContext, new RouteData(), new ActionDescriptor());

            //return new ActionContext();
        }

    }
}
