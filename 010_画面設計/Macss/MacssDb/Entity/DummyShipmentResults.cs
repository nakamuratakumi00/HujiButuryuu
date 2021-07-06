using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_shipmentresults")]
    public class DummyShipmentResults : Common
    {
        public enum DataTypes
        {
            CS1 = 1,
            CS2 = 2,
            CS3 = 3,
        }

        public static string GetDataTypeName(DataTypes dataType)
        {
            switch (dataType)
            {
                case DataTypes.CS1:
                    //return "オフコン ⇒ TMS 連携";
                    return "TMS 連携";
                case DataTypes.CS2:
                    //return "オフコン ⇒ ACCESS 連携";
                    return "ACCESS 連携";
                case DataTypes.CS3:
                    //return "オフコン ⇒ エラーファイル";
                    return "エラーファイル";
                default:
                    return String.Empty;
            }

        }
        public enum CreateTypes
        {
            All = 0,
            ExistChargeList = 2,
            NoExistChargeList = 3,
        }

        public static string GetCreateTypeName(CreateTypes dataType)
        {
            switch (dataType)
            {
                case CreateTypes.All:
                    return "すべて";
                case CreateTypes.ExistChargeList:
                    return "TMS料金表あり のみ";
                case CreateTypes.NoExistChargeList:
                    return "TMS料金表なし のみ";
                default:
                    return String.Empty;
            }

        }
        [NotMapped]
        [DisplayName("出荷No.")]
        public string MainShipmentNumber { get; set; }

        [NotMapped]
        [DisplayName("出荷日８")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ShipmentYMD2 { get; set; }

        [NotMapped]
        [DisplayName("レコード種別")]
        public DataTypes DataType { get; set; }

        [NotMapped]
        [DisplayName("レコード種別")]
        public string DataTypeString
        {
            get
            {
                return GetDataTypeName(this.DataType);
            }
        }

        [NotMapped]
        [DisplayName("顧客")]
        public string Customer
        {
            get
            {
                return String.Format(CodeNameFormat, this.CustomerCode, this.CustomerName);
            }

        }
        [NotMapped]
        [DisplayName("顧客コード")]
        public string CustomerCode { get; set; }

        [NotMapped]
        [DisplayName("顧客名称")]
        public string CustomerName { get; set; }

        [NotMapped]
        [DisplayName("倉庫")]
        public string Warehouse
        {
            get
            {
                return String.Format(CodeNameFormat, this.WarehouseCode, this.WarehouseName);
            }

        }
        [NotMapped]
        [DisplayName("倉庫コード")]
        public string WarehouseCode { get; set; }

        [NotMapped]
        [DisplayName("倉庫名")]
        public string WarehouseName { get; set; }

        [NotMapped]
        [DisplayName("運送方法")]
        public string Transportation
        {
            get
            {
                return String.Format(CodeNameFormat, this.TransportationCode, this.TransportationName);
            }

        }

        [NotMapped]
        [DisplayName("運送方法コード")]
        public string TransportationCode { get; set; }

        [NotMapped]
        [DisplayName("運送方法名称")]
        public string TransportationName { get; set; }

        [NotMapped]
        [DisplayName("顧客伝票番号")]
        public string CustomerSlipNumber { get; set; }

        [NotMapped]
        [DisplayName("管理番号")]
        public string AccountingControlNumber { get; set; }

        [NotMapped]
        [DisplayName("出荷日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ShipmentYMD { get; set; }

        [NotMapped]
        [DisplayName("納期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryPeriod { get; set; }

        [NotMapped]
        [DisplayName("出荷総重量")]
        public decimal ShipmentTotalWeight { get; set; }

        [NotMapped]
        [DisplayName("出荷容積")]
        public long ShipmentBulk { get; set; }

        [NotMapped]
        [DisplayName("備考")]
        public string Note { get; set; }

        [NotMapped]
        [DisplayName("積地名称")]
        public string LoadingLocationName { get; set; }

        [NotMapped]
        [DisplayName("積地所在地")]
        public string LoadingLocationAddress { get; set; }

        [NotMapped]
        [DisplayName("積地所在地詳細")]
        public string LoadingLocationAddressDetail { get; set; }

        [NotMapped]
        [DisplayName("積地郵便番号")]
        public string LoadingLocationZipCode { get; set; }

        [NotMapped]
        [DisplayName("積地電話番号")]
        public string LoadingLocationPhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("積地担当部署")]
        public string LoadingLocationDepartment { get; set; }

        [NotMapped]
        [DisplayName("積地担当者")]
        public string LoadingLocationStaff { get; set; }

        [NotMapped]
        [DisplayName("積地備考")]
        public string LoadingLocationNote { get; set; }

        [NotMapped]
        [DisplayName("届先名称")]
        public string DestinationName { get; set; }

        [NotMapped]
        [DisplayName("届先所在地")]
        public string DestinationAddress { get; set; }

        [NotMapped]
        [DisplayName("届先所在地詳細")]
        public string DestinationAddressDetail { get; set; }

        [NotMapped]
        [DisplayName("届先郵便番号")]
        public string DestinationZipCode { get; set; }

        [NotMapped]
        [DisplayName("届先電話番号")]
        public string DestinationPhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("届先担当部署")]
        public string DestinationDepartment { get; set; }

        [NotMapped]
        [DisplayName("届先担当者")]
        public string DestinationStaff { get; set; }

        [NotMapped]
        [DisplayName("届先備考")]
        public string DestinationNote { get; set; }

        [NotMapped]
        [DisplayName("登録日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationYMD { get; set; }

        [NotMapped]
        [DisplayName("登録時間")]
        public string RegistrationTime
        {
            get
            {
                return this.RegistrationYMD.ToString("hh:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("登録日時")]
        public string RegistrationDateTime
        {
            get
            {
                return this.RegistrationYMD.ToString("yyyy/MM/dd hh:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("登録担当")]
        public string RegistrationUser { get; set; }

        [NotMapped]
        [DisplayName("登録WS名")]
        public string RegistrationWSName { get; set; }

        [NotMapped]
        [DisplayName("引渡日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryYMD { get; set; }

        [NotMapped]
        [DisplayName("引渡時間")]
        public string DeliveryTime
        {
            get
            {
                return this.DeliveryYMD.ToString("hh:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("引渡日時")]
        public string DeliveryDateTime
        {
            get
            {
                return this.DeliveryYMD.ToString("yyyy/MM/dd hh:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("受取日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReceivingYMD { get; set; }

        [NotMapped]
        [DisplayName("受取時間")]
        public string ReceivingTime
        {
            get
            {
                if (!this.ReceivingYMD.HasValue)
                {
                    return String.Empty;
                }
                return this.ReceivingYMD.Value.ToString("hh:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("受取日時")]
        public string ReceivingDateTime
        {
            get
            {
                if (!this.ReceivingYMD.HasValue)
                {
                    return String.Empty;
                }
                return this.ReceivingYMD.Value.ToString("yyyy/MM/dd hh:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("PC部門コード")]
        public string PcCode { get; set; }

        [NotMapped]
        [DisplayName("到着日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ArrivalYMD { get; set; }

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

        [NotMapped]
        public DummyShipmentResultsDetail[] ShipmentsDetail { get; set; }



        [NotMapped]
        [DisplayName("作成範囲")]
        public CreateTypes CreateType { get; set; }

        [NotMapped]
        [DisplayName("作成範囲")]
        public string CreateTypeString
        {
            get
            {
                return GetCreateTypeName(this.CreateType);
            }
        }

        [NotMapped]
        [DisplayName("エラーメッセージ")]
        public string ErrorMessage { get; set; }

        [NotMapped]
        [DisplayName("TMSファイル名")]
        public string TMSFileName { get; set; }

        [NotMapped]
        [DisplayName("請求先")]
        public string Billing { get; set; }

        [NotMapped]
        [DisplayName("TMS運送方法")]
        public string TMSTransportationCode
        {
            get
            {
                return this.TransportationCode;
            }
        }

        [NotMapped]
        [DisplayName("TMS顧客コード")]
        public string TMSCustomerCode
        {
            get
            {
                return this.CustomerCode;
            }
        }

        [NotMapped]
        [DisplayName("TMS協力会社")]
        public string TMSPartner { get; set; }

        [NotMapped]
        [DisplayName("登録担当者")]
        public string RegistrationUserName { get; set; }

        [NotMapped]
        [DisplayName("仕入先")]
        public string Supplier { get; set; }

        [NotMapped]
        [DisplayName("作成元")]
        public string CreatedBy { get; set; }
        [NotMapped]
        [DisplayName("出荷場所")]
        public string OutLocation
        {
            get
            {
                return this.WarehouseCode;
            }
        }
        [NotMapped]
        [DisplayName("運送方法")]
        public string TransportationType
        {
            get
            {
                if (String.IsNullOrEmpty(this.TransportationCode))
                {
                    return String.Empty;
                }
                return this.TransportationCode.Substring(0, 2);
            }
        }
        [NotMapped]
        [DisplayName("運送区分")]
        public string TransportationCategory { get; set; }
        [NotMapped]
        [DisplayName("伝票区分")]
        public string FormCategory { get; set; }
    }
}
