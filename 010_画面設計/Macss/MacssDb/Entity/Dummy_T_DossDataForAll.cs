using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_dossdata_for_all")]
    public class DummyDossDataForAll : Common
    {

        [NotMapped]
        [DisplayName("出荷日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime OutYMD { get; set; }
        [NotMapped]
        [DisplayName("出荷場所")]
        public string Location { get; set; }

        [NotMapped]
        [DisplayName("出荷場所コード")]
        public string LocationCode { get; set; }

        [NotMapped]
        [DisplayName("出荷No.")]
        public string OutNumber { get; set; }

        [NotMapped]
        [DisplayName("基幹配送ルートコード")]
        public string RouteCode { get; set; }

        [NotMapped]
        [DisplayName("基幹配送ルート名")]
        public string RouteName { get; set; }

        [NotMapped]
        [DisplayName("配送車両コード")]
        public string VehicleCode { get; set; }

        [NotMapped]
        [DisplayName("基幹配送届先コード")]
        public string DestinationCode { get; set; }

        [NotMapped]
        [DisplayName("基幹配送届先名称")]
        public string DestinationName { get; set; }

        [NotMapped]
        [DisplayName("基幹配送届先住所")]
        public string DestinationAddress { get; set; }

        [NotMapped]
        [DisplayName("基幹配送届先郵便番号")]
        public string DestinationZipCode { get; set; }

        [NotMapped]
        [DisplayName("基幹配送届先電話番号")]
        public string DestinationPhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("品名")]
        public string ProductName { get; set; }

        [NotMapped]
        [DisplayName("出荷数")]
        public int OutQuantity { get; set; }

        [NotMapped]
        [DisplayName("問い合わせNo.")]
        public string InquiryNumber { get; set; }

        [NotMapped]
        [DisplayName("箱数")]
        public int PackageQuantity { get; set; }

        [NotMapped]
        [DisplayName("重量")]
        public int Weight { get; set; }

        [NotMapped]
        [DisplayName("得意先コード")]
        public string ClientCode { get; set; }

        [NotMapped]
        [DisplayName("得意先略称")]
        public string ClientShortName { get; set; }

        [NotMapped]
        [DisplayName("PC部門コード")]
        public string PcCode { get; set; }

        [NotMapped]
        [DisplayName("WMS届先コード")]
        public string WMSDestinationCode { get; set; }

        [NotMapped]
        [DisplayName("WMS届先名称")]
        public string WMSDestinationName { get; set; }

        [NotMapped]
        [DisplayName("WMS届先住所")]
        public string WMSDestinationAddress { get; set; }

        [NotMapped]
        [DisplayName("備考1")]
        public string Note1 { get; set; }

        [NotMapped]
        [DisplayName("備考2")]
        public string Note2 { get; set; }

        [NotMapped]
        [DisplayName("備考3")]
        public string Note3 { get; set; }

        [NotMapped]
        [DisplayName("納品日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliverYMD { get; set; }
    }
}
