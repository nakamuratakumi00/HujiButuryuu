using Sic.Common.Attribute;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_tmsfixeddata")]
    public class DummyTMSFixedData : Common
    {
        protected string CodeAndNameDisplayFormat = "{0}） {1}";

        protected string GetCodeAndNameDisplayString(string code, string name)
        {
            if (String.IsNullOrWhiteSpace(code))
            {
                return name;
            }
            return String.Format(CodeAndNameDisplayFormat, code, name);
        }

        public enum DataTypes
        {
            [Value("作業依頼")]
            Request = 0,
            [Value("請求")]
            Billing = 1,
            [Value("支払")]
            Payment = 2,
        }

        [NotMapped]
        [DisplayName("区分")]
        public string Category { get; set; }

        [NotMapped]
        [DisplayName("取込")]
        public string Imported { get; set; }

        [NotMapped]
        [DisplayName("No.")]
        public int ImportedIndex { get; set; }

        /// <summary>
        /// LUDB610
        /// </summary>
        [NotMapped]
        [DisplayName("メッセージ")]
        public string UploadErrorMessage { get; set; }

        [NotMapped]
        [DisplayName("取込日時")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ImportedAt { get; set; }

        [NotMapped]
        [DisplayName("ファイル更新日時")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime FileUpdatedAt { get; set; }

        [NotMapped]
        [DisplayName("取込フラグ")]
        public DataTypes DataType { get; set; }

        [NotMapped]
        [DisplayName("エラーコード")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// LUDB612
        /// </summary>
        [NotMapped]
        [DisplayName("エラーメッセージ")]
        public string ImportedErrorMessage { get; set; }

        [NotMapped]
        [DisplayName("エラー情報")]
        public string Error
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.ErrorCode, this.ImportedErrorMessage);
            }
        }

        [NotMapped]
        [DisplayName("受付No.")]
        public string AcceptanceNumber { get; set; }

        [NotMapped]
        [DisplayName("受付日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime AcceptanceYMD { get; set; }

        [NotMapped]
        [DisplayName("発注No.")]
        public string OrderNumber { get; set; }

        [NotMapped]
        [DisplayName("発注日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderYMD { get; set; }

        [NotMapped]
        [DisplayName("顧客伝票No.")]
        public string CustomerSlipNumber { get; set; }

        [NotMapped]
        [DisplayName("請求計上状態1")]
        public string BillingRecordedStatus1
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.BillingRecordedStatusCode1, this.BillingRecordedStatusName1);
            }
        }

        [NotMapped]
        [DisplayName("請求計上状態1")]
        public string BillingRecordedStatusCode1 { get; set; }

        [NotMapped]
        [DisplayName("請求計上状態名1")]
        public string BillingRecordedStatusName1 { get; set; }

        [NotMapped]
        [DisplayName("請求計上状態2")]
        public string BillingRecordedStatus2
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.BillingRecordedStatusCode2, this.BillingRecordedStatusName2);
            }
        }

        [NotMapped]
        [DisplayName("請求計上状態2")]
        public string BillingRecordedStatusCode2 { get; set; }

        [NotMapped]
        [DisplayName("請求計上状態名2")]
        public string BillingRecordedStatusName2 { get; set; }

        [NotMapped]
        [DisplayName("支払計上状態1")]
        public string PaymentRecordedStatus1
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.PaymentRecordedStatusCode1, this.PaymentRecordedStatusName1);
            }
        }

        [NotMapped]
        [DisplayName("支払計上状態1")]
        public string PaymentRecordedStatusCode1 { get; set; }

        [NotMapped]
        [DisplayName("支払計上状態名1")]
        public string PaymentRecordedStatusName1 { get; set; }

        [NotMapped]
        [DisplayName("支払計上状態2")]
        public string PaymentRecordedStatus2
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.PaymentRecordedStatusCode2, this.PaymentRecordedStatusName2);
            }
        }

        [NotMapped]
        [DisplayName("支払計上状態2")]
        public string PaymentRecordedStatusCode2 { get; set; }

        [NotMapped]
        [DisplayName("支払計上状態名2")]
        public string PaymentRecordedStatusName2 { get; set; }

        [NotMapped]
        [DisplayName("請求距離")]
        public decimal BillingDistance { get; set; }

        [NotMapped]
        [DisplayName("支払距離")]
        public decimal PaymentDistance { get; set; }
    }
}
