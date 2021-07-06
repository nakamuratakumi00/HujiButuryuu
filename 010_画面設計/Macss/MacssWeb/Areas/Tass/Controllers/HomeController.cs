using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Controllers
{
    public class HomeController : Controller
    {
        // GET: Tass/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}