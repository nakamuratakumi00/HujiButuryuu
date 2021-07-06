using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sic.Common.Attribute;
using MacssWeb.Common;

namespace MacssWeb.Areas.Aims.Controllers
{
    public class D012Controller : Controller
    {
        // GET: Aims/D012
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Print()
        {
            ViewBag.Title = Resources.Aims.RequestDocumentPrint;
            ViewBag.PageMainTitle = Resources.Aims.RequestDocumentPrint;
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ResultMessage = "承認が必要です。";

            return View();
        }

        public ActionResult PrintConfirm()
        {
            ViewBag.Title = Resources.Aims.RequestDocumentPrint + "確認";
            ViewBag.PageMainTitle = Resources.Aims.RequestDocumentPrint + "確認";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.TargetCount = String.Format("{0} 件", 30);

            return View();
        }

        public ActionResult PrintResult()
        {
            ViewBag.Title = Resources.Aims.RequestDocumentPrint + "完了";
            ViewBag.PageMainTitle = Resources.Aims.RequestDocumentPrint + "完了";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.TargetCount = String.Format("{0} 件", 30);

            return View();
        }

        public ActionResult PrintApprovalIndex()
        {
            ViewBag.Title = "請求関連データ 承認申請";
            ViewBag.PageMainTitle = "請求関連データ 承認申請";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);

            return View();
        }

        public ActionResult PrintCancelApprovalIndex()
        {
            ViewBag.Title = "請求関連データ 承認解除申請";
            ViewBag.PageMainTitle = "請求関連データ 承認解除申請";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);

            return View();
        }
    }
}