using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{
    [Table("m_hokan_rireki_tanka")]
    public class MHokanRirekiTanka
    {

        [Description("実施年月")]
        [Column("MONTH", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Month { get; set; }

        [Description("品番コード")]
        [Column("HINCOD", Order = 1)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Hincod { get; set; }

        [Description("場所")]
        [Column("SYBCOD")]
        [MaxLength(2)]
        public string Sybcod { get; set; }

        [Description("品名型式")]
        [Column("HINNMK")]
        [MaxLength(20)]
        public string Hinnmk { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA")]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("機種Ｂ")]
        [Column("KISYUB")]
        [MaxLength(6)]
        public string Kisyub { get; set; }

        [Description("単価設定フラグ")]
        [Column("TFULAG")]
        [MaxLength(1)]
        public string Tfulag { get; set; }

        [Description("振替区分")]
        [Column("FRIKAE")]
        [MaxLength(1)]
        public string Frikae { get; set; }

        [Description("従価率")]
        [Column("JYUKAR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Jyukar { get; set; }

        [Description("収受率")]
        [Column("SYUJYR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Syujyr { get; set; }

        [Description("ＦＰ単価")]
        [Column("FPTANK")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 2)]
        public decimal? Fptank { get; set; }

        [Description("保管単価")]
        [Column("HOKANT")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 5)]
        public decimal? Hokant { get; set; }

        [Description("保管単価設定日")]
        [Column("HOKYMD")]
        public DateTime? Hokymd { get; set; }

        [Description("保管単価設定担当")]
        [Column("HOKTAN")]
        [MaxLength(8)]
        public string Hoktan { get; set; }

        [Description("荷役単価")]
        [Column("NIEKIT")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 5)]
        public decimal? Niekit { get; set; }

        [Description("荷役単価設定日")]
        [Column("NIEYMD")]
        public DateTime? Nieymd { get; set; }

        [Description("荷役単価設定担当")]
        [Column("NIETAN")]
        [MaxLength(8)]
        public string Nietan { get; set; }

        [Description("旧場所")]
        [Column("OSYBCOD")]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Osybcod { get; set; }

        [Description("旧品名型式")]
        [Column("OHINNMK")]
        [MaxLength(20)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Ohinnmk { get; set; }

        [Description("旧機種Ａ")]
        [Column("OKISYUA")]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Okisyua { get; set; }

        [Description("旧機種Ｂ")]
        [Column("OKISYUB")]
        [MaxLength(6)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Okisyub { get; set; }

        [Description("旧振替区分")]
        [Column("OFRIKAE")]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Ofrikae { get; set; }

        [Description("旧ＦＰ単価")]
        [Column("OFPTNK")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 2)]
        public decimal? Ofptnk { get; set; }

        [Description("旧保管単価")]
        [Column("OHOKAT")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 5)]
        public decimal? Ohokat { get; set; }

        [Description("旧荷役単価")]
        [Column("ONIEKT")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 5)]
        public decimal? Oniekt { get; set; }

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
