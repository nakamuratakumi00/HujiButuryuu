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

namespace MacssWeb.Areas.Aims.Controllers
{
    public class D004Controller : BaseController
    {

        public ActionResult Make()
        {
            return View();
        }

        public ActionResult MakeComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "作成を完了しました。";
            return View();
        }
        public ActionResult RecordSales()
        {
            return View();
        }
        public ActionResult RecordSalesComplete()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
            ViewBag.ResultMessage = "PCコード設定処理でエラーが発生しました。売上計上処理を中止しました。";
            return View();
        }
    }
}