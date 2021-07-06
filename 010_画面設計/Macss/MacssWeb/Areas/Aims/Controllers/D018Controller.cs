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
        
        public ActionResult Change()
        {
            return View();
        }
        public ActionResult ChangeConfirm(string id)
        {
            ViewBag.ConfirmMessage = "包装・出荷実績データを作成します。よろしいですか？";
            ViewBag.TargetCount = String.Format("{0} 件", 3);
            return View();
        }
        public ActionResult ChangeResult(string id)
        {
            ViewBag.HasError = false;
            ViewBag.MakeCount = String.Format("{0} 件", 2);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "変更を完了しました。";
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.MakeCompleteFailure;
            }

            return View();
        }
    }
}