using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MacssWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception exception = Server.GetLastError();

                if (exception != null)
                {
                    //var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    //logger.Error(exception.Message + "\r\n" + exception.StackTrace);
                }
#if !DEBUG
                Response.Redirect("~/Error/Index");
#endif
            }
        }

    }
}
