using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

//http://tech-journals.com/jonow/2012/01/25/implementing-xml-rpc-services-with-asp-net-mvc
//http://www.aaron-powell.com/posts/2010-06-16-aspnet-mvc-xml-action-result.html
//http://www.aaron-powell.com/posts/2010-06-16-aspnet-mvc-xml-action-result.html
//https://github.com/myquay/Chq.XmlRpc.Mvc

namespace cloudscribe.MetaWeblog
{
    public class XmlResult : ActionResult
    {
        public XDocument Xml { get; private set; }
        public string ContentType { get; set; }
        //public Encoding Encoding { get; set; }

        public XmlResult(XDocument xml)
        {
            this.Xml = xml;
            this.ContentType = "text/xml";
        }

       

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = this.ContentType;

            if (Xml != null)
            {
                var ms = new MemoryStream();
                await Xml.SaveAsync(ms, SaveOptions.DisableFormatting, CancellationToken.None);
                var bytes = ms.ToArray();
                await context.HttpContext.Response.BodyWriter.WriteAsync(bytes); 
            }
            else
            {
                await base.ExecuteResultAsync(context);
            }
        }
    }
}
