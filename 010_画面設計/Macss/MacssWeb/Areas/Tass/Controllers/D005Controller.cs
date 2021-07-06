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
    // TASS-D005 出荷検品（コントローラー）
    public class D005Controller : BaseController
    {
        // ■画面表示処理
        // 一覧画面表示
        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

            //ViewBag.Bumon = MacssDb.m_hokan_bumon.ToList();
            return View();
        }

        // 詳細画面表示
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // 登録画面表示
        public ActionResult Create()
        {
            RetrieveAlertParamsToViewBag();

            // _Formの動作モードを「新規作成」に変更
            ViewBag.CreateFlg = true;
            return View();
        }

        // 登録画面表示(顧客品番有)
        public ActionResult CreateHinban()
        {
            RetrieveAlertParamsToViewBag();

            // _Formの動作モードを「新規作成」に変更
            ViewBag.CreateFlg = true;
            return View();
        }

        // 更新画面表示
        public ActionResult Edit(string id)
        {
            return View();
        }

        // 返却画面表示
        public ActionResult Return()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 登録確認画面表示
        public ActionResult CreateConfirm()
        {
            return View();
        }

        // 更新確認画面表示
        public ActionResult EditConfirm()
        {
            return View();
        }

        // 取消確認画面表示
        public ActionResult CancelConfirm(string id)
        {
            return View();
        }

        // 返却確認画面表示
        public ActionResult ReturnConfirm()
        {
            return View();
        }

        // ■ダイアログアクション
        // 登録画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(CreateConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    // 前画面へ戻る
                    return View();
                case ButtonValues.Exec:     // 登録
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess, true);

                    // _Formの動作モードを「新規作成」に変更
                    ViewBag.CreateFlg = true;

                    // 登録画面へ遷移
                    return RedirectToAction(nameof(Create));
                default:                    // その他
                    // 登録確認画面へ遷移
                    return View(nameof(CreateConfirm));
            }
        }

        // 更新画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 更新確認画面へ遷移
                return View(nameof(EditConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    // 前画面へ戻る
                    return View();
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 更新確認画面へ遷移
                    return View(nameof(EditConfirm));
            }
        }

        // 取消画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 取消確認画面へ遷移
                return View(nameof(CancelConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "取消しました。");

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 取消確認画面へ遷移
                    return View(nameof(CancelConfirm));
            }
        }

        // 返却画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 返却確認画面へ遷移
                return View(nameof(ReturnConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    return View();
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }
                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "返却しました。");

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 返却確認画面へ遷移
                    return View(nameof(ReturnConfirm));
            }
        }
    }
}