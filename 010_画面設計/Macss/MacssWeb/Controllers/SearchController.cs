using System;
using System.Linq;
using System.Web.Mvc;

namespace MacssWeb.Controllers
{
    public class SearchController : BaseController
    {
        [HttpPost]
        public ActionResult SearchDepartments(string code, string name)
        {
            if (!Request.IsAjaxRequest())
            {
                return new EmptyResult();
            }

            var result = MacssDb.m_hokan_bumon.AsQueryable();

            if (!String.IsNullOrEmpty(code))
            {
                result = result.Where(item => item.BMNCOD.Contains(code));
            }
            if (!String.IsNullOrEmpty(name))
            {
                result = result.Where(item => item.BASYO.Contains(name));
            }

            return Json(result.ToList());
        }
    }
}