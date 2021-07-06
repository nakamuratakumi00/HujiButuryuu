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
    public class D015Controller : BaseController
    {

        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

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
                    SetResultAlertParams(AlertTypes.Success, "前回 [出荷No: 10-03481]、[得意先コード: 8999002] " + Resources.Messages.CreateSuccess, true);
                    //SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess, true);
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

            if (id == "1212")
            {
                ViewBag.AriFlg = true;
            }
            else
            {
                ViewBag.AriFlg = false;
            }

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


        public ActionResult Copy(string id)
        {
            ViewBag.InputFlg = true;
            ViewBag.CopyFlg = true;
            //return View(MacssDb.m_account.Find(id));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyConfirm()
        {
            ViewBag.InputFlg = true;
            ViewBag.CopyFlg = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.InputFlg = true;
            ViewBag.CopyFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(CopyConfirm));
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
                    SetResultAlertParams(AlertTypes.Success, "[出荷No: 10-03481]、[得意先コード: 8999002] " + Resources.Messages.CopySuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(CopyConfirm));
            }
        }

        //サブ画面から戻った場合用
        [HttpGet]
        public ActionResult Edit(string id, string mode, string process)
        {
            ViewBag.InputFlg = true;
            ViewBag.PrevMode = mode;
            ViewBag.PrevProcess = process;
            if (id == "1212")
            {
                ViewBag.AriFlg = true;
            }
            else
            {
                ViewBag.AriFlg = false;
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditConfirm(string id, string mode, string process)
        {
            ViewBag.InputFlg = true;
            ViewBag.PrevMode = mode;
            ViewBag.PrevProcess = process;
            return View();
        }

        [HttpGet]
        public ActionResult Details(string id, string mode, string process)
        {
            ViewBag.PrevMode = mode;
            ViewBag.PrevProcess = process;
            return View();
        }

        [HttpGet]
        public ActionResult Delete(string id, string mode, string process)
        {
            ViewBag.PrevMode = mode;
            ViewBag.PrevProcess = process;
            return View();
        }
        [HttpGet]
        public ActionResult Copy(string id, string mode, string process)
        {
            ViewBag.InputFlg = true;
            ViewBag.PrevMode = mode;
            ViewBag.PrevProcess = process;
            return View();
        }

        [HttpGet]
        public ActionResult CopyConfirm(string id, string mode, string process)
        {
            ViewBag.InputFlg = true;
            ViewBag.PrevMode = mode;
            ViewBag.PrevProcess = process;
            return View();
        }


        //詳細
        public ActionResult DetailsTsumeawase(string id,string transAction)
        {
            ViewBag.myId = id;
            ViewBag.PrevMode = "Tsumeawase";
            ViewBag.TransAction = transAction;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
        public ActionResult DetailsKashikiribin(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.PrevMode = "Kashikiribin";
            ViewBag.TransAction = transAction;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
        public ActionResult DetailsSeikyu(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.PrevMode = "Seikyu";
            ViewBag.TransAction = transAction;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }


        //編集
        public ActionResult EditTsumeawase(string id)
        {
            ViewBag.InputFlg = true;
            ViewBag.myId = id;
            ViewBag.PrevMode = "Tsumeawase";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
        public ActionResult EditKashikiribin(string id)
        {
            ViewBag.InputFlg = true;
            ViewBag.myId = id;
            ViewBag.PrevMode = "Kashikiribin";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
      
        public ActionResult EditConfirmTsumeawase(string id, string transAction)
        {
            ViewBag.InputFlg = true;
            ViewBag.myId = id;
            ViewBag.PrevMode = "Tsumeawase";
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditConfirmKashikiribin(string id, string transAction)
        {
            ViewBag.InputFlg = true;
            ViewBag.myId = id;
            ViewBag.PrevMode = "Kashikiribin";
            ViewBag.TransAction = transAction;
            return View();
        }

        ////編集
        //public ActionResult DeleteTsumeawase(string id)
        //{
        //    ViewBag.DeleteFlg = true;
        //    ViewBag.myId = id;
        //    ViewBag.PrevMode = "Tsumeawase";
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    return View();
        //}
        //public ActionResult DeleteKashikiribin(string id)
        //{
        //    ViewBag.DeleteFlg = true;
        //    ViewBag.myId = id;
        //    ViewBag.PrevMode = "Kashikiribin";
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    return View();
        //}
    }
}