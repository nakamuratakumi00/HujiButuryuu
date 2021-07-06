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
    public class BatchController : BaseController
    {
        public ActionResult BatchList()
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
            ViewBag.ResultMessage = "起動を完了しました。";
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}