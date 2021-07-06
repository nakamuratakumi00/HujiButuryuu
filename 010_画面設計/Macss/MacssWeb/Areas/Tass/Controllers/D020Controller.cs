using Macss.Database.Entity;
using MacssWeb.Common;
using MacssWeb.Models;
using MacssWeb.Controllers;
using Sic.Common.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MacssWeb.Areas.Tass.Controllers
{
    public class D020Controller : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult Edit(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditConfirm()
        {
            return View();
        }
        public ActionResult Delete(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
    }
}