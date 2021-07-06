using System.Collections.Generic;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Helper
{
    public class ShippingCostForTMSHelper
    {
        public static IEnumerable<SelectListItem> SupplierSelectList =
           new SelectListItem[] {
                    new SelectListItem() { Value="すべて", Text="すべて", Selected=true },
                    new SelectListItem() { Value="北陸名鉄", Text="北陸名鉄" },
                    new SelectListItem() { Value="信州名鉄", Text="信州名鉄" },
                    new SelectListItem() { Value="東海西濃", Text="東海西濃" },
           };

    }
}