using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Pats.Controllers
{
    public class HomeController : Controller
    {
        // GET: Pats/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}