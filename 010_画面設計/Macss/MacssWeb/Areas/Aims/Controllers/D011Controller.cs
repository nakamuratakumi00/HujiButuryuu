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

namespace MacssWeb.Areas.Aims.Controllers
{
    public class D011Controller : BaseController
    {

        #region "ESP請求項目マスタアップロード"
        public ActionResult Upload01()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConfirm01(HttpPostedFileWrapper postedFile)
        {
            if (postedFile == null)
            {
                ViewBag.ErrorMessage = "ファイルがありません。";
                return View(nameof(Upload01));
            }

            var ext = Path.GetExtension(postedFile.FileName);

            if (String.Compare(ext, ".csv", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Upload01));
            }

            if (Directory.Exists(Server.MapPath("~/App_Data/Uploaded/")) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data/Uploaded/"));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(uploadPath);

            //var model = MacssDb.m_account.ToList();
            var list = new List<AccountMasterUpload>();

            var i = 0;
            var cntNew = 0;
            var cntUpd = 0;
            var cntDel = 0;

            //foreach (var item in model)
            //{
            //    var uploadData = new AccountMasterUpload(item);

            //    uploadData.Index = ++i;

            //    if (i % 3 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Update;
            //        cntUpd++;
            //    }
            //    else if (i % 7 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Delete;
            //        cntDel++;
            //    }
            //    else
            //    {
            //        uploadData.ProcType = ProcTypes.New;
            //        cntNew++;
            //    }

            //    list.Add(uploadData);

            //}

            TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
                , postedFile.FileName, postedFile.ContentLength);
            ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            //return View(list);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult01(string uploadFileName)
        {
            var list = TempData["UploadedList"] as List<AccountMasterUpload>;
            var resultList = new List<AccountMasterUpload>();

            ViewBag.HasError = true;
            //foreach (var item in list)
            //{
            //    if (item.Index % 5 == 0)
            //    {
            //        item.ErrorMessage = "既に登録されています。";
            //        item.HasError = true;
            //    }
            //    else if (item.Index % 14 == 0)
            //    {
            //        item.ErrorMessage = "対象データがありません。";
            //        item.HasError = true;
            //    }
            //    else
            //    {
            //        item.HasError = false;
            //    }

            //    if (item.HasError)
            //    {
            //        ViewBag.HasError = true;
            //        resultList.Add(item);
            //    }
            //}

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", list.Count);
            ViewBag.ErrorCount = String.Format("{0} 件", resultList.Count);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailureConfirm;
            }


            return View(resultList);

        }
        #endregion

        #region "ESPデータ変換マスタアップロード"
        public ActionResult Upload02()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConfirm02(HttpPostedFileWrapper postedFile)
        {
            if (postedFile == null)
            {
                ViewBag.ErrorMessage = "ファイルがありません。";
                return View(nameof(Upload02));
            }

            var ext = Path.GetExtension(postedFile.FileName);

            if (String.Compare(ext, ".csv", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Upload02));
            }

            if (Directory.Exists(Server.MapPath("~/App_Data/Uploaded/")) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data/Uploaded/"));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(uploadPath);

            //var model = MacssDb.m_account.ToList();
            var list = new List<AccountMasterUpload>();

            var i = 0;
            var cntNew = 0;
            var cntUpd = 0;
            var cntDel = 0;

            //foreach (var item in model)
            //{
            //    var uploadData = new AccountMasterUpload(item);

            //    uploadData.Index = ++i;

            //    if (i % 3 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Update;
            //        cntUpd++;
            //    }
            //    else if (i % 7 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Delete;
            //        cntDel++;
            //    }
            //    else
            //    {
            //        uploadData.ProcType = ProcTypes.New;
            //        cntNew++;
            //    }

            //    list.Add(uploadData);

            //}

            TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
                , postedFile.FileName, postedFile.ContentLength);
            ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            //return View(list);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult02(string uploadFileName)
        {
            var list = TempData["UploadedList"] as List<AccountMasterUpload>;
            var resultList = new List<AccountMasterUpload>();

            ViewBag.HasError = true;
            //foreach (var item in list)
            //{
            //    if (item.Index % 5 == 0)
            //    {
            //        item.ErrorMessage = "既に登録されています。";
            //        item.HasError = true;
            //    }
            //    else if (item.Index % 14 == 0)
            //    {
            //        item.ErrorMessage = "対象データがありません。";
            //        item.HasError = true;
            //    }
            //    else
            //    {
            //        item.HasError = false;
            //    }

            //    if (item.HasError)
            //    {
            //        ViewBag.HasError = true;
            //        resultList.Add(item);
            //    }
            //}

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", list.Count);
            ViewBag.ErrorCount = String.Format("{0} 件", resultList.Count);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailureConfirm;
            }


            return View(resultList);

        }
        #endregion

        #region "ESP本社得意先マスタアップロード"
        public ActionResult Upload03()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConfirm03(HttpPostedFileWrapper postedFile)
        {
            if (postedFile == null)
            {
                ViewBag.ErrorMessage = "ファイルがありません。";
                return View(nameof(Upload03));
            }

            var ext = Path.GetExtension(postedFile.FileName);

            if (String.Compare(ext, ".csv", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Upload03));
            }

            if (Directory.Exists(Server.MapPath("~/App_Data/Uploaded/")) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data/Uploaded/"));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(uploadPath);

            //var model = MacssDb.m_account.ToList();
            var list = new List<AccountMasterUpload>();

            var i = 0;
            var cntNew = 0;
            var cntUpd = 0;
            var cntDel = 0;

            //foreach (var item in model)
            //{
            //    var uploadData = new AccountMasterUpload(item);

            //    uploadData.Index = ++i;

            //    if (i % 3 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Update;
            //        cntUpd++;
            //    }
            //    else if (i % 7 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Delete;
            //        cntDel++;
            //    }
            //    else
            //    {
            //        uploadData.ProcType = ProcTypes.New;
            //        cntNew++;
            //    }

            //    list.Add(uploadData);

            //}

            TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
                , postedFile.FileName, postedFile.ContentLength);
            ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            //return View(list);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult03(string uploadFileName)
        {
            var list = TempData["UploadedList"] as List<AccountMasterUpload>;
            var resultList = new List<AccountMasterUpload>();

            ViewBag.HasError = true;
            //foreach (var item in list)
            //{
            //    if (item.Index % 5 == 0)
            //    {
            //        item.ErrorMessage = "既に登録されています。";
            //        item.HasError = true;
            //    }
            //    else if (item.Index % 14 == 0)
            //    {
            //        item.ErrorMessage = "対象データがありません。";
            //        item.HasError = true;
            //    }
            //    else
            //    {
            //        item.HasError = false;
            //    }

            //    if (item.HasError)
            //    {
            //        ViewBag.HasError = true;
            //        resultList.Add(item);
            //    }
            //}

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", list.Count);
            ViewBag.ErrorCount = String.Format("{0} 件", resultList.Count);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailureConfirm;
            }


            return View(resultList);

        }
        #endregion

        #region "ESP本社仕入先マスタアップロード"
        public ActionResult Upload04()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConfirm04(HttpPostedFileWrapper postedFile)
        {
            if (postedFile == null)
            {
                ViewBag.ErrorMessage = "ファイルがありません。";
                return View(nameof(Upload04));
            }

            var ext = Path.GetExtension(postedFile.FileName);

            if (String.Compare(ext, ".csv", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Upload04));
            }

            if (Directory.Exists(Server.MapPath("~/App_Data/Uploaded/")) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data/Uploaded/"));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            postedFile.SaveAs(uploadPath);

            //var model = MacssDb.m_account.ToList();
            var list = new List<AccountMasterUpload>();

            var i = 0;
            var cntNew = 0;
            var cntUpd = 0;
            var cntDel = 0;

            //foreach (var item in model)
            //{
            //    var uploadData = new AccountMasterUpload(item);

            //    uploadData.Index = ++i;

            //    if (i % 3 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Update;
            //        cntUpd++;
            //    }
            //    else if (i % 7 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Delete;
            //        cntDel++;
            //    }
            //    else
            //    {
            //        uploadData.ProcType = ProcTypes.New;
            //        cntNew++;
            //    }

            //    list.Add(uploadData);

            //}

            TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            ViewBag.UploadFileName = postedFile.FileName;
            ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
                , postedFile.FileName, postedFile.ContentLength);
            ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            //return View(list);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult04(string uploadFileName)
        {
            var list = TempData["UploadedList"] as List<AccountMasterUpload>;
            var resultList = new List<AccountMasterUpload>();

            ViewBag.HasError = true;
            //foreach (var item in list)
            //{
            //    if (item.Index % 5 == 0)
            //    {
            //        item.ErrorMessage = "既に登録されています。";
            //        item.HasError = true;
            //    }
            //    else if (item.Index % 14 == 0)
            //    {
            //        item.ErrorMessage = "対象データがありません。";
            //        item.HasError = true;
            //    }
            //    else
            //    {
            //        item.HasError = false;
            //    }

            //    if (item.HasError)
            //    {
            //        ViewBag.HasError = true;
            //        resultList.Add(item);
            //    }
            //}

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", list.Count);
            ViewBag.ErrorCount = String.Format("{0} 件", resultList.Count);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailureConfirm;
            }


            return View(resultList);

        }
        #endregion

        #region "ESP請求引渡しデータ作成"
        public ActionResult MakeSeikyu()
        {
            return View();
        }
        public ActionResult MakeSeikyuConfirm(string id)
        {
            ViewBag.ConfirmMessage = "ESP請求引渡しデータを作成します。よろしいですか？";
            ViewBag.TargetCount = String.Format("{0} 件", 3);
            return View();
        }
        public ActionResult MakeSeikyuResult(string id)
        {
            ViewBag.HasError = true;
            ViewBag.MakeName01 = "ESP請求引渡しデータ[リハーサル用]";
            ViewBag.MakeCount01 = String.Format("{0} 件", 2);
            ViewBag.ErrorCount01 = String.Format("{0} 件", 1);
            ViewBag.MakeName02 = "ESP請求引渡しデータ[リハーサル用]（取消用）";
            ViewBag.MakeCount02 = String.Format("{0} 件", 2);
            ViewBag.ErrorCount02 = String.Format("{0} 件", 1);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.MakeCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.MakeCompleteFailure;
            }

            ViewBag.ResultInfoMessage = "作成したESP請求引渡しデータ、ESPマスタ登録用データを共有フォルダーへ保存しました。";

            return View();
        }
        public ActionResult SumSeikyu()
        {
            return View();
        }
        #endregion
        

        #region "ESP仕入引渡しデータ作成"
        public ActionResult MakeShiire()
        {
            return View();
        }
        public ActionResult MakeShiireConfirm(string id)
        {
            ViewBag.ConfirmMessage = "ESP仕入引渡しデータを作成します。よろしいですか？";
            ViewBag.TargetCount = String.Format("{0} 件", 3);
            return View();
        }
        public ActionResult MakeShiireResult(string id)
        {
            ViewBag.HasError = true;
            ViewBag.MakeName01 = "ESP仕入引渡しデータ[リハーサル用]";
            ViewBag.MakeCount01 = String.Format("{0} 件", 2);
            ViewBag.ErrorCount01 = String.Format("{0} 件", 1);
            ViewBag.MakeName02 = "ESP仕入引渡しデータ[リハーサル用]（取消用）";
            ViewBag.MakeCount02 = String.Format("{0} 件", 2);
            ViewBag.ErrorCount02 = String.Format("{0} 件", 1);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.MakeCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.MakeCompleteFailure;
            }

            ViewBag.ResultInfoMessage = "作成したESP仕入引渡しデータを共有フォルダーへ保存しました。";

            return View();
        }
        public ActionResult SumShiire()
        {
            return View();
        }
        #endregion

    }
}