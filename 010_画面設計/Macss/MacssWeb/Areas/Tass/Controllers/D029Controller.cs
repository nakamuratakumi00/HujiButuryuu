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
    // TASS-D029 請求・仕入（コントローラー）
    public class D029Controller : BaseController
    {
        // ■画面表示処理
        // 請求・仕入リハｰサルデータ作成画面表示
        public ActionResult Rehearsal()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 請求・仕入リハｰサルデータ作成確認画面表示
        public ActionResult RehearsalConfirm()
        {
            return View();
        }

        // 請求・仕入リハｰサルデータ作成完了画面表示
        public ActionResult RehearsalResult()
        {
            return View();
        }

        // 請求データ作成画面表示
        public ActionResult Requestdata()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 請求データ作成確認画面表示
        public ActionResult RequestdataConfirm()
        {
            return View();
        }

        // 請求データ作成完了画面表示
        public ActionResult RequestdataResult()
        {
            return View();
        }

        // 仕入データ作成画面表示
        public ActionResult Purchasing()
        {
            RetrieveAlertParamsToViewBag();

            return View();
        }

        // 仕入データ作成確認画面表示
        public ActionResult PurchasingConfirm()
        {
            return View();
        }

        // 仕入データ作成完了画面表示
        public ActionResult PurchasingResult()
        {
            return View();
        }


        // ■ダイアログアクション
        // 請求・仕入リハｰサルデータ作成確認画面の実行アクション
        [HttpPost]
        public ActionResult RehearsalResult(ButtonValues? cmdtopback, ButtonValues? cmdbtmback, ButtonValues? cmdbtmexec)
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
                    // 請求・仕入リハｰサルデータ作成画面へ遷移
                    return RedirectToAction(nameof(Rehearsal));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 画面を維持
                        return View();
                    }

                    // メッセージセット
                    //SetResultAlertParams(AlertTypes.Success, "月次集計データを出力しました。");

                    // 請求・仕入リハｰサルデータ作成完了画面へ遷移
                    return RedirectToAction(nameof(RehearsalResult));
                default:
                    // 画面を維持
                    return View();
            }
        }


        // 請求データ作成確認画面の実行アクション
        [HttpPost]
        public ActionResult RequestdataResult(ButtonValues? cmdtopback, ButtonValues? cmdbtmback, ButtonValues? cmdbtmexec)
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
                    // 請求データ作成画面へ遷移
                    return RedirectToAction(nameof(Requestdata));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 画面を維持
                        return View();
                    }

                    // メッセージセット
                    //SetResultAlertParams(AlertTypes.Success, "月次集計データを出力しました。");

                    // 請求データ作成完了画面へ遷移
                    return RedirectToAction(nameof(RequestdataResult));
                default:
                    // 画面を維持
                    return View();
            }
        }


        // 仕入データ作成確認画面の実行アクション
        [HttpPost]
        public ActionResult PurchasingResult(ButtonValues? cmdtopback, ButtonValues? cmdbtmback, ButtonValues? cmdbtmexec)
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
                    // 仕入データ作成画面へ遷移
                    return RedirectToAction(nameof(Purchasing));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        // 画面を維持
                        return View();
                    }

                    // メッセージセット
                    //SetResultAlertParams(AlertTypes.Success, "月次集計データを出力しました。");

                    // 仕入データ作成完了画面へ遷移
                    return RedirectToAction(nameof(PurchasingResult));
                default:
                    // 画面を維持
                    return View();
            }
        }
    }
}