using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    /// <summary>
    /// 表示用のテーブルクラス
    /// </summary>
    [Table("dummy_t_stock")]
    public class DummyStock : Common
    {

        public enum StockStatus
        {
            OutOrder = 1,
            PlanCreate = 2,
            PlanExecution = 4,
            PickingList = 8,
            AllocFix = 16,
            Complete = 32,
        }
        public enum PickingListOutputStatus
        {
            Nothing = 0,
            NoOutput = 1,
            Output = 2,
        }

        [NotMapped]
        [DisplayName("整理No.")]
        public string Number { get; set; }

        [NotMapped]
        [DisplayName("ステータス")]
        public StockStatus Status { get; set; }

        [NotMapped]
        [DisplayName("ステータス")]
        public string StatusString
        {
            get
            {
                switch (this.Status)
                {
                    case StockStatus.OutOrder:
                        return "出庫指示";
                    case StockStatus.PlanCreate:
                        return "計画作成";
                    case StockStatus.PlanExecution:
                        return "計画実施";
                    case StockStatus.PickingList:
                        return "ピッキング";
                    case StockStatus.AllocFix:
                        return "出庫確定済";
                    case StockStatus.Complete:
                        return "出庫実績";
                    default:
                        return String.Empty;
                }
            }
        }
        [NotMapped]
        [DisplayName("入出庫年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime InOutYMD { get; set; }

        [NotMapped]
        [DisplayName("移動前入出庫年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? BeforeInOutYMD { get; set; }

        [NotMapped]
        [DisplayName("入庫年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime InYMD { get; set; }

        [NotMapped]
        [DisplayName("出庫年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime OutYMD { get; set; }

        [NotMapped]
        [DisplayName("出庫指示年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime OutOrderYMD { get; set; }

        [NotMapped]
        [DisplayName("倉庫")]
        public string Warehouse
        {
            get
            {
                return String.Format(CodeNameFormat, this.WarehouseCode, this.WarehouseName);
            }

        }

        [Column("warehouse_cd")]
        [DisplayName("倉庫コード")]
        public string WarehouseCode { get; set; }

        [NotMapped]
        [DisplayName("倉庫名")]
        public string WarehouseName { get; set; }

        [NotMapped]
        [DisplayName("棚番")]
        public string RackNumber { get; set; }

        [NotMapped]
        [DisplayName("移動元棚番")]
        public string CurrentRackNumber { get { return this.RackNumber; } }

        [NotMapped]
        [DisplayName("移動先棚番")]
        public string DestinationRackNumber { get; set; }

        [NotMapped]
        [DisplayName("移動前棚番")]
        public string BeforeRackNumber { get; set; }

        [NotMapped]
        [DisplayName("得意先")]
        public string Client
        {
            get
            {
                return String.Format(CodeNameFormat, this.ClientCode, this.ClientName);
            }

        }

        [NotMapped]
        [DisplayName("得意先コード")]
        public string ClientCode { get; set; }

        [NotMapped]
        [DisplayName("得意先名")]
        public string ClientName { get; set; }

        [NotMapped]
        [DisplayName("品名")]
        public string Product
        {
            get
            {
                return String.Format(CodeNameFormat, this.ProductCode, this.ProductName);
            }

        }

        [NotMapped]
        [DisplayName("品名コード")]
        public string ProductCode { get; set; }

        [NotMapped]
        [DisplayName("品名")]
        public string ProductName { get; set; }

        [NotMapped]
        [DisplayName("規格")]
        public string Standards { get; set; }
        [NotMapped]
        [DisplayName("単位")]
        public string Unit { get; set; }


        [NotMapped]
        [DisplayName("前月在庫数")]
        public int PreviousMonthRemaining { get; set; }

        [NotMapped]
        [DisplayName("当月在庫数")]
        public int PresentMonthRemaining { get; set; }

        [NotMapped]
        [DisplayName("当月入庫数")]
        public int PresentMonthInQuantity { get; set; }

        [NotMapped]
        [DisplayName("当月出庫数")]
        public int PresentMonthOutQuantity { get; set; }

        [NotMapped]
        [DisplayName("在庫数")]
        public int StockQuantity { get; set; }

        [NotMapped]
        [DisplayName("引当数")]
        public int AllocationQuantity { get; set; }

        [NotMapped]
        [DisplayName("出庫指示数")]
        public int OutOrderQuantity { get; set; }

        [NotMapped]
        [DisplayName("計画引当数")]
        public int AllocationPlanQuantity { get; set; }

        [NotMapped]
        [DisplayName("計画実施済引当数")]
        public int AllocationExecQuantity { get; set; }

        [NotMapped]
        [DisplayName("確定済引当数")]
        public int FixAllocationQuantity
        {
            get
            {
                return AllocationQuantity;
            }
        }

        [NotMapped]
        [DisplayName("在庫残数")]
        public int StockRemaining
        {
            get
            {
                return this.StockQuantity - this.AllocationQuantity - this.OutOrderQuantity;
            }
        }

        [NotMapped]
        [DisplayName("確定済在庫残数")]
        public int FixStockRemaining
        {
            get
            {
                return this.StockQuantity - this.AllocationQuantity;
            }
        }


        [NotMapped]
        [DisplayName("ロットNo.")]
        public string LotNumber { get; set; }

        [NotMapped]
        [DisplayName("ロットNo.計")]
        public int LotNumberQuantity { get; set; }

        [NotMapped]
        [DisplayName("伝票区分")]
        public string SlipType { get; set; }

        [NotMapped]
        [DisplayName("伝票No.")]
        public string SlipNumber { get; set; }

        [NotMapped]
        [DisplayName("移動年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime MovedAt { get; set; }

        [NotMapped]
        [DisplayName("移動数")]
        public int MovedQuantity { get; set; }

        [NotMapped]
        [DisplayName("保存期限")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM}", ApplyFormatInEditMode = true)]
        public DateTime? StockLimitYM { get; set; }

        [NotMapped]
        [DisplayName("集約コード")]
        public string AggregateCode { get; set; }

        [NotMapped]
        [DisplayName("ピッキングリスト")]
        public PickingListOutputStatus PickingListOutput { get; set; }

        [NotMapped]
        [DisplayName("ピッキングリスト")]
        public string PickingList
        {
            get
            {
                var str = String.Empty;
                switch (this.PickingListOutput)
                {
                    case PickingListOutputStatus.Nothing:
                        str = "対象外";
                        break;
                    case PickingListOutputStatus.NoOutput:
                        str = "未出力";
                        break;
                    case PickingListOutputStatus.Output:
                        str = "出力済";
                        break;
                    default:
                        break;
                }
                return str;
            }
        }

    }
}
