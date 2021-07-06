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
    public class D008Controller : BaseController
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
            ViewBag.ConfirmMessage = "Fe請求データを作成します。よろしいですか？";
            ViewBag.TargetCount = String.Format("{0} 件", 3);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeResult()
        {
            ViewBag.MakeCountFe = String.Format("{0} 件", 1);
            ViewBag.MakeCountAll = String.Format("{0} 件", 2);
            ViewBag.TotalKingakuFe = String.Format("{0:#,0} 円", 2800);
            ViewBag.TotalKingakuAll = String.Format("{0:#,0} 円", 4300);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.MakeCompleteSuccess;

            ViewBag.ResultInfoMessage = "作成したFe請求データを共有フォルダーへ保存しました。";

            return View();
        }


    }
}