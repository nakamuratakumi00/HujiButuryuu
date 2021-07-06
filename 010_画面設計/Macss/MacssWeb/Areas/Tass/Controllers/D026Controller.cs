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
    // TASS-D026 固定出荷注文書（コントローラー）
    public class D026Controller : BaseController
    {
        // ■画面表示処理
        // 一覧画面表示
        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

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

        // 更新画面表示
        public ActionResult Edit(string id)
        {
            return View();
        }

        // 印刷画面表示
        public ActionResult Print()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 登録確認画面表示
        public ActionResult CreateConfirm()
        {
            ViewBag.InputFlg = true;
            ViewBag.CreateFlg = true;
            return View();
        }

        // 更新確認画面表示
        public ActionResult EditConfirm()
        {
            ViewBag.InputFlg = true;
            return View();
        }

        // 削除確認画面表示
        public ActionResult DeleteConfirm(string id)
        {
            return View();
        }

        // 印刷確認画面表示
        public ActionResult PrintConfirm()
        {
            return View();
        }

        // ■画面アクション
        // 一覧画面アクション
        [HttpPost]
        public ActionResult Index(ButtonValues? cmdtopback, ButtonValues? cmdbtmback, ButtonValues? cmdbtmexec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdtopback, cmdbtmback, cmdbtmexec);
            if (ret == ButtonValues.Nothing)
            {
                // 画面を維持
                return View();
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    return View();
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 画面を維持
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "印刷を実行しました。");

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 画面を維持
                    return View();
            }
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
                // 登録確認画面へ遷移
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

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
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

        // 印刷画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 印刷確認画面へ遷移
                return View(nameof(PrintConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    // 前画面へ戻る
                    return RedirectToAction(nameof(Print));
                case ButtonValues.Exec:     // 登録
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "印刷しました。");

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 印刷確認画面へ遷移
                    return View(nameof(PrintConfirm));
            }
        }

        // 削除画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(DeleteConfirm));
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
                    SetResultAlertParams(AlertTypes.Success, "削除しました。");

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 削除確認画面へ遷移
                    return View(nameof(DeleteConfirm));
            }
        }
    }
}