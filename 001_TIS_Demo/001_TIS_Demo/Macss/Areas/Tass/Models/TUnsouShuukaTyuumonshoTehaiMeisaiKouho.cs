using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("t_unsou_shuuka_tyuumonsho_tehai_mk")]
    public class TUnsouShuukaTyuumonshoTehaiMeisaiKouho
    {

        [Description("出荷Ｎｏ")]
        [Column("SYUKNO", Order = 0)]
        [Required]
        [Key]
        [MaxLength(20)]
        public string Syukno { get; set; }

        [Description("データ作成日")]
        [Column("CDATE", Order = 1)]
        [Required]
        [Key]
        [MaxLength(6)]
        public string Cdate { get; set; }

        [Description("連番")]
        [Column("RENBAN", Order = 2)]
        [Required]
        [Key]
        public int? Renban { get; set; }

        [Description("品名コード")]
        [Column("HINCOD")]
        [Required]
        [MaxLength(15)]
        public string Hincod { get; set; }

        [Description("品名")]
        [Column("HINNAM")]
        [Required]
        [MaxLength(80)]
        public string Hinnam { get; set; }

        [Description("出荷数")]
        [Column("SYUKSU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syuksu { get; set; }

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