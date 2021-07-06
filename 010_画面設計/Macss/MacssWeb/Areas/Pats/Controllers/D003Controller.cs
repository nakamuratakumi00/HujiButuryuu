using MacssWeb.Common;
using Sic.Common.Attribute;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
namespace MacssWeb.Areas.Pats.Controllers
{
    public class D003Controller : Controller
    {
        // GET: Pats/D003
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取引実績一覧
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_01()
        {
            ViewBag.Title = Resources.Pats.Trading + "一覧";
            ViewBag.PageMainTitle = Resources.Pats.Trading + "一覧";
            ViewBag.ScreenId = "PATS-D003-01";

            return View();
        }

        /// <summary>
        /// 取引実績詳細確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_02()
        {
            ViewBag.Title = Resources.Pats.Trading + "詳細確認";
            ViewBag.PageMainTitle = Resources.Pats.Trading + "詳細確認";
            ViewBag.ScreenId = "PATS-D003-02";
            ViewBag.OnlyShow = true;

            return View();
        }

        /// <summary>
        /// 売上データ登録
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_03()
        {
            ViewBag.Title = "売上データ登録";
            ViewBag.PageMainTitle = "売上データ登録";
            ViewBag.ScreenId = "PATS-D003-03";

            return View();
        }

        /// <summary>
        /// 売上データ登録確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_04()
        {
            ViewBag.Title = "売上データ登録確認";
            ViewBag.PageMainTitle = "売上データ登録確認";
            ViewBag.ScreenId = "PATS-D003-04";

            return View();
        }

        /// <summary>
        /// 出庫実績登録
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_05()
        {
            ViewBag.Title = "出庫実績登録";
            ViewBag.PageMainTitle = "出庫実績登録";
            ViewBag.ScreenId = "PATS-D003-05";

            return View();
        }

        public ActionResult D003_06()
        {
            ViewBag.Title = "出庫実績登録確認";
            ViewBag.PageMainTitle = "出庫実績登録確認";
            ViewBag.ScreenId = "PATS-D003-06";

            return View();
        }

        /// <summary>
        /// 取引実績更新
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_07()
        {
            ViewBag.Title = Resources.Pats.Trading + "更新";
            ViewBag.PageMainTitle = Resources.Pats.Trading + "更新";
            ViewBag.ScreenId = "PATS-D003-07";

            return View();
        }

        /// <summary>
        /// 取引実績更新確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_08()
        {
            ViewBag.Title = Resources.Pats.Trading + "更新確認";
            ViewBag.PageMainTitle = Resources.Pats.Trading + "更新確認";
            ViewBag.ScreenId = "PATS-D003-08";
            ViewBag.ConfirmMessage = Resources.Messages.EditConfirm;
            ViewBag.ButtonText = Resources.ControlText.EditExec;
            ViewBag.ConfirmDialogText = Resources.Messages.EditReconfirm;

            return View();
        }

        /// <summary>
        /// 取引実績削除確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_09()
        {
            ViewBag.Title = Resources.Pats.Trading + "削除確認";
            ViewBag.PageMainTitle = Resources.Pats.Trading + "削除確認";
            ViewBag.ScreenId = "PATS-D003-09";
            ViewBag.ConfirmMessage = Resources.Messages.DeleteConfirm;
            ViewBag.ButtonText = Resources.ControlText.DeleteExec;
            ViewBag.ConfirmDialogText = Resources.Messages.DeleteReconfirm;

            return View();
        }

        /// <summary>
        /// 受入払出実績データアップロード

        /// </summary>
        /// <returns></returns>
        public ActionResult D003_10()
        {
            ViewBag.Title = "受入払出実績データアップロード";
            ViewBag.PageMainTitle = "受入払出実績データアップロード";
            ViewBag.ScreenId = "PATS-D003-10";
            ViewBag.UploadFileAccept = new List<string>() { ".xlsx" };

            return View();
        }

        /// <summary>
        /// 受入払出実績データアップロード確認
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult D003_11(HttpPostedFileWrapper postedFile)
        {
            ViewBag.Title = "受入払出実績データアップロード確認";
            ViewBag.PageMainTitle = "受入払出実績データアップロード確認";
            ViewBag.ScreenId = "PATS-D003-11";

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = $"{postedFile.FileName} (サイズ：1000バイト)";
            ViewBag.UploadFileContentsCount = "1000 件";

            return View();
        }

        /// <summary>
        /// 受入払出実績データ取込結果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult D003_12(string uploadFileName)
        {
            ViewBag.Title = "受入払出実績データ取込結果";
            ViewBag.PageMainTitle = "受入払出実績データ取込結果";
            ViewBag.ScreenId = "PATS-D003-12";

            ViewBag.HasError = true;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                //ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailure;
                ViewBag.ResultMessage = "エラーデータが存在したため、取込を中止しました。";
            }
            else
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            }

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = "0 件";
            ViewBag.ErrorCount = "1 件";


            return View();
        }

        /// <summary>
        /// 納品明細書出力
        /// </summary>
        /// <returns></returns>
        public ActionResult D003_13()
        {
            ViewBag.Title = "納品明細書出力";
            ViewBag.PageMainTitle = "納品明細書出力";
            ViewBag.ScreenId = "PATS-D003-13";

            return View();
        }

        /// <summary>
        /// 納品明細書出力確認
        /// </summary>
        /// <param name="syubetsu">種別: 1=明細, 2=数量合計</param>
        /// <returns></returns>
        public ActionResult D003_14(int syubetsu)
        {
            var name = (syubetsu == 1 ? "(明細)" : "(数量合計)");
            ViewBag.Title = $"納品明細書{name}出力確認";
            ViewBag.PageMainTitle = $"納品明細書{name}出力確認";
            ViewBag.ScreenId = "PATS-D003-14";
            ViewBag.Syubetsu = syubetsu;

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ResultMessage = $"納品明細書{name}を出力します。よろしいですか？";

            return View();
        }
    }
}