using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_unsoukousu")]
    public class MUnsouUnsoukousu
    {

        [Description("運送コースコード")]
        [Column("UNSCRS", Order = 0)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Unscrs { get; set; }

        [Description("運送コース名")]
        [Column("CRSNAM")]
        [Required]
        [MaxLength(20)]
        public string Crsnam { get; set; }

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