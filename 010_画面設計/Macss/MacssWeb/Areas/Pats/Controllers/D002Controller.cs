using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Pats.Controllers
{
    public class D002Controller : Controller
    {
        // GET: Pats/D002
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 検収一覧
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_01()
        {
            ViewBag.Title = Resources.Pats.Acceptance + "一覧";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "一覧";
            ViewBag.ScreenId = "PATS-D002-01";

            return View();
        }

        /// <summary>
        /// 検収詳細確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_02()
        {
            ViewBag.Title = Resources.Pats.Acceptance + "詳細確認";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "詳細確認";
            ViewBag.ScreenId = "PATS-D002-02";
            ViewBag.OnlyShow = true;

            return View();
        }

        /// <summary>
        /// 検収登録
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_03(string kanbun)
        {
            ViewBag.Title = Resources.Pats.Acceptance + "登録";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "登録";
            ViewBag.ScreenId = "PATS-D002-03";
            ViewBag.KanBun = kanbun;

            return View();
        }

        /// <summary>
        /// 検収登録確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_04()
        {
            ViewBag.Title = Resources.Pats.Acceptance + "登録確認";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "登録確認";
            ViewBag.ScreenId = "PATS-D002-04";
            ViewBag.ConfirmMessage = Resources.Messages.CreateConfirm;
            ViewBag.ButtonText = Resources.ControlText.CreateExec;
            ViewBag.ConfirmDialogText = Resources.Messages.CreateConfirm;

            return View();
        }

        /// <summary>
        /// 検収更新
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_05()
        {
            ViewBag.Title = Resources.Pats.Acceptance + "更新";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "更新";
            ViewBag.ScreenId = "PATS-D002-05";

            return View();
        }

        /// <summary>
        /// 検収更新確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_06()
        {
            ViewBag.Title = Resources.Pats.Acceptance + "更新確認";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "更新確認";
            ViewBag.ScreenId = "PATS-D002-06";
            ViewBag.ConfirmMessage = Resources.Messages.EditConfirm;
            ViewBag.ButtonText = Resources.ControlText.EditExec;
            ViewBag.ConfirmDialogText = Resources.Messages.EditReconfirm;

            return View();
        }

        /// <summary>
        /// 検収削除確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D002_07()
        {
            ViewBag.Title = Resources.Pats.Acceptance + "削除確認";
            ViewBag.PageMainTitle = Resources.Pats.Acceptance + "削除確認";
            ViewBag.ScreenId = "PATS-D002-07";
            ViewBag.ConfirmMessage = Resources.Messages.DeleteConfirm;
            ViewBag.ButtonText = Resources.ControlText.DeleteExec;
            ViewBag.ConfirmDialogText = Resources.Messages.DeleteReconfirm;

            return View();
        }
    }
}