using MacssWeb.Common;
using System.Collections.Generic;

namespace MacssWeb.Areas.Dcos.Helper
{

    public class StockAllocationHelper
    {
        public static List<SelectItem> StockAllocationSerachDataList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="すべて", InitSelect=true},
                new SelectItem(){TargetId = 1, TargetText="出庫指示データ（引当前）", InitSelect=false},
                new SelectItem(){TargetId = 2, TargetText="計画作成済み", InitSelect=false},
                new SelectItem(){TargetId = 3, TargetText="計画実施済み", InitSelect=false},
                new SelectItem(){TargetId = 4, TargetText="計画確定済み", InitSelect=false},
            };
        //public static List<SelectItem> StockStatusTargetList =
        //    new List<SelectItem>()
        //    {
        //        new SelectItem(){TargetId = 0, TargetText="ピッキングリスト出力済み", InitSelect=true},
        //        new SelectItem(){TargetId = 1, TargetText="計画実施済み", InitSelect=false},
        //        new SelectItem(){TargetId = 2, TargetText="計画確定済み", InitSelect=false},
        //    };
    }
}