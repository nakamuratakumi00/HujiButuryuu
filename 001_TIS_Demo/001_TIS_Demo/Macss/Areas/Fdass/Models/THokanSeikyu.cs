using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{
    [Table("t_hokan_seikyu")]
    public class THokanSeikyu
    {

        [Description("請求先コード")]
        [Column("SEICOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(9)]
        public string Seicod { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA", Order = 1)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("機種Ｂ")]
        [Column("KISYUB", Order = 2)]
        [Required]
        [Key]
        [MaxLength(3)]
        public string Kisyub { get; set; }

        [Description("機種名")]
        [Column("KISNAM", Order = 3)]
        [Required]
        [Key]
        [MaxLength(15)]
        public string Kisnam { get; set; }

        [Description("品番コード")]
        [Column("HINCOD", Order = 4)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Hincod { get; set; }

        [Description("保管場所")]
        [Column("HOKCOD", Order = 5)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Hokcod { get; set; }

        [Description("PCコード")]
        [Column("PCCOD", Order = 6)]
        [Required]
        [Key]
        [MaxLength(12)]
        public string Pccod { get; set; }

        [Description("品名型式")]
        [Column("HINNMK")]
        [Required]
        [MaxLength(20)]
        public string Hinnmk { get; set; }

        [Description("前月残数")]
        [Column("ZANSUU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(10, 0)]
        public decimal? Zansuu { get; set; }

        [Description("入庫数")]
        [Column("NYUKSU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(10, 0)]
        public decimal? Nyuksu { get; set; }

        [Description("出庫数")]
        [Column("SYKSUU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(10, 0)]
        public decimal? Syksuu { get; set; }

        [Description("積数")]
        [Column("SEKISU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(10, 2)]
        public decimal? Sekisu { get; set; }

        [Description("保管単価")]
        [Column("HOKANT")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(10, 5)]
        public decimal? Hokant { get; set; }

        [Description("保管料")]
        [Column("HOKANK")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Hokank { get; set; }

        [Description("保管料（値引き後）")]
        [Column("HOKANKR")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Hokankr { get; set; }

        [Description("扱い数")]
        [Column("ATUKAI")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(10, 0)]
        public decimal? Atukai { get; set; }

        [Description("伝票件数")]
        [Column("DENSUU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(8, 0)]
        public decimal? Densuu { get; set; }

        [Description("荷役単価")]
        [Column("NIEKIT")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(9, 4)]
        public decimal? Niekit { get; set; }

        [Description("荷役料")]
        [Column("NIEKIK")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Niekik { get; set; }

        [Description("荷役料（値引き後）")]
        [Column("NIEKIKR")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Niekikr { get; set; }

        [Description("保管請求用フラグ１")]
        [Column("HOKFLG1")]
        [Required]
        [MaxLength(1)]
        public string Hokflg1 { get; set; }

        [Description("保管請求用フラグ２")]
        [Column("HOKFLG2")]
        [Required]
        [MaxLength(1)]
        public string Hokflg2 { get; set; }

        [Description("保管請求用フラグ３")]
        [Column("HOKFLG3")]
        [Required]
        [MaxLength(1)]
        public string Hokflg3 { get; set; }

        [Description("保管請求用フラグ４")]
        [Column("HOKFLG4")]
        [Required]
        [MaxLength(1)]
        public string Hokflg4 { get; set; }

        [Description("保管請求用フラグ５")]
        [Column("HOKFLG5")]
        [Required]
        [MaxLength(1)]
        public string Hokflg5 { get; set; }

        [Description("荷役請求用フラグ１")]
        [Column("NIEFLG1")]
        [Required]
        [MaxLength(1)]
        public string Nieflg1 { get; set; }

        [Description("荷役請求用フラグ２")]
        [Column("NIEFLG2")]
        [Required]
        [MaxLength(1)]
        public string Nieflg2 { get; set; }

        [Description("荷役請求用フラグ３")]
        [Column("NIEFLG3")]
        [Required]
        [MaxLength(1)]
        public string Nieflg3 { get; set; }

        [Description("荷役請求用フラグ４")]
        [Column("NIEFLG4")]
        [Required]
        [MaxLength(1)]
        public string Nieflg4 { get; set; }

        [Description("荷役請求用フラグ５")]
        [Column("NIEFLG5")]
        [Required]
        [MaxLength(1)]
        public string Nieflg5 { get; set; }

        [Description("登録担当")]
        [Column("CRTCOD")]
        [Required]
        [MaxLength(8)]
        public string Crtcod { get; set; }

        [Description("登録日")]
        [Column("CRTYMD")]
        [Required]
        public DateTime? Crtymd { get; set; }

        [Description("更新担当")]
        [Column("UPDCOD")]
        [Required]
        [MaxLength(8)]
        public string Updcod { get; set; }

        [Description("更新日")]
        [Column("UPDYMD")]
        [Required]
        public DateTime? Updymd { get; set; }

    }

}