using log4net;
using MacssWeb.Common;
using MacssWeb.Models.Context;
using Microsoft.Owin.Security;
using Sic.Common.Attribute;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Controllers
{
    public abstract class BaseController : Controller
    {

        protected MacssWebDbContext MacssDb = new MacssWebDbContext();

        protected ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void SetResultAlertParams(AlertTypes alertType, string message, bool alertInfoFlg = false)
        {
            TempData["AlertType"] = alertType;
            TempData["AlertMessage"] = message;
            TempData["AlertInfoFlg"] = alertInfoFlg;
        }

        protected void RetrieveAlertParamsToViewBag()
        {
            ViewBag.ShowAlert = false;

            var alertType = TempData["AlertType"] as AlertTypes?;
            var alertMsg = TempData["AlertMessage"] as string;
            var alertInfoFlg = TempData["AlertInfoFlg"] as bool?;

            if (!alertType.HasValue || string.IsNullOrEmpty(alertMsg))
            {
                return;
            }

            ViewBag.ShowAlert = true;
            ViewBag.AlertType = ValueAttribute.GetValue(alertType.Value);
            ViewBag.AlertMessage = alertMsg;
            if (alertInfoFlg == true)
            {
                ViewBag.AlertInfoFlg = 1;
            }
            else
            {
                ViewBag.AlertInfoFlg = 0;
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected string GetInformationText(string fileName)
        {
            var appDataPath = HttpContext.Server.MapPath("~/App_Data");
            var infoFilePath = Path.Combine(appDataPath, fileName);
            return System.IO.File.ReadAllText(infoFilePath);
        }
    }
}