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
    // TASS-D033 月次集計データ出力（コントローラー）
    public class D033Controller : BaseController
    {
        // ■画面表示処理
        // 改正エネルギー法データ出力画面表示
        public ActionResult Eneprint()
        {
            return View();
        }

        // 改正エネルギー法データ出力完了画面表示
        public ActionResult EneprintResult()
        {
            return View();
        }

        // 月次集計データ出力画面表示
        public ActionResult Monthagg()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 月次集計データ出力確認画面表示
        public ActionResult MonthaggConfirm()
        {
            return View();
        }

        // 月次集計データ出力完了画面表示
        public ActionResult MonthaggResult()
        {
            return View();
        }

        // 運送S全件データ一覧画面表示
        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 運送S全件データ出力パターン登録画面表示
        public ActionResult Setting()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // ■ダイアログアクション
        // 月次集計データ出力確認画面の実行アクション
        [HttpPost]
        public ActionResult MonthaggResult(ButtonValues? cmdtopback, ButtonValues? cmdbtmback, ButtonValues? cmdbtmexec)
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
                    // 月次集計データ出力画面へ遷移
                    return RedirectToAction(nameof(Monthagg));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 画面を維持
                        return View();
                    }

                    // メッセージセット
                    //SetResultAlertParams(AlertTypes.Success, "月次集計データを出力しました。");

                    // 月次集計データ出力完了画面へ遷移
                    return RedirectToAction(nameof(MonthaggResult));
                default:
                    // 画面を維持
                    return View();
            }
        }

        // 運送S全件データ出力パターン登録画面アクション
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setting(ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            // 押下されたボタン判定
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);
            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(Setting));
            }

            switch (ret)
            {
                case ButtonValues.Back:     // 戻るor一覧へ戻る
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "パターンを削除しました。");

                    return RedirectToAction(nameof(Index));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View();
                    }

                    // メッセージセット
                    SetResultAlertParams(AlertTypes.Success, "パターンを登録しました。");

                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(Setting));
            }
        }
    }
}