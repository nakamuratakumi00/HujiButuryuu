using Macss.Database.Entity;
using MacssWeb.Common;
using MacssWeb.Models;
using Sic.Common.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MacssWeb.Controllers;
using System.Net;

namespace MacssWeb.Areas.Base.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Base/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SubMenu()
        {
            return View();
        }

        public ActionResult SubMenuD100()
        {
            return View();
        }
        public ActionResult SubMenuD200()
        {
            return View();
        }
        public ActionResult SubMenuD300()
        {
            return View();
        }
        public ActionResult SubMenuD400()
        {
            return View();
        }
    }

}