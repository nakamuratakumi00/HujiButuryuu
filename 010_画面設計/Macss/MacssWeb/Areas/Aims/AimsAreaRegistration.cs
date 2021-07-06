using System.Web.Mvc;

namespace MacssWeb.Areas.Aims
{
    public class AimsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Aims";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Aims_default",
                "Aims/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}