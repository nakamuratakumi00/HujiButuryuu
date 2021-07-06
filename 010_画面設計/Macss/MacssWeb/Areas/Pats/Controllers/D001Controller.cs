using Macss.Database.Entity;
using MacssWeb.Common;
using MacssWeb.Models;
using Sic.Common.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Pats.Controllers
{
    public class D001Controller : Controller
    {
        // GET: Pats/D001
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 内示・発注一覧
        /// </summary>
        /// <returns></returns>
        public ActionResult D001_01()
        {
            ViewBag.Title = Resources.Pats.Ordering + "一覧";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "一覧";
            ViewBag.ScreenId = "PATS-D001-01";

            return View();
        }

        /// <summary>
        /// 内示・発注詳細確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D001_02()
        {
            ViewBag.Title = Resources.Pats.Ordering + "詳細確認";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "詳細確認";
            ViewBag.ScreenId = "PATS-D001-02";
            ViewBag.OnlyShow = true;

            return View();
        }

        /// <summary>
        /// 内示・発注一括登録
        /// </summary>
        /// <param name="naihatsu"></param>
        /// <returns></returns>
        public ActionResult D001_03(string naihatsu)
        {
            ViewBag.Title = Resources.Pats.Ordering + "一括登録";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "一括登録";
            ViewBag.ScreenId = "PATS-D001-03";
            ViewBag.NaiHatsu = naihatsu;
            ViewBag.ConfirmMessage = Resources.Messages.DeleteConfirm;
            ViewBag.ButtonText = Resources.ControlText.DeleteExec;
            ViewBag.ConfirmDialogText = Resources.Messages.DeleteReconfirm;

            return View();
        }

        /// <summary>
        /// 内示・発注一括登録確認
        /// </summary>
        /// <param name="naihatsu"></param>
        /// <param name="syubetsu"></param>
        /// <returns></returns>
        public ActionResult D001_04(string naihatsu, string syubetsu)
        {
            ViewBag.Title = Resources.Pats.Ordering + "一括登録確認";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "一括登録確認";
            ViewBag.ScreenId = "PATS-D001-04";
            ViewBag.NaiHatsu = naihatsu;
            ViewBag.Syu = syubetsu;
            ViewBag.ConfirmMessage = Resources.Messages.DeleteConfirm;
            ViewBag.ButtonText = Resources.ControlText.DeleteExec;
            ViewBag.ConfirmDialogText = Resources.Messages.DeleteReconfirm;

            return View();
        }

        /// <summary>
        /// 内示・発注個別登録
        /// </summary>
        /// <param name="naihatsu"></param>
        /// <returns></returns>
        public ActionResult D001_05(string naihatsu)
        {
            ViewBag.Title = Resources.Pats.Ordering + "個別登録";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "個別登録";
            ViewBag.ScreenId = "PATS-D001-05";
            ViewBag.NaiHatsu = naihatsu;

            return View();
        }

        /// <summary>
        /// 内示・発注個別登録確認
        /// </summary>
        /// <param name="naihatsu"></param>
        /// <param name="syubetsu"></param>
        /// <returns></returns>
        public ActionResult D001_06(string naihatsu, int syubetsu)
        {
            ViewBag.Title = Resources.Pats.Ordering + "個別登録確認";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "個別登録確認";
            ViewBag.ScreenId = "PATS-D001-06";
            ViewBag.NaiHatsu = naihatsu;
            ViewBag.Syu = syubetsu;
            ViewBag.ConfirmMessage = Resources.Messages.CreateConfirm;
            ViewBag.ButtonText = Resources.ControlText.CreateExec;
            ViewBag.ConfirmDialogText = Resources.Messages.CreateReconfirm;

            return View();
        }

        /// <summary>
        /// 内示・発注更新
        /// </summary>
        /// <param name="naihatsu"></param>
        /// <param name="chuNo"></param>
        /// <returns></returns>
        public ActionResult D001_07(string naihatsu, string chuNo)
        {
            ViewBag.Title = Resources.Pats.Ordering + "更新";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "更新";
            ViewBag.ScreenId = "PATS-D001-07";
            ViewBag.NaiHatsu = naihatsu;
            ViewBag.ChumonNo = chuNo;

            return View();
        }

        /// <summary>
        /// 内示・発注更新確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D001_08()
        {
            ViewBag.Title = Resources.Pats.Ordering + "更新確認";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "更新確認";
            ViewBag.ScreenId = "PATS-D001-08";
            ViewBag.ChumonNo = "1234567890";
            ViewBag.ConfirmMessage = Resources.Messages.EditConfirm;
            ViewBag.ButtonText = Resources.ControlText.EditExec;
            ViewBag.ConfirmDialogText = Resources.Messages.EditReconfirm;

            return View();
        }

        /// <summary>
        /// 内示・発注削除確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D001_09()
        {
            ViewBag.Title = Resources.Pats.Ordering + "削除確認";
            ViewBag.PageMainTitle = Resources.Pats.Ordering + "削除確認";
            ViewBag.ScreenId = "PATS-D001-09";
            ViewBag.ChumonNo = "1234567890";
            ViewBag.ConfirmMessage = Resources.Messages.DeleteConfirm;
            ViewBag.ButtonText = Resources.ControlText.DeleteExec;
            ViewBag.ConfirmDialogText = Resources.Messages.DeleteReconfirm;

            return View();
        }

        /// <summary>
        /// 内示・発注情報アップロード
        /// </summary>
        /// <returns></returns>
        public ActionResult D001_10()
        {
            ViewBag.Title = "内示・発注情報アップロード";
            ViewBag.PageMainTitle = "内示・発注情報アップロード";
            ViewBag.ScreenId = "PATS-D001-10";
            ViewBag.UploadFileAccept = new List<string>() { ".xlsx" };

            return View();
        }

        /// <summary>
        /// 内示・発注情報アップロード確認
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult D001_11(HttpPostedFileWrapper postedFile)
        {
            ViewBag.Title = "内示・発注情報アップロード確認";
            ViewBag.PageMainTitle = "内示・発注情報アップロード確認";
            ViewBag.ScreenId = "PATS-D001-11";

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = $"{postedFile.FileName} (サイズ：1000バイト)";
            ViewBag.UploadFileContentsCount = "1000 件";
            ViewBag.NaijiContentsCount = "500 件";
            ViewBag.HachuContentsCount = "500 件";

            return View();
        }

        /// <summary>
        /// 内示・発注情報取込結果
        /// </summary>
        /// <param name="uploadFileName"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult D001_12(string uploadFileName)
        {
            ViewBag.Title = "内示・発注情報取込結果";
            ViewBag.PageMainTitle = "内示・発注情報取込結果";
            ViewBag.ScreenId = "PATS-D001-12";

            ViewBag.HasError = true;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailure;
            }
            else
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            }

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = "999 件";
            ViewBag.NaijiContentsCount = "499 件";
            ViewBag.HachuContentsCount = "500 件";
            ViewBag.ErrorCount = "1 件";

            return View();
        }
    }
}