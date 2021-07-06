using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{

    [Table("m_hokan_rireki_seikyuusaki_change")]
    public class MHokanRirekiSeikyuusakiChange
    {

        [Description("実施年月")]
        [Column("MONTH", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Month { get; set; }

        [Description("請求先コード")]
        [Column("SEICOD", Order = 1)]
        [Required]
        [Key]
        [MaxLength(9)]
        public string Seicod { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA", Order = 2)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("変更先コード")]
        [Column("CHGCOD")]
        [MaxLength(9)]
        public string Chgcod { get; set; }

        [Description("登録担当")]
        [Column("CRTCOD")]
        [MaxLength(8)]
        public string Crtcod { get; set; }

        [Description("登録日")]
        [Column("CRTYMD")]
        public DateTime? Crtymd { get; set; }

        [Description("更新担当")]
        [Column("UPDCOD")]
        [MaxLength(8)]
        public string Updcod { get; set; }

        [Description("更新日")]
        [Column("UPDYMD")]
        public DateTime? Updymd { get; set; }

    }
}
