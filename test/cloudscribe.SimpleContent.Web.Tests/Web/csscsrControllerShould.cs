using cloudscribe.SimpleContent.Web.Mvc.Controllers;
using cloudscribe.Web.Common.Helpers;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cloudscribe.SimpleContent.Web.Web
{
    public class csscsrControllerShould
    {

        [Fact]
        public void Return_Expected_Stream_For_PageTree_js()
        {

            var controllerLoggerMock = new Mock<ILogger<CsscsrController>>();
            var resourceHelperMock = new ResourceHelper(new Mock<ILogger<ResourceHelper>>().Object);


            var csscsrController = new CsscsrController(resourceHelperMock, controllerLoggerMock.Object);

            csscsrController.ControllerContext = new ControllerContext();
            csscsrController.ControllerContext.HttpContext = new DefaultHttpContext();
            csscsrController.ControllerContext.HttpContext.Request.Path = "/csscsr/js/pagetree.js";

            var result = csscsrController.Js();

            Assert.IsAssignableFrom<FileStreamResult>(result);
        }

        [Fact]
        public void Return_Expected_Stream_For_BlogCommon_css()
        {

            var controllerLoggerMock = new Mock<ILogger<CsscsrController>>();
            var resourceHelperMock = new ResourceHelper(new Mock<ILogger<ResourceHelper>>().Object);


            var csscsrController = new CsscsrController(resourceHelperMock, controllerLoggerMock.Object);

            csscsrController.ControllerContext = new ControllerContext();
            csscsrController.ControllerContext.HttpContext = new DefaultHttpContext();
            csscsrController.ControllerContext.HttpContext.Request.Path = "/csscsr/css/blog-common.css";

            var result = csscsrController.Css();

            Assert.IsAssignableFrom<FileStreamResult>(result);
            
        }

    }
}