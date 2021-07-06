using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Controllers;
using Macss.Models;
using Macss.Models.Service;
using Macss.Repositories;
using Macss.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Macss.Attributes.ActionFilter
{
    public class StartActionFilter : ActionFilterAttribute
    {
        #region 定数

        private const string ControllerName = "Controller";
        private const string ActionName = "Action";
        private const string AreaName = "area";

        private const string HomeController = "Home";
        private const string LoginAction = "login";

        private const string NonParent = "MAIN";

        private IUseStatusRepository useStatusRepository;

        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // セッション切れの場合、ログイン画面へ遷移
            if (filterContext.RouteData.Values[ControllerName] as string != HomeController &&
                filterContext.RouteData.Values[ActionName] as string != LoginAction)
            {
                var session = filterContext.HttpContext.Session;
                var sessionInfo = session[SessionExtensions.Field.UserID];

                if (sessionInfo == null)
                {
                    filterContext.Result = new RedirectResult("/" + HomeController + "/" + LoginAction);
                    return;
                }
            }

            if (filterContext.RouteData.Values[ControllerName] as string == HomeController &&
                filterContext.RouteData.Values[ActionName] as string == "Index")
            {
                // URL直接指定チェック
                if (filterContext.HttpContext.Request.ServerVariables["HTTP_REFERER"] == null)
                {
                    filterContext.Result = new RedirectResult("/" + HomeController + "/" + LoginAction);
                    return;
                }
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var dbContext = filterContext.Controller.ControllerContext.HttpContext.GetOwinContext().Get<ApplicationDB>();
            useStatusRepository = new UseStatusRepository(dbContext);

            // メニューと画面タイトルを設定
            var menu = new List<MenuViewModels>();
            var title = "";

            var controllerN = "";
            var actionN = "";

            var session = filterContext.HttpContext.Session;
            var sessionMenu = session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            if (sessionMenu != null)
            {
                // メニューを設定
                menu.AddRange(sessionMenu.Select(x => x.Clone()).Where(x => x.MenuKbn == 1));
                var controllerName = filterContext.RouteData.Values[ControllerName] as string;
                var controllerNameTmp = controllerName.Clone();
                var areaName = filterContext.RouteData.DataTokens[AreaName] as string;
                var actionName = filterContext.RouteData.Values[ActionName];
                if (areaName != null)
                {
                    controllerNameTmp = areaName + "/" + controllerNameTmp;
                }
                var targetMenu = menu.FirstOrDefault(x => x.ControllerName == controllerNameTmp as string && x.ActionName == actionName as string);

                if (targetMenu != null)
                {
                    // タイトルを設定
                    title = targetMenu.TitleName;

                    // 起動中の画面のサブメニューを展開
                    var menuIndex = menu.IndexOf(targetMenu);
                    MenuViewModels startData = menu[menuIndex];
                    startData.IsTarget = true;
                    menu[menuIndex] = startData;

                    // 起動中の画面の親項目にフラグを立てる（サイドメニューの初期展開に使用）
                    var parentId = menu[menuIndex].ParentId;
                    menuIndex--;

                    for (; 0 <= menuIndex; menuIndex--)
                    {
                        MenuViewModels menuData = menu[menuIndex];
                        if (parentId == menuData.MenuId)
                        {
                            menuData.IsExpansion = true;
                            menu[menuIndex] = menuData;

                            parentId = menuData.ParentId;
                        }

                        if (parentId == NonParent)
                        {
                            break;
                        }
                    }
                }
                controllerN = controllerNameTmp as string;
                actionN = actionName as string;
            } 

            AccessMonitoringAsync(dbContext, session, controllerN as string, actionN as string);

            filterContext.Controller.ViewBag.Menu = menu;
            filterContext.Controller.ViewBag.Title = title;
        }

        private void AccessMonitoringAsync(ApplicationDB dbContext, HttpSessionStateBase session, string controllerName, string actionName)
        {

            if (session == null) return;
            var sessionInfo = session[SessionExtensions.Field.UserID];
            if (sessionInfo == null) return;
            if (actionName == "Login") return;

            var sessionID = session.SessionID;
            var title = "";
            var menu = new List<MenuViewModels>();
            var sessionMenu = session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            if (sessionMenu != null)
            {
                menu.AddRange(sessionMenu.Select(x => x.Clone()));
                actionName = actionName == "Serch" ? "Index" : actionName == "Search" ? "Index" : actionName == "All" ? "Index" : actionName;
                var targetMenu = menu.FirstOrDefault(x => x.ControllerName == controllerName as string && x.ActionName == actionName as string);
                if (targetMenu != null)
                {
                    title = targetMenu.TitleName;
                }
            }
            if (title == string.Empty && controllerName == "Home")
            {
                title = "メニュー";
            }

            UseStatusService.UpdateUseStatus(dbContext, sessionID, SessionExtensions.GetUserID(session), title, actionName, controllerName);

        }
    }
}