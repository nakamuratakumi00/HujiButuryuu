using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_shippingCost")]
    public class DummyShippingCost : Common
    {
        /// <summary>
        /// 発店No.
        /// </summary>
        [NotMapped]
        [DisplayName("協力会社ID")]
        public string PartnerID
        {
            get
            {
                return this.Supplier;
            }
        }

        [NotMapped]
        [DisplayName("仕入先")]
        public string Supplier { get; set; }

        /// <summary>
        /// 荷主コード
        /// </summary>
        [NotMapped]
        [DisplayName("出荷No.")]
        public string ShipmentNumber { get; set; }

        /// <summary>
        /// 路線区分コード
        /// </summary>
        [NotMapped]
        [DisplayName("路線区分")]
        public string RouteCategory
        {
            get
            {
                return String.Format("{0} {1}", this.Supplier, this.TransportationCategory);
            }
        }


        /// <summary>
        /// T運送区分
        /// </summary>
        [NotMapped]
        [DisplayName("運送区分")]
        public string TransportationCategory { get; set; }

        /// <summary>
        /// T発送月日
        /// </summary>
        [NotMapped]
        [DisplayName("出荷日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ShipmentYMD { get; set; }

        /// <summary>
        /// T送り状No.
        /// </summary>
        [NotMapped]
        [DisplayName("仕入先原票No.")]
        public string SupplierSlipNumber { get; set; }

        [NotMapped]
        [DisplayName("納入先住所")]
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// 納入先名
        /// </summary>
        [NotMapped]
        [DisplayName("得意先")]
        public string Client { get; set; }

        /// <summary>
        /// T個数
        /// </summary>
        [NotMapped]
        [DisplayName("包装数")]
        public int PackingQuantity { get; set; }

        /// <summary>
        /// T重量
        /// </summary>
        [NotMapped]
        [DisplayName("重量")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public decimal Weight { get; set; }

        /// <summary>
        /// T税込合計
        /// </summary>
        [NotMapped]
        [DisplayName("税込合計")]
        public decimal TotalIncludeTax
        {
            get
            {
                return
                this.CostExcludeTax + this.Insurance + this.Expense + this.RelayExpense + this.Tax;
            }
        }

        [NotMapped]
        [DisplayName("運賃")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public decimal ShipingCost
        {
            get
            {
                return this.TotalIncludeTax;
            }
        }

        [NotMapped]
        [DisplayName("税区分")]
        public string TaxCategory { get; set; }

        /// <summary>
        /// T運賃
        /// </summary>
        [NotMapped]
        [DisplayName("運賃")]
        public decimal CostExcludeTax { get; set; }

        /// <summary>
        /// T保険料
        /// </summary>
        [NotMapped]
        [DisplayName("保険料")]
        public decimal Insurance { get; set; }

        /// <summary>
        /// T諸料金
        /// </summary>
        [NotMapped]
        [DisplayName("諸料金")]
        public decimal Expense { get; set; }

        /// <summary>
        /// T中継料
        /// </summary>
        [NotMapped]
        [DisplayName("中継料")]
        public decimal RelayExpense { get; set; }

        /// <summary>
        /// T消費税
        /// </summary>
        [NotMapped]
        [DisplayName("消費税")]
        public decimal Tax
        {
            get
            {
                return ((this.CostExcludeTax + this.Insurance + this.Expense + this.RelayExpense) * ((decimal)0.1));
            }
        }

        [NotMapped]
        [DisplayName("出荷場所")]
        public string OutLocation { get; set; }

        [NotMapped]
        [DisplayName("登録担当")]
        public string RegistrationUser { get; set; }

        [NotMapped]
        [DisplayName("登録WS名")]
        public string RegistrationWSName { get; set; }

        [NotMapped]
        [DisplayName("エラーメッセージ")]
        public string ErrorMessage { get; set; }
    }
}
