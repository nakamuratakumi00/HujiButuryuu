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

namespace MacssWeb.Areas.Base.Controllers
{
    public class D315Controller : BaseController
    {
        public ActionResult Index()
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
                return View(nameof(Index));
            }

            var ext = Path.GetExtension(postedFile.FileName);

            if (String.Compare(ext, ".xlsx", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Index));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            //postedFile.SaveAs(uploadPath);

            var model = MacssDb.m_account.ToList();
            var list = new List<AccountMasterUpload>();

            var i = 0;
            var cntNew = 0;
            var cntUpd = 0;
            var cntDel = 0;

            foreach (var item in model)
            {
                var uploadData = new AccountMasterUpload(item);

                uploadData.Index = ++i;

                if (i % 3 == 0)
                {
                    uploadData.ProcType = ProcTypes.Update;
                    cntUpd++;
                }
                else if (i % 7 == 0)
                {
                    uploadData.ProcType = ProcTypes.Delete;
                    cntDel++;
                }
                else
                {
                    uploadData.ProcType = ProcTypes.New;
                    cntNew++;
                }

                list.Add(uploadData);

            }

            TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
                , postedFile.FileName, postedFile.ContentLength);
            ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult(string uploadFileName)
        {
            var list = TempData["UploadedList"] as List<AccountMasterUpload>;
            var resultList = new List<AccountMasterUpload>();

            foreach (var item in list)
            {
                if (item.Index % 5 == 0)
                {
                    item.ErrorMessage = "既に登録されています。";
                    item.HasError = true;
                }
                else if (item.Index % 14 == 0)
                {
                    item.ErrorMessage = "対象データがありません。";
                    item.HasError = true;
                }
                else
                {
                    item.HasError = false;
                }

                if (item.HasError)
                {
                    ViewBag.HasError = true;
                    resultList.Add(item);
                }
            }

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", list.Count);
            ViewBag.ErrorCount = String.Format("{0} 件", resultList.Count);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailure;
            }


            return View(resultList);

        }

    }
}