using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{
    [Table("t_hokan_rireki_denpyokensu")]
    public class THokanRirekiDenpyokensu
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

        [Description("機種Ｂ")]
        [Column("KISYUB", Order = 2)]
        [Required]
        [Key]
        [MaxLength(3)]
        public string Kisyub { get; set; }

        [Description("回収場所")]
        [Column("BASYO", Order = 3)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Basyo { get; set; }

        [Description("伝票Ｎｏ")]
        [Column("DENKUB", Order = 4)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Denkub { get; set; }

        [Description("インプット日")]
        [Column("INPYMD", Order = 5)]
        [Key]
        public DateTime? Inpymd { get; set; }

        [Description("品番コード")]
        [Column("HINCOD", Order = 6)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Hincod { get; set; }

        [Description("伝票件数")]
        [Column("DENSUU")]
        [CustomAttributes.DecimalPrecisionAttribute(6, 0)]
        public decimal? Densuu { get; set; }

        [Description("物流請求Ｎｏ")]
        [Column("SEIKYU")]
        [MaxLength(9)]
        public string Seikyu { get; set; }

        [Description("計上日")]
        [Column("KEIYMD", Order = 7)]
        [Required]
        [Key]
        public DateTime? Keiymd { get; set; }

        [Description("入出庫数")]
        [Column("NSKOSU")]
        [CustomAttributes.DecimalPrecisionAttribute(9, 0)]
        public decimal? Nskosu { get; set; }

        [Description("営業倉庫")]
        [Column("EIGSOK")]
        [MaxLength(2)]
        public string Eigsok { get; set; }

        [Description("データ管理元")]
        [Column("DTMOTO")]
        [MaxLength(1)]
        public string Dtmoto { get; set; }

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
