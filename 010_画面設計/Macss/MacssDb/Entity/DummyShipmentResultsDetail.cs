using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_shipmentresultsdetail")]
    public class DummyShipmentResultsDetail : Common
    {
        [NotMapped]

        public long ShipmentResultsId { get; set; }

        [NotMapped]
        [DisplayName("運送会社送状番号")]
        public string CarrierInvoiceNumber { get; set; }

        [NotMapped]
        [DisplayName("品名")]
        public string ProductName { get; set; }

        [NotMapped]
        [DisplayName("宅配出荷サイズ")]
        public decimal ShipmentSize { get; set; }

        [NotMapped]
        [DisplayName("出荷個数")]
        public decimal NumberOfShipments { get; set; }

        [NotMapped]
        [DisplayName("出荷ケース数")]
        public decimal NumberOfShipmentCases { get; set; }

    }
}
