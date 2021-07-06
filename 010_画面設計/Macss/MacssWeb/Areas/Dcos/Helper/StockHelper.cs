using MacssWeb.Common;
using System;
using System.Collections.Generic;

namespace MacssWeb.Areas.Dcos.Helper
{
    public enum StockScreens
    {
        StockDetail = 1,
        StockAllocationIndex = 2,
        StockAutoAllocation = 4,
        StockAutoAllocationResult = 8,
        StockAllocationDetail = 16,
        StockAllocationShow = 32,
        StockAllocationExec = 64,
        StockAllocationPlanCancel = 128,
        StockAllocationPlanFix = 256,
        StockAllocationPlanFixCancel = 512,
        StockAllocationPickingList = 1024,
    }

    public class StockHelper
    {
        public static List<SelectItem> StockSerachDataList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="在庫問い合わせ", InitSelect=true},
                new SelectItem(){TargetId = 1, TargetText="在庫報告明細データ(集約コード別)", InitSelect=false},
                new SelectItem(){TargetId = 2, TargetText="在庫報告明細データ(倉庫別)", InitSelect=false},
                new SelectItem(){TargetId = 3, TargetText="在庫報告明細データ(品名別)", InitSelect=false},
                //new SelectItem(){TargetId = 2, TargetText="在庫報告書データ", InitSelect=false},
                new SelectItem(){TargetId = 4, TargetText="棚卸リスト", InitSelect=false},
                new SelectItem(){TargetId = 5, TargetText="保存期間超過報告書データ", InitSelect=false},
            };
        public static List<SelectItem> StockSerachTargetList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="月次データのみ対象", InitSelect=false},
                new SelectItem(){TargetId = 1, TargetText="日次データと月次データ対象", InitSelect=true},
            };
        public static List<SelectItem> SortColumnList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText=String.Empty, InitSelect=false},
                new SelectItem(){TargetId = 1, TargetText="品名コード", InitSelect=false},
                new SelectItem(){TargetId = 2, TargetText="品名", InitSelect=false},
                new SelectItem(){TargetId = 3, TargetText="規格", InitSelect=false},
                new SelectItem(){TargetId = 4, TargetText="ロットNo.", InitSelect=false},
                new SelectItem(){TargetId = 5, TargetText="当月在庫数", InitSelect=false},
            };
        public static List<SelectItem> InventoryListPrintTypeList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="通常", InitSelect=true},
                new SelectItem(){TargetId = 1, TargetText="別様式", InitSelect=false},
            };
        public static List<SelectItem> ClosingDateList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="10日締", InitSelect=true},
                new SelectItem(){TargetId = 1, TargetText="20日締", InitSelect=false},
                new SelectItem(){TargetId = 2, TargetText="末締", InitSelect=false},
            };
        public static List<SelectItem> StockReportModeList =
            new List<SelectItem>()
            {
                new SelectItem(){TargetId = 0, TargetText="随時（印刷のみ）", InitSelect=true},
                new SelectItem(){TargetId = 1, TargetText="最終（印刷と更新）", InitSelect=false},
                new SelectItem(){TargetId = 2, TargetText="最終（更新のみ）", InitSelect=false},
            };
    }
}