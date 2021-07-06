using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Macss.Attributes.ActionFilter;

namespace Macss.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index(string id)
        {
            if (id == AuthorityActionFilter.AuthorityError)
            {
                ViewBag.Message = Resources.Message.CE022;
            }
            else
            {
                ViewBag.Message = Resources.Message.CE001;
            }

            return View();
        }
    }
}