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
    public class MonthlyController : BaseController
    {
        // GET: Dcos/MonthlyDataUpdate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MonthlyExec()
        {
            return View();
        }

        public ActionResult MonthlyExecConfirm()
        {
            return View();
        }

        public ActionResult MonthlyComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "更新を完了しました。";

            return View();
        }
    }
}