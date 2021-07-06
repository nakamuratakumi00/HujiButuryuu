using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_dossdata_for_kitakanto")]
    public class DummyDossDataForKitaKanto : Common
    {

        [NotMapped]
        [DisplayName("梱包実績日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime PackingYMD { get; set; }

        [NotMapped]
        [DisplayName("運送名称")]
        public string ShippingName { get; set; }

        [NotMapped]
        [DisplayName("運送コード")]
        public string ShippingCode { get; set; }

        [NotMapped]
        [DisplayName("FBNO")]
        public string FBNO { get; set; }

        [NotMapped]
        [DisplayName("送状番号")]
        public string InvoiceNumber { get; set; }

        [NotMapped]
        [DisplayName("配達個数")]
        public int DeliverQuantity { get; set; }

        [NotMapped]
        [DisplayName("配達重量")]
        public int DeliverWeight { get; set; }

        [NotMapped]
        [DisplayName("納期日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryYMD { get; set; }

        [NotMapped]
        [DisplayName("届先郵便番号")]
        public string DestinationZipCode { get; set; }

        [NotMapped]
        [DisplayName("届先電話番号")]
        public string DestinationPhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("届先コード")]
        public string DestinationCode { get; set; }

        [NotMapped]
        [DisplayName("届先名称")]
        public string DestinationName { get; set; }

        [NotMapped]
        [DisplayName("届先住所")]
        public string DestinationAddress { get; set; }

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
        [DisplayName("データ登録日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DataRegistrationDate { get; set; }

        [NotMapped]
        [DisplayName("対象期間開始")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime PeriodStart { get; set; }

        [NotMapped]
        [DisplayName("対象期間終了")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime PeriodEnd { get; set; }
    }
}
