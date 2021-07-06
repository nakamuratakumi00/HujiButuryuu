using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    [Table("dummy_t_tmsfixeddatarequest")]
    public class DummyTMSFixedDataRequest : DummyTMSFixedData
    {
        [NotMapped]
        [DisplayName("レコード識別区分")]
        public string RecordIdCategory { get; set; }

        [NotMapped]
        [DisplayName("顧客")]
        public string Customer
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.CustomerCode, this.CustomerName);
            }
        }
        [NotMapped]
        [DisplayName("顧客コード")]
        public string CustomerCode { get; set; }

        [NotMapped]
        [DisplayName("顧客名")]
        public string CustomerName { get; set; }

        [NotMapped]
        [DisplayName("協力会社")]
        public string Partner
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.PartnerCode, this.PartnerName);
            }
        }

        [NotMapped]
        [DisplayName("協力会社コード")]
        public string PartnerCode { get; set; }

        [NotMapped]
        [DisplayName("協力会社名")]
        public string PartnerName { get; set; }

        [NotMapped]
        [DisplayName("進捗状況（作業依頼）")]
        public string Progress
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.ProgressCode, this.ProgressName);
            }
        }

        [NotMapped]
        [DisplayName("進捗状況（作業依頼）")]
        public string ProgressCode { get; set; }

        [NotMapped]
        [DisplayName("進捗状況名（作業依頼）")]
        public string ProgressName { get; set; }

        [NotMapped]
        [DisplayName("業務")]
        public string Operation
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.OperationNumber, this.OperationName);
            }
        }

        [NotMapped]
        [DisplayName("業務No.")]
        public string OperationNumber { get; set; }

        [NotMapped]
        [DisplayName("業務名")]
        public string OperationName { get; set; }

        [NotMapped]
        [DisplayName("輸送モード（作業依頼）")]
        public string TransportationModeForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("運賃種別（作業依頼）")]
        public string ShipingCostTypeForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("運賃（作業依頼）")]
        public decimal? ShipingCostForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("貨物区分")]
        public string ShipmentCategory
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.ShipmentCategoryCode, this.ShipmentCategoryName);
            }
        }
        [NotMapped]
        [DisplayName("貨物区分コード")]
        public string ShipmentCategoryCode { get; set; }

        [NotMapped]
        [DisplayName("貨物区分名")]
        public string ShipmentCategoryName { get; set; }

        [NotMapped]
        [DisplayName("貨物荷姿コード")]
        public string ShipmentPackingStyleCode { get; set; }

        [NotMapped]
        [DisplayName("貨物荷姿名")]
        public string ShipmentPackingStyleName { get; set; }

        [NotMapped]
        [DisplayName("貨物重量")]
        public decimal ShipmentWeight { get; set; }

        [NotMapped]
        [DisplayName("貨物容積")]
        public long? ShipmentBulk { get; set; }

        [NotMapped]
        [DisplayName("作業員時間区分")]
        public string StaffTimeCategory { get; set; }

        [NotMapped]
        [DisplayName("作業員数")]
        public long? NumberOfStaff { get; set; }

        [NotMapped]
        [DisplayName("備考（作業依頼）")]
        public string NoteForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("車種（作業依頼）")]
        public string VehicleClassForWorkRequest
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.VehicleClassCodeForWorkRequest, this.VehicleClassNameForWorkRequest);
            }
        }

        [NotMapped]
        [DisplayName("車種コード（作業依頼）")]
        public string VehicleClassCodeForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("車種名（作業依頼）")]
        public string VehicleClassNameForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("付帯設備（作業依頼）")]
        public string EquipmentsForWorkRequest
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.EquipmentsCodeForWorkRequest, this.EquipmentsNameForWorkRequest);
            }
        }

        [NotMapped]
        [DisplayName("付帯設備コード（作業依頼）")]
        public string EquipmentsCodeForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("付帯設備名（作業依頼）")]
        public string EquipmentsNameForWorkRequest { get; set; }

        [NotMapped]
        [DisplayName("送り状No.")]
        public string InvoiceNumber { get; set; }

        [NotMapped]
        [DisplayName("数量")]
        public long Quantity { get; set; }

        [NotMapped]
        [DisplayName("時間指定")]
        public string TimeAppointed { get; set; }

        [NotMapped]
        [DisplayName("品名")]
        public string ProductName { get; set; }

        [NotMapped]
        [DisplayName("箱数")]
        public int PackageQuantity { get; set; }

        [NotMapped]
        [DisplayName("顧客貨物サイズ")]
        public string ClientShipmentSize
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.ClientShipmentSizeCode, this.ClientShipmentSizeName);
            }
        }

        [NotMapped]
        [DisplayName("顧客貨物サイズ")]
        public string ClientShipmentSizeCode { get; set; }

        [NotMapped]
        [DisplayName("顧客貨物サイズ名")]
        public string ClientShipmentSizeName { get; set; }

        [NotMapped]
        [DisplayName("協力会社貨物サイズ")]
        public string PartnerShipmentSize
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.PartnerShipmentSizeCode, this.PartnerShipmentSizeName);
            }
        }

        [NotMapped]
        [DisplayName("協力会社貨物サイズ")]
        public string PartnerShipmentSizeCode { get; set; }

        [NotMapped]
        [DisplayName("協力会社貨物サイズ名")]
        public string PartnerShipmentSizeName { get; set; }

        [NotMapped]
        [DisplayName("車種（発注）")]
        public string VehicleClassForOrder
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.VehicleClassCodeForOrder, this.VehicleClassNameForOrder);
            }
        }

        [NotMapped]
        [DisplayName("車種コード（発注）")]
        public string VehicleClassCodeForOrder { get; set; }

        [NotMapped]
        [DisplayName("車種名（発注）")]
        public string VehicleClassNameForOrder { get; set; }

        [NotMapped]
        [DisplayName("付帯設備（発注）")]
        public string EquipmentsForOrder
        {
            get
            {
                return GetCodeAndNameDisplayString(
                    this.EquipmentsCodeForOrder, this.EquipmentsNameForOrder);
            }
        }

        [NotMapped]
        [DisplayName("付帯設備コード（発注）")]
        public string EquipmentsCodeForOrder { get; set; }

        [NotMapped]
        [DisplayName("付帯設備名（発注）")]
        public string EquipmentsNameForOrder { get; set; }

        [NotMapped]
        [DisplayName("車番")]
        public string VehicleNumber { get; set; }

        [NotMapped]
        [DisplayName("ドライバー")]
        public string Driver { get; set; }

        [NotMapped]
        [DisplayName("ドライバー連絡先")]
        public string DriverContactInformation { get; set; }

        [NotMapped]
        [DisplayName("輸送モード（発注）")]
        public string TransportationModeForOrder { get; set; }

        [NotMapped]
        [DisplayName("運賃種別（車両発注）")]
        public string ShipingCostTypeForOrder { get; set; }

        [NotMapped]
        [DisplayName("運賃（車両発注）")]
        public decimal? ShipingCostForOrder { get; set; }

        [NotMapped]
        [DisplayName("積日時")]
        public string LoadingAt
        {
            get
            {
                return this.LoadingAtFrom.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("積地 日時（From）")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime LoadingAtFrom { get; set; }

        [NotMapped]
        [DisplayName("積地 日時（To）")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? LoadingAtTo { get; set; }


        [NotMapped]
        [DisplayName("積日")]
        public string LoadingYMDFrom
        {
            get
            {
                return this.LoadingAtFrom.ToString("yyyy/MM/dd");
            }
        }

        [NotMapped]
        [DisplayName("積地 時間（From）")]
        public string LoadingTimeFrom
        {
            get
            {
                return this.LoadingAtFrom.ToString("HH:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("積日")]
        public string LoadingYMDTo
        {
            get
            {
                if (!this.LoadingAtTo.HasValue)
                {
                    return String.Empty;
                }
                return this.LoadingAtTo.Value.ToString("yyyy/MM/dd");
            }
        }

        [NotMapped]
        [DisplayName("積地 時間（To）")]
        public string LoadingTimeTo
        {
            get
            {
                if (!this.LoadingAtTo.HasValue)
                {
                    return String.Empty;
                }
                return this.LoadingAtTo.Value.ToString("HH:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("積地名")]
        public string LoadingName { get; set; }

        [NotMapped]
        [DisplayName("積地 所在地JISコード")]
        public string LoadingLocationJIS { get; set; }

        [NotMapped]
        [DisplayName("積地 郵便番号")]
        public string LoadingLocationZipCode { get; set; }

        [NotMapped]
        [DisplayName("積地 所在地")]
        public string LoadingLocation { get; set; }

        [NotMapped]
        [DisplayName("積地 所在地詳細")]
        public string LoadingLocationDetail { get; set; }

        [NotMapped]
        [DisplayName("積地 電話番号")]
        public string LoadingLocationPhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("積地 担当部署")]
        public string LoadingLocationDepartment { get; set; }

        [NotMapped]
        [DisplayName("積地 担当者")]
        public string LoadingLocationStaff { get; set; }

        [NotMapped]
        [DisplayName("積地 備考")]
        public string LoadingNote { get; set; }

        [NotMapped]
        [DisplayName("積地 自拠点フラグ")]
        public string LoadingSiteFlag { get; set; }

        [NotMapped]
        [DisplayName("積地区分")]
        public string LoadingCategory { get; set; }

        [NotMapped]
        [DisplayName("積地コード")]
        public string LoadingCode { get; set; }

        [NotMapped]
        [DisplayName("降日時")]
        public string UnloadingAt
        {
            get
            {
                return this.UnloadingAtTo.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        [NotMapped]
        [DisplayName("降地 日時（From）")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? UnloadingAtFrom { get; set; }

        [NotMapped]
        [DisplayName("降地 日時（To）")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime UnloadingAtTo { get; set; }

        [NotMapped]
        [DisplayName("降日")]
        public string UnloadingYMDFrom
        {
            get
            {
                if (!this.UnloadingAtFrom.HasValue)
                {
                    return String.Empty;
                }
                return this.UnloadingAtFrom.Value.ToString("yyyy/MM/dd");
            }
        }

        [NotMapped]
        [DisplayName("降地 時間（From）")]
        public string UnloadingTimeFrom
        {
            get
            {
                if (!this.UnloadingAtFrom.HasValue)
                {
                    return String.Empty;
                }
                return this.UnloadingAtFrom.Value.ToString("HH:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("降日")]
        public string UnloadingYMDTo
        {
            get
            {
                return this.UnloadingAtTo.ToString("yyyy/MM/dd");
            }
        }

        [NotMapped]
        [DisplayName("降地 時間（To）")]
        public string UnloadingTimeTo
        {
            get
            {
                return this.UnloadingAtTo.ToString("HH:mm:ss");
            }
        }

        [NotMapped]
        [DisplayName("降地名")]
        public string UnloadingName { get; set; }

        [NotMapped]
        [DisplayName("降地 所在地JISコード")]
        public string UnloadingLocationJIS { get; set; }

        [NotMapped]
        [DisplayName("降地 郵便番号")]
        public string UnloadingLocationZipCode { get; set; }

        [NotMapped]
        [DisplayName("降地 所在地")]
        public string UnloadingLocation { get; set; }

        [NotMapped]
        [DisplayName("降地 所在地詳細")]
        public string UnloadingLocationDetail { get; set; }

        [NotMapped]
        [DisplayName("降地 電話番号")]
        public string UnloadingLocationPhoneNumber { get; set; }

        [NotMapped]
        [DisplayName("降地 担当部署")]
        public string UnloadingLocationDepartment { get; set; }

        [NotMapped]
        [DisplayName("降地 担当者")]
        public string UnloadingLocationStaff { get; set; }

        [NotMapped]
        [DisplayName("降地 備考")]
        public string UnloadingNote { get; set; }

        [NotMapped]
        [DisplayName("降地 自拠点フラグ")]
        public string UnloadingSiteFlag { get; set; }
    }
}
