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
    // TASS-D019 既定出荷手配（コントローラー）
    public class D019Controller : BaseController
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

        // 参照作成画面表示
        public ActionResult Copy(string id)
        {
            ViewBag.CopyFlg = true;
            return View();
        }

        // 計上画面表示
        public ActionResult Summ()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 計上完了画面表示
        public ActionResult SummResult()
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

        // 計上確認画面表示
        public ActionResult SummConfirm()
        {
            return View();
        }

        // 削除確認画面表示
        public ActionResult DeleteConfirm(string id)
        {
            return View();
        }

        // 参照作成確認画面表示
        public ActionResult CopyConfirm()
        {
            return View();
        }


        // ■ダイアログアクション
        // 帳票印刷の実行ダイアログアクション
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

        // 削除画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 削除確認画面へ遷移
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

        // 参照作成画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            ViewBag.CopyFlg = true;

            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 参照作成確認画面へ遷移
                return View(nameof(CopyConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    return View();
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 参照作成画面を維持
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess, true);

                    // 一覧画面へ遷移
                    return RedirectToAction(nameof(Index));
                default:
                    // 参照作成確認画面へ遷移
                    return View(nameof(CopyConfirm));
            }
        }

        // 計上画面の実行ダイアログアクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Summ(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                // 計上確認画面へ遷移
                return View(nameof(SummConfirm));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    // 計上画面へ遷移
                    return RedirectToAction(nameof(Summ));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 計上画面を維持
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "計上を実行しました。");

                    // 計上確認画面へ遷移
                    return RedirectToAction(nameof(SummResult));
                default:
                    // 計上確認画面へ遷移
                    return View(nameof(SummConfirm));
            }
        }
    }
}