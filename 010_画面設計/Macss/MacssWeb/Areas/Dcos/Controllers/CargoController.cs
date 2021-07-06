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
using System.IO;

namespace MacssWeb.Areas.Dcos.Controllers
{
    public class CargoController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirm()
        {
            return View();
        }

        public ActionResult Details()
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
        
        public ActionResult Exec()
        {
            return View();
        }
        public ActionResult ExecConfirm()
        {
            return View();
        }

        public ActionResult ExecComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "作成を完了しました。";

            return View();
        }
        public ActionResult Inquiry()
        {

            return View();
        }
    }
}
