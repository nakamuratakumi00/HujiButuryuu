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

namespace MacssWeb.Areas.Dcos.Controllers
{
    public class D001Controller : BaseController
    {
        public ActionResult Create2()
        {
            return View();
        }
        // GET: Sample
        public ActionResult SubMenu()
        {
            return View();
        }
        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

            ViewBag.Bumon = MacssDb.m_hokan_bumon.ToList();
            return View(MacssDb.m_account.ToList());
        }
        public ActionResult Index2()
        {
            RetrieveAlertParamsToViewBag();

            ViewBag.Bumon = MacssDb.m_hokan_bumon.ToList();
            return View(MacssDb.m_account.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateConfirm(string id)
        {
            var accountMaster = MacssDb.m_account.Find(id);
            return View(accountMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "account_id,account_name,account_name_kana,account_password,group_cd,login_count,login_failure_count,last_login_date,delete_flg,create_id,create_date,update_id,update_date,bumon_cd,basyo_cd,account_password_date,sdek12,ctlfl1")]
            AccountMaster accountMaster, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(CreateConfirm), accountMaster);
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return View(accountMaster);
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View(accountMaster);
                    }
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.CreateSuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(CreateConfirm), accountMaster);
            }
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditConfirm(string id)
        {
            var accountMaster = MacssDb.m_account.Find(id);
            return View(accountMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "account_id,account_name,account_name_kana,account_password,group_cd,login_count,login_failure_count,last_login_date,delete_flg,create_id,create_date,update_id,update_date,bumon_cd,basyo_cd,account_password_date,sdek12,ctlfl1")]
            AccountMaster accountMaster, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditConfirm), accountMaster);
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return View(accountMaster);
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View(accountMaster);
                    }
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(EditConfirm), accountMaster);
            }
        }

        public ActionResult DeleteConfirm(string id)
        {
            var accountMaster = MacssDb.m_account.Find(id);
            return View(accountMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(
            [Bind(Include = "account_id,account_name,account_name_kana,account_password,group_cd,login_count,login_failure_count,last_login_date,delete_flg,create_id,create_date,update_id,update_date,bumon_cd,basyo_cd,account_password_date,sdek12,ctlfl1")]
            AccountMaster accountMaster, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(DeleteConfirm), accountMaster);
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return RedirectToAction(nameof(Index));
                case ButtonValues.Exec:
                    if (!ModelState.IsValid)
                    {
                        return View(accountMaster);
                    }
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.DeleteSuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(DeleteConfirm), accountMaster);
            }
        }


        public ActionResult Details(string id)
        {
            var accountMaster = MacssDb.m_account.Find(id);
            return View(accountMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(
            [Bind(Include = "account_id,account_name,account_name_kana,account_password,group_cd,login_count,login_failure_count,last_login_date,delete_flg,create_id,create_date,update_id,update_date,bumon_cd,basyo_cd,account_password_date,sdek12,ctlfl1")]
            AccountMaster accountMaster, Actions? preAction)
        {

            if (!preAction.HasValue)
            {
                return RedirectToAction("Index");
            }
            string msg = String.Empty;
            string title = String.Empty;
            switch (preAction.Value)
            {
                case Actions.Create:
                    title = "新規作成確認サンプル";
                    msg = Resources.Messages.CreateConfirm;
                    break;
                case Actions.Edit:
                    title = "編集確認サンプル";
                    msg = Resources.Messages.EditConfirm;
                    break;
                case Actions.Delete:
                    title = "削除確認サンプル";
                    msg = Resources.Messages.DeleteConfirm;
                    break;
                default:
                    return RedirectToAction("Index");
            }

            ViewBag.PageMainTitle = title;
            ViewBag.ConfirmMessage = msg;
            ViewBag.TargetAction = preAction;

            return View(accountMaster);
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
        public ActionResult Check()
        {
            return View();
        }
        public ActionResult CheckResult()
        {
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "チェックを完了しました。";


            return View();
        }
    }
}