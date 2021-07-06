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

namespace MacssWeb.Areas.Dcos.Controllers
{
    public class D006Controller : BaseController
    {

        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }
        public ActionResult ExecConfirm()
        {
            return View();
        }

        public ActionResult ExecComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "取込を完了しました。";
            return View();
        }

        public ActionResult List()
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

        public ActionResult RecaptureConfirm()
        {
            return View();
        }
        public ActionResult RecaptureComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "再取込を完了しました。";
            return View();
        }
    }
}