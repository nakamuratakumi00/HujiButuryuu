using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Models
{
    [Table("m_kishu")]
    public class MKishu
    {

        [Description("機種Ａ")]
        [Column("KISYUA", Order = 0)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("機種Ｂ１")]
        [Column("KISYB1", Order = 1)]
        [Required]
        [Key]
        [MaxLength(3)]
        public string Kisyb1 { get; set; }

        [Description("機種Ｂ２")]
        [Column("KISYB2", Order = 2)]
        [Required]
        [Key]
        [MaxLength(3)]
        public string Kisyb2 { get; set; }

        [Description("機種区分名")]
        [Column("KISNAM")]
        [Required]
        [MaxLength(20)]
        public string Kisnam { get; set; }

        [Description("制御フラグ３")]
        [Column("CTLF03")]
        [Required]
        [MaxLength(1)]
        public string Ctlf03 { get; set; }

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