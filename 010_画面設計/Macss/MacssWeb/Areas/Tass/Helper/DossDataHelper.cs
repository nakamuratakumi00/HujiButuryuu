using MacssWeb.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Helper
{
    public class DossDataHelper
    {
        public static List<SelectItem> DossDataTypeDataList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="すべて", InitSelect=true},
                new SelectItem(){TargetId = 1, TargetText="北関東支社向け", InitSelect=false},
                new SelectItem(){TargetId = 2, TargetText="全社基幹向け", InitSelect=false},
            };

        public static IEnumerable<SelectListItem> RouteSelectList =
            new SelectListItem[] {
                //new SelectListItem() { Value="25：埼玉便 & 26：京浜支社便", Text="25：埼玉便 & 26：京浜支社便", Selected=true },
                new SelectListItem() { Value="25：埼玉便", Text="25：埼玉便", Selected=true },
                new SelectListItem() { Value="81 & W定期輸出", Text="81 & W定期輸出" },
            };

        public static IEnumerable<SelectListItem> LocationSelectList =
            new SelectListItem[] {
                new SelectListItem() { Value="S2：丸子出張所", Text="S2：丸子出張所", Selected=true },
                new SelectListItem() { Value="B3：半導体", Text="B3：半導体" },
            };
    }
}