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
    public class D009Controller : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Make()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeConfirm()
        {
            ViewBag.ConfirmMessage = "請求引渡しデータを作成します。よろしいですか？";
            ViewBag.TargetCount = String.Format("{0} 件", 3);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeResult()
        {
            ViewBag.HasError = true;
            ViewBag.MakeCount = String.Format("{0} 件", 2);
            ViewBag.ErrorCount = String.Format("{0} 件", 1);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.MakeCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.MakeCompleteFailureConfirm;
            }

            ViewBag.ResultInfoMessage = "作成した請求引渡しデータを共有フォルダーへ保存しました。";

            return View();
        }


    }
}