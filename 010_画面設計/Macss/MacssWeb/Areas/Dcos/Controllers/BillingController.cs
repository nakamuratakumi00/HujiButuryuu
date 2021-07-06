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
    public class BillingController : BaseController
    {
        public ActionResult BillingExec()
        {
            return View();
        }

        public ActionResult BillingExecConfirm()
        {
            return View();
        }

        public ActionResult BillingComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "作成を完了しました。";

            return View();
        }

    }
}