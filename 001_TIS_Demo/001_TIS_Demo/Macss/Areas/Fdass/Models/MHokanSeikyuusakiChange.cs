using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{

    [Table("m_hokan_seikyuusaki_change")]
    public class MHokanSeikyuusakiChange
    {

        [Description("請求先コード")]
        [Column("SEICOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(9)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Seicod { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA", Order = 1)]
        [Required]
        [Key]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Kisyua { get; set; }

        [Description("変更先コード")]
        [Column("CHGCOD")]
        [MaxLength(9)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
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