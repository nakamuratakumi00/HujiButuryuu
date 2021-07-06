using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{

    [Table("m_hokan_rireki_keiyaku")]
    public class MHokanRirekiKeiyaku
    {

        [Description("実施年月")]
        [Column("MONTH", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Month { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA", Order = 1)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("機種名")]
        [Column("KISNAM")]
        [MaxLength(15)]
        public string Kisnam { get; set; }

        [Description("請求対象")]
        [Column("SEITAI")]
        [MaxLength(1)]
        public string Seitai { get; set; }

        [Description("請求先名")]
        [Column("SEINAM")]
        [MaxLength(40)]
        public string Seinam { get; set; }

        [Description("ＦＢ担当部署")]
        [Column("TANBSY")]
        [MaxLength(15)]
        public string Tanbsy { get; set; }

        [Description("本社得意先コード")]
        [Column("FBTCOD")]
        [MaxLength(9)]
        public string Fbtcod { get; set; }

        [Description("保管請求用フラグ１")]
        [Column("HOKFLG1")]
        [MaxLength(1)]
        public string Hokflg1 { get; set; }

        [Description("保管請求用フラグ２")]
        [Column("HOKFLG2")]
        [MaxLength(1)]
        public string Hokflg2 { get; set; }

        [Description("保管請求用フラグ３")]
        [Column("HOKFLG3")]
        [MaxLength(1)]
        public string Hokflg3 { get; set; }

        [Description("保管請求用フラグ４")]
        [Column("HOKFLG4")]
        [MaxLength(1)]
        public string Hokflg4 { get; set; }

        [Description("保管請求用フラグ５")]
        [Column("HOKFLG5")]
        [MaxLength(1)]
        public string Hokflg5 { get; set; }

        [Description("保管値引率")]
        [Column("HNEBIR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Hnebir { get; set; }

        [Description("保管単価")]
        [Column("HOKANT")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 5)]
        public decimal? Hokant { get; set; }

        [Description("荷役請求用フラグ１")]
        [Column("NIEFLG1")]
        [MaxLength(1)]
        public string Nieflg1 { get; set; }

        [Description("荷役請求用フラグ２")]
        [Column("NIEFLG2")]
        [MaxLength(1)]
        public string Nieflg2 { get; set; }

        [Description("荷役請求用フラグ３")]
        [Column("NIEFLG3")]
        [MaxLength(1)]
        public string Nieflg3 { get; set; }

        [Description("荷役請求用フラグ４")]
        [Column("NIEFLG4")]
        [MaxLength(1)]
        public string Nieflg4 { get; set; }

        [Description("荷役請求用フラグ５")]
        [Column("NIEFLG5")]
        [MaxLength(1)]
        public string Nieflg5 { get; set; }

        [Description("荷役単価")]
        [Column("NIEANT")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 4)]
        public decimal? Nieant { get; set; }

        [Description("荷役単価（休日用）")]
        [Column("NIEKYT")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 4)]
        public decimal? Niekyt { get; set; }

        [Description("荷役値引率")]
        [Column("NNEBIR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Nnebir { get; set; }

        [Description("乙地従価率(1000円につき)")]
        [Column("OJYUKR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Ojyukr { get; set; }

        [Description("乙地収受率")]
        [Column("OSYJYR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Osyjyr { get; set; }

        [Description("丙地従価率(1000円につき)")]
        [Column("HJYUKR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Hjyukr { get; set; }

        [Description("丙地収受率")]
        [Column("HSYJYR")]
        [CustomAttributes.DecimalPrecisionAttribute(5, 3)]
        public decimal? Hsyjyr { get; set; }

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
