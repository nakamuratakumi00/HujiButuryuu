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
    public class D018Controller : BaseController
    {

        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            return View();
        }
        public ActionResult Details()
        {
            return View();
        }

        public ActionResult CreateConfirm()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult EditConfirm()
        {
            return View();
        }
        public ActionResult DeleteConfirm()
        {
            return View();
        }




    }
}