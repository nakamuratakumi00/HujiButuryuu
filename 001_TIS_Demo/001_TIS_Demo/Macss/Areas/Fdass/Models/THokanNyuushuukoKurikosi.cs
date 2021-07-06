using Macss.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Macss.Areas.Fdass.Models
{
    [Table("t_hokan_nyuushuuko_kurikosi")]
    public class THokanNyuushuukoKurikosi
    {

        [Description("繰越年月日")]
        [Column("KURYMD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Kurymd { get; set; }

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

        [Description("品番コード")]
        [Column("HINCOD", Order = 3)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Hincod { get; set; }

        [Description("保管場所")]
        [Column("HOKCOD", Order = 4)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Hokcod { get; set; }

        [Description("区分")]
        [Column("DKBN", Order = 5)]
        [MaxLength(1)]
        [Required]
        [Key]
        public string Dkbn { get; set; }

        [Description("前月残数")]
        [Column("ZANSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Zansuu { get; set; }

        [Description("前月残金額")]
        [Column("ZANKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Zankin { get; set; }

        [Description("仕込倉入数")]
        [Column("SIKSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Siksuu { get; set; }

        [Description("仕込倉入金額")]
        [Column("SIKKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Sikkin { get; set; }

        [Description("改造倉入数")]
        [Column("KAISUU")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Kaisuu { get; set; }

        [Description("改造倉入金額")]
        [Column("KAIKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Kaikin { get; set; }

        [Description("出庫数")]
        [Column("SYKSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Syksuu { get; set; }

        [Description("出庫金額")]
        [Column("SYKKIN")]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Sykkin { get; set; }

        [Description("物流請求Ｎｏ")]
        [Column("SEIKYU")]
        [MaxLength(9)]
        public string Seikyu { get; set; }

        [Description("媒体データ区分")]
        [Column("BAITAI")]
        [MaxLength(1)]
        public string Baitai { get; set; }

        [Description("当月末在庫数")]
        [Column("TOUZAN")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Touzan { get; set; }

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