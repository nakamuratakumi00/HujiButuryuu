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
    public class D030Controller : BaseController
    {
        public ActionResult Details(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult Create(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult CreateConfirm(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult Edit(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditConfirm(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult Delete(string id, string transAction)
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult CreateAll()
        {
            return View();
        }
        public ActionResult CreateAllConfirm()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConfirm(HttpPostedFileWrapper postedFile)
        {
            if (postedFile == null)
            {
                ViewBag.ErrorMessage = "ファイルがありません。";
                return View(nameof(Upload));
            }

            var ext = Path.GetExtension(postedFile.FileName);

            if (String.Compare(ext, ".csv", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Upload));
            }

            if (Directory.Exists(Server.MapPath("~/App_Data/Uploaded/")) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data/Uploaded/"));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(uploadPath);

            var list = new List<AccountMasterUpload>();

            var i = 0;
            var cntNew = 0;
            var cntUpd = 0;
            var cntDel = 0;

            TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
                , postedFile.FileName, postedFile.ContentLength);
            ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult(string uploadFileName)
        {
            var list = TempData["UploadedList"] as List<AccountMasterUpload>;
            var resultList = new List<AccountMasterUpload>();

            ViewBag.HasError = true;

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", list.Count);
            ViewBag.ErrorCount = String.Format("{0} 件", resultList.Count);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Failure);
                ViewBag.ResultMessage = "以下のデータにエラーがあり取込できませんでした。確認してください。";
            }


            return View(resultList);

        }
    }
}