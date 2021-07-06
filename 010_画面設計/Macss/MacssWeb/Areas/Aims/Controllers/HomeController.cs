using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Aims.Controllers
{
    public class HomeController : Controller
    {
        // GET: Aims/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}