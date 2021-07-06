using Macss.Models;
using Macss.Repositories;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Macss.Controllers
{
    public class UseStatusController : Controller
    {

        private IUseStatusRepository useStatusRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            useStatusRepository = new UseStatusRepository(dbContext);
        }

        // GET: UseStatus
        public ActionResult Index()
        {
            ViewBag.Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> All()
        {
            return Json(await useStatusRepository.GetAllUseStatusAsync());
        }

    }
}