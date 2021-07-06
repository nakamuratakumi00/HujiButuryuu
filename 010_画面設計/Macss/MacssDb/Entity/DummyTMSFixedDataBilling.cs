using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_tmsfixeddatabilling")]
    public class DummyTMSFixedDataBilling : DummyTMSFixedData
    {
        [NotMapped]
        [DisplayName("顧客コード")]
        public string CustomerCode { get; set; }

        [NotMapped]
        [DisplayName("顧客名")]
        public string CustomerName { get; set; }

        [NotMapped]
        [DisplayName("業務グループNo.")]
        public string OperationGroupNumber { get; set; }

        [NotMapped]
        [DisplayName("顧客グループNo.")]
        public string CustomerGroupNumber { get; set; }

        [NotMapped]
        [DisplayName("売上No.")]
        public string SaleNumber { get; set; }

        [NotMapped]
        [DisplayName("処理日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ProcessYMD { get; set; }

        [NotMapped]
        [DisplayName("PCコード1（部門コード）")]
        public string PcCode1 { get; set; }

        [NotMapped]
        [DisplayName("PCコード2（事業区分コード）")]
        public string PcCode2 { get; set; }

        [NotMapped]
        [DisplayName("PCコード3（請求グループコード）")]
        public string PcCode3 { get; set; }

        [NotMapped]
        [DisplayName("出力区分")]
        public string OutputCategory { get; set; }

        [NotMapped]
        [DisplayName("科目コード")]
        public string AccountItemCode { get; set; }

        [NotMapped]
        [DisplayName("税区分")]
        public string TaxCategory { get; set; }

        [NotMapped]
        [DisplayName("税率")]
        public decimal TaxRate { get; set; }

        [NotMapped]
        [DisplayName("摘要コード")]
        public string AccountTitleCode { get; set; }

        [NotMapped]
        [DisplayName("摘要名")]
        public string AccountTitleName { get; set; }

        [NotMapped]
        [DisplayName("金額")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [NotMapped]
        [DisplayName("外税金額")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public decimal AmountExcludeTax { get; set; }

        [NotMapped]
        [DisplayName("締日")]
        public int AccountingDeadline { get; set; }

        [NotMapped]
        [DisplayName("売外税合計金額（端数処理済）")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public decimal TotalAmountExcludeTax { get; set; }

        [NotMapped]
        [DisplayName("端数処理金額")]
        [DisplayFormat(DataFormatString = "{0:#,#}", ApplyFormatInEditMode = true)]
        public decimal RoundedAmount { get; set; }
    }
}
