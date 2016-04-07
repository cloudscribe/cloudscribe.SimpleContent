using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;

//http://tech-journals.com/jonow/2012/01/25/implementing-xml-rpc-services-with-asp-net-mvc
//http://www.aaron-powell.com/posts/2010-06-16-aspnet-mvc-xml-action-result.html
//http://www.aaron-powell.com/posts/2010-06-16-aspnet-mvc-xml-action-result.html
//https://github.com/myquay/Chq.XmlRpc.Mvc

namespace cloudscribe.Syndication.Web
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

//#if !DNXCORE50
        public override void ExecuteResult(ActionContext context)
        {
            //context.HttpContext.Response.ContentType = this.ContentType;
            //XmlTextWriter writer = new XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8);
            //Xml.WriteTo(writer);
            //writer.Close();

            context.HttpContext.Response.ContentType = this.ContentType;
            if (Xml != null)
            {
                Xml.Save(context.HttpContext.Response.Body, SaveOptions.DisableFormatting);
   
            }   
        }
//#endif

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = this.ContentType;
            
            if (Xml != null)
            {
                Xml.Save(context.HttpContext.Response.Body, SaveOptions.DisableFormatting);  
                return Task.FromResult(0);
                
            }
            else
            {
                return base.ExecuteResultAsync(context);
            }
        }
    }

    //public class Utf8StringWriter : StringWriter
    //{
    //    public override Encoding Encoding { get { return Encoding.UTF8; } }
    //}
}
