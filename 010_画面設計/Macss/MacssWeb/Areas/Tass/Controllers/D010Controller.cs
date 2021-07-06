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

namespace MacssWeb.Areas.Tass.Controllers
{
    public class D010Controller : BaseController
    {

        //バーコードテスト
        public ActionResult Test()
        {

            return View();
        }
        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();


            ViewBag.ConfirmDialogText1 = "運送会社送状Noデータファイルを指定ディレクトリにデータ出力します。よろしいですか？";
            ViewBag.ConfirmDialogText2 = "運送会社送状Noデータファイルをメール送信します。よろしいですか？";

            //ViewBag.Bumon = MacssDb.m_hokan_bumon.ToList();
            return View();
        }
        public ActionResult Create()
        {

            RetrieveAlertParamsToViewBag();

            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirm()
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;


            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(CreateConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return View();
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess, false);
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction(nameof(Create)); //登録完了後は登録画面に遷移する
                default:
                    return View(nameof(CreateConfirm));
            }
        }
        public ActionResult Edit(string id)
        {
            ViewBag.InputFlg = true;
            //return View(MacssDb.m_account.Find(id));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirm()
        {
            ViewBag.InputFlg = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.InputFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return View();
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(EditConfirm));
            }
        }
        public ActionResult DeleteConfirm(string id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(DeleteConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return RedirectToAction(nameof(Index));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.DeleteSuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(DeleteConfirm));
            }
        }

        public ActionResult Details(string id)
        {
            ViewBag.myId = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var accountMaster = MacssDb.m_account.Find(id);
            //if (accountMaster == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(accountMaster);
            return View();
        }

        public ActionResult Make()
        {
            return View();
        }
        public ActionResult MakeConfirm(string id)
        {
            ViewBag.ConfirmMessage = "顧客提供用送状データを作成します。よろしいですか？";
            ViewBag.TargetCount = String.Format("{0} 件", 3);
            return View();
        }
        //public ActionResult MakeResult(string id)
        //{
        //    ViewBag.HasError = false;
        //    ViewBag.MakeCount = String.Format("{0} 件", 2);
        //    ViewBag.ErrorCount = String.Format("{0} 件", 1);

        //    ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
        //    ViewBag.ResultMessage = Resources.Messages.MakeCompleteSuccess;
        //    if (ViewBag.HasError)
        //    {
        //        ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
        //        ViewBag.ResultMessage = Resources.Messages.MakeCompleteFailure;
        //    }

        //    return View();
        //}
  
    }
}