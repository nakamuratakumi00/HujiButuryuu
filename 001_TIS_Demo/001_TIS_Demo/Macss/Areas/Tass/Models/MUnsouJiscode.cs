using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_jiscode")]
    public class MUnsouJiscode
    {

        [Description("ＪＩＳコード")]
        [Column("JISCOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Jiscod { get; set; }

        [Description("分割住所１")]
        [Column("JYUSY1")]
        [Required]
        [MaxLength(10)]
        public string Jyusy1 { get; set; }

        [Description("分割住所２")]
        [Column("JYUSY2")]
        [Required]
        [MaxLength(50)]
        public string Jyusy2 { get; set; }

        [Description("分割住所３")]
        [Column("JYUSY3")]
        [Required]
        [MaxLength(50)]
        public string Jyusy3 { get; set; }

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