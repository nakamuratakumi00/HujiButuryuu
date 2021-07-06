using Macss.Database.Entity;
using MacssWeb.Common;
using MacssWeb.Models;
using Sic.Common.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MacssWeb.Controllers;
using System.Net;
using System.IO;

namespace MacssWeb.Areas.Dcos.Controllers
{
    public class CustomerController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "照合を完了しました。";

            //object model = MacssDb.d_customer.ToList();
            //return View(model);
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

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile.FileName));

            //postedFile.SaveAs(uploadPath);

            //var model = MacssDb.d_customer.ToList();
            //var list = new List<CustomerDataUpload>();

            //var i = 0;
            //var cntNew = 0;
            //var cntUpd = 0;
            //var cntDel = 0;

            //foreach (var item in model)
            //{
            //    var uploadData = new CustomerDataUpload(item);

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

            //TempData["UploadedList"] = list;

            //ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;
            //ViewBag.UploadFileName = postedFile.FileName;
            //ViewBag.UploadFileNameWithSize = String.Format("{0} (サイズ：{1:#,0}バイト)"
            //    , postedFile.FileName, postedFile.ContentLength);
            //ViewBag.UploadFileContentsCount = String.Format("{0} 件", i);

            //ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            //ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            //ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

//            return View(list);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult(string uploadFileName)
        {
            //var list = TempData["UploadedList"] as List<CustomerDataUpload>;
            //var resultList = new List<CustomerDataUpload>();

            //foreach (var item in list)
            //{
            //    if (item.Index % 5 == 0)
            //    {
            //        item.ErrorMessage = "商品コードが空白！！";
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
            ViewBag.HasError = true;

            ViewBag.UploadFileName = uploadFileName;
            ViewBag.UploadedCount = String.Format("{0} 件", 7);
            ViewBag.ErrorCount = String.Format("{0} 件", 1);

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = Resources.Messages.UploadCompleteSuccess;
            if (ViewBag.HasError)
            {
                ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Warning);
                ViewBag.ResultMessage = Resources.Messages.UploadCompleteFailure;
            }


            //return View(resultList);
            return View();

        }
    }
}
