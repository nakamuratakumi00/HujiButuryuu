using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    /// <summary>
    /// 表示用のテーブルクラス
    /// </summary>
    [Table("dummy_t_dossdata")]
    public class DummyDossData : Common
    {
        public enum DossDataTypes
        {
            Nothing = 0,
            KitaKanto = 1,
            All = 2,
        }

        [NotMapped]
        [DisplayName("データ区分")]
        public DossDataTypes DossDataType { get; set; }

        [NotMapped]
        [DisplayName("データ区分")]
        public string DossDataTypeString
        {
            get
            {
                var str = String.Empty;
                switch (this.DossDataType)
                {
                    case DossDataTypes.Nothing:
                        str = "-";
                        break;
                    case DossDataTypes.KitaKanto:
                        str = "北関東支社向け";
                        break;
                    case DossDataTypes.All:
                        str = "全社基幹向け";
                        break;
                    default:
                        break;
                }
                return str;
            }
        }

        [NotMapped]
        [DisplayName("作成年月日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DataCreateYMD { get; set; }

        [NotMapped]
        [DisplayName("出荷場所")]
        public string LocationCode { get; set; }

        [NotMapped]
        [DisplayName("出荷場所")]
        public string Location { get; set; }

        [NotMapped]
        [DisplayName("ファイル名")]
        public string FileName { get; set; }

        [NotMapped]
        [DisplayName("メール送信")]
        public bool? MailSend { get; set; }

        [NotMapped]
        [DisplayName("メール送信")]
        public string MailSendString
        {
            get
            {
                var str = "-";
                if (this.MailSend.HasValue)
                {
                    if (this.MailSend.Value)
                    {
                        str = "送信済み";
                    }
                    else
                    {
                        str = "未送信";
                    }
                }
                return str;
            }
        }

        [NotMapped]
        [DisplayName("出荷日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime OutYMD { get; set; }

        [NotMapped]
        [DisplayName("対象ルート")]
        public string Route { get; set; }

        [NotMapped]
        [DisplayName("作成対象")]
        public string TargetDossData { get; set; }

        [NotMapped]
        [DisplayName("北関東支社向け")]
        public bool DossDataKitaKanto { get; set; }

        [NotMapped]
        [DisplayName("全社基幹向け")]
        public bool DossDataAll { get; set; }

        [NotMapped]
        [DisplayName("作成種別")]
        public string CreateType { get; set; }

    }
}
