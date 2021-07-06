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

namespace MacssWeb.Areas.Base.Controllers
{
    public class ApprovalController : BaseController
    {
        public ActionResult ApprovalList()
        {
            RetrieveAlertParamsToViewBag();

            ViewBag.Bumon = MacssDb.m_hokan_bumon.ToList();

            //object model = MacssDb.d_approval.ToList();
            //return View(model);
            return View();
        }
        public ActionResult ApprovalDetails(string id)
        {
            //return View(MacssDb.d_approval.Find(id));
            return View();
        }
        public ActionResult ApprovalDetailsBulkDenial(string id)
        {
            //return View(MacssDb.d_approval.Find(id));
            return View();
        }
        public ActionResult ApprovalDetailsBulkRecovery(string id)
        {
            //return View(MacssDb.d_approval.Find(id));
            return View();
        }

        public ActionResult ApprovalConfirmDenial(string id)
        {
            //return View(MacssDb.d_approval.Find(id));
            return View();
        }
        public ActionResult ApprovalConfirmRecovery(string id)
        {
            //return View(MacssDb.d_approval.Find(id));
            return View();
        }
    }
}