using log4net;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Macss.App_Start;
using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Repositories;

namespace Macss
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalFilters.Filters.Add(new StartActionFilter(), 0);
        }


        protected void Application_EndRequest()
        {
            this.Context.GetOwinContext().Get<Models.ApplicationDB>().Dispose();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception exception = Server.GetLastError();

                if (exception != null)
                {
                    var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    logger.Error(exception.Message + "\r\n" + exception.StackTrace);
                }

                Response.Redirect("~/Error/Index");
            }
        }
    }
}
