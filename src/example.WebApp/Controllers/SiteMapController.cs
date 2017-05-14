using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace example.WebApp.Controllers
{
    public class SiteMapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
