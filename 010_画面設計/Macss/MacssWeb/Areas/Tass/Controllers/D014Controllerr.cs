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
    public class D014Controller : BaseController
    {

        public ActionResult Index()
        {
            RetrieveAlertParamsToViewBag();

            //ViewBag.Bumon = MacssDb.m_hokan_bumon.ToList();
            return View();
        }




    }
}