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

namespace MacssWeb.Areas.Dcos.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Dcos/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SubMenu()
        {
            return View();
        }
    }
}