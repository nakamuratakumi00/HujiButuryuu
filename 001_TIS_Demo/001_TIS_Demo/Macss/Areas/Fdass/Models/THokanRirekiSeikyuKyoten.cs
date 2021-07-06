using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{

    [Table("t_hokan_rireki_seikyu_kyoten")]
    public class THokanRirekiSeikyuKyoten
    {

        [Description("実施年月")]
        [Column("MONTH", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Month { get; set; }

        [Description("場所")]
        [Column("BASYO", Order = 1)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Basyo { get; set; }

        [Description("場所名")]
        [Column("BASNAM")]
        [MaxLength(20)]
        public string Basnam { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA", Order = 2)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("機種Ｂ")]
        [Column("KISYUB", Order = 3)]
        [Required]
        [Key]
        [MaxLength(3)]
        public string Kisyub { get; set; }

        [Description("機種Ｂ（日本語）")]
        [Column("KISYBN")]
        [MaxLength(3)]
        public string Kisybn { get; set; }

        [Description("請求先コード")]
        [Column("SEICOD")]
        [MaxLength(9)]
        public string Seicod { get; set; }

        [Description("前月残数")]
        [Column("ZANSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Zansuu { get; set; }

        [Description("前月残金額")]
        [Column("ZANKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Zankin { get; set; }

        [Description("仕込倉入数")]
        [Column("SIKSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Siksuu { get; set; }

        [Description("仕込倉入金額")]
        [Column("SIKKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Sikkin { get; set; }

        [Description("改造倉入数")]
        [Column("KAISUU")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Kaisuu { get; set; }

        [Description("改造倉入金額")]
        [Column("KAIKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Kaikin { get; set; }

        [Description("入庫数")]
        [Column("NYUKSU")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Nyuksu { get; set; }

        [Description("入庫金額")]
        [Column("NYUKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Nyukin { get; set; }

        [Description("出庫数")]
        [Column("SYKSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Syksuu { get; set; }

        [Description("出庫金額")]
        [Column("SYKKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Sykkin { get; set; }

        [Description("在庫数")]
        [Column("ZAIKSU")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Zaiksu { get; set; }

        [Description("在庫金額")]
        [Column("ZAIKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(12, 0)]
        public decimal? Zaikin { get; set; }

        [Description("伝票件数")]
        [Column("DENSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(6, 0)]
        public decimal? Densuu { get; set; }

        [Description("伝票件数（休日）")]
        [Column("DENSKY")]
        [CustomAttributes.DecimalPrecisionAttribute(6, 0)]
        public decimal? Densky { get; set; }

        [Description("保管料")]
        [Column("HOKANK")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Hokank { get; set; }

        [Description("荷役料")]
        [Column("NIEKIK")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Niekik { get; set; }

        [Description("荷役料（休日）")]
        [Column("NIEKYJ")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Niekyj { get; set; }

        [Description("ＰＣコード保管")]
        [Column("PCCODH")]
        [MaxLength(12)]
        public string Pccodh { get; set; }

        [Description("ＰＣコード荷役")]
        [Column("PCCODN")]
        [MaxLength(12)]
        public string Pccodn { get; set; }

        [Description("年月")]
        [Column("DATAYM")]
        [MaxLength(4)]
        public string Dataym { get; set; }

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
