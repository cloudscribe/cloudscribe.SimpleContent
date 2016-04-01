using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
            //this.Encoding = Encoding.UTF8;

            //responseDoc = new XDocument(new XElement("methodResponse"));

            //if (data is Exception)
            //{
            //    //Encode as a fault
            //    responseDoc.Element("methodResponse").Add(
            //        new XElement("fault",
            //            new XElement("value",
            //                new XElement("string",
            //                    (data as Exception).Message))));
            //}
            //else
            //{
            //    //Encode as params
            //    responseDoc.Element("methodResponse").Add(
            //        new XElement("params",
            //            new XElement("param",
            //                XmlRpcHelper.SerializeValue(data))));
            //}
        }

#if !DNXCORE50
        public override void ExecuteResult(ActionContext context)
        {
            //base.ExecuteResult(context);
            context.HttpContext.Response.ContentType = this.ContentType;
            //context.HttpContext.Response.HeaderEncoding = this.Encoding;
            XmlTextWriter writer = new XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8);
            Xml.WriteTo(writer);
            writer.Close();
        }
#endif

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = this.ContentType;
            //context.HttpContext.Response.HeaderEncoding = this.Encoding;
            //XmlTextWriter writer = new XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8);
            //Xml.WriteTo(writer);
            //writer.Close();


            if (Xml != null)
            {
                string response = Xml.ToString(SaveOptions.DisableFormatting);
                context.HttpContext.Response.ContentType = "text/xml";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                context.HttpContext.Response.Headers["content-length"] = responseBytes.Length.ToString();

                return context.HttpContext.Response.WriteAsync(response);
            }
            else
            {
                return base.ExecuteResultAsync(context);
            }



        }




    }
}
