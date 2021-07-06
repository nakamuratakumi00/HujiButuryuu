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
    public class D003Controller : BaseController
    {

        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }
        public ActionResult Create()
        {

            RetrieveAlertParamsToViewBag();

            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            return View();
        }
        public ActionResult CreateZaiko()
        {

            RetrieveAlertParamsToViewBag();

            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            ViewBag.ZaikoFlg = true;
            return View();
        }

        public ActionResult CreateConfirm()
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            return View();
        }

        public ActionResult CreateZaikoConfirm()
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            ViewBag.ZaikoFlg = true;
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
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess);
                    return RedirectToAction(nameof(Create)); //登録完了後は登録画面に遷移する
                default:
                    return View(nameof(CreateConfirm));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateZaiko(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            ViewBag.ZaikoFlg = true;

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
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess);
                    return RedirectToAction(nameof(Create)); //登録完了後は登録画面に遷移する
                default:
                    return View(nameof(CreateConfirm));
            }
        }
        public ActionResult EditKenpin(string id)
        {
            ViewBag.KenpinFlg = true;
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            return View();
        }
        public ActionResult EditMikenpin(string id)
        {
            ViewBag.KenpinFlg = false;
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            return View();
        }
        public ActionResult EditZaiko(string id)
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            ViewBag.ZaikoFlg = true;
            return View();
        }
        public ActionResult EditDenso(string id)
        {
            ViewBag.KenpinFlg = true;
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            return View();
        }
        public ActionResult Transfer(string id)
        {
            return View();
        }
        public ActionResult EditKenpinConfirm()
        {
            ViewBag.KenpinFlg = true;
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            return View();
        }
        public ActionResult EditMiKenpinConfirm()
        {
            ViewBag.KenpinFlg = false;
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            return View();
        }
        public ActionResult EditZaikoConfirm()
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            ViewBag.ZaikoFlg = true;
            return View();
        }
        public ActionResult EditDensoConfirm()
        {
            ViewBag.KenpinFlg = true;
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            return View();
        }
        public ActionResult TransferConfirm()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKenpin(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditKenpinConfirm));
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
                    return View(nameof(EditKenpinConfirm));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMiKenpin(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditMiKenpinConfirm));
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
                    return View(nameof(EditMiKenpinConfirm));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditZaiko(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;
            ViewBag.ZaikoFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditZaikoConfirm));
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
                    return View(nameof(EditZaikoConfirm));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDenso(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditDensoConfirm));
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
                    return View(nameof(EditDensoConfirm));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.EditFlg = true;
            ViewBag.InputFlg = true;

            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(TransferConfirm));
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
                    SetResultAlertParams(AlertTypes.Success, "振り替えました。 ");
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(TransferConfirm));
            }
        }
        public ActionResult DeleteKenpinConfirm(string id)
        {
            return View();
        }
        public ActionResult DeleteMiKenpinConfirm(string id)
        {
            return View();
        }
        public ActionResult DeleteDensoConfirm(string id)
        {
            return View();
        }
        public ActionResult DeleteZaikoConfirm(string id)
        {
            ViewBag.ZaikoFlg = true;
            return View();
        }
        public ActionResult TransferDelete(string id)
        {
            return View();
        }
        public ActionResult Details(string id)
        {
            ViewBag.myId = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
        public ActionResult DetailsZaiko(string id)
        {
            ViewBag.ZaikoFlg = true;
            ViewBag.myId = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }


    }
}