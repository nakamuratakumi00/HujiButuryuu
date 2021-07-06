using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Macss.Controllers;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Attributes.ActionFilter
{
    public class AuthorityActionFilter : ActionFilterAttribute
    {

        #region 定数

        private const string ControllerName = "Controller";
        private const string ActionName = "Action";
        private const string AreaName = "area";

        private const string HomeController = "Home";
        private const string ErrorController = "Error";
        private const string IndexAction = "Index";
        private const string LoginAction = "login";

        public const string AuthorityError = "AuthorityError";

        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // URL直接指定チェック
            if (filterContext.HttpContext.Request.ServerVariables["HTTP_REFERER"] == null)
            {
                filterContext.Result = new RedirectResult("/" + HomeController + "/" + LoginAction);
                return;
            }

            // 権限チェック
            var session = filterContext.HttpContext.Session;
            var sessionMenu = session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            if (sessionMenu != null)
            {
                var areaName = filterContext.RouteData.DataTokens[AreaName] as string;
                var controllerName = filterContext.RouteData.Values[ControllerName] as string;
                var controllerNameTmp = controllerName.Clone() as string;
                var actionName = filterContext.RouteData.Values[ActionName] as string;
                if (areaName != null)
                {
                    controllerNameTmp = areaName + "/" + controllerNameTmp;
                }
                var menu = sessionMenu.Where(x => x.ControllerName == controllerNameTmp)
                                                    .Where(x => x.ActionName == actionName)
                                                    .FirstOrDefault();
                if (menu == null)
                {
                    filterContext.Result = new RedirectResult("/" + ErrorController + "/" + IndexAction + "?id=" + AuthorityError);
                    return;
                }
                var processingId = menu.MenuId;
                session.SetProcessingID(processingId);
            }else
            {
                session.SetProcessingID(null);
            }
        }
    }
}