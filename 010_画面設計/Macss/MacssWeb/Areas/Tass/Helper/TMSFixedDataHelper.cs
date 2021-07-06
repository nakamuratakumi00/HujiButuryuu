using System.Collections.Generic;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Helper
{
    public class TMSFixedDataHelper
    {
        public static IEnumerable<SelectListItem> DataTypeSelectList =
          new SelectListItem[] {
                    new SelectListItem() { Value="すべて", Text="すべて", Selected=true },
                    //new SelectListItem() { Value="0", Text="作業依頼データ" },
                    new SelectListItem() { Value="1", Text="請求データ" },
                    new SelectListItem() { Value="2", Text="支払データ" },
          };
    }
}