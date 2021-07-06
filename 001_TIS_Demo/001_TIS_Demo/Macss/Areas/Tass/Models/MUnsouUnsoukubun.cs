using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_unsoukubun")]
    public class MUnsouUnsoukubun
    {

        [Description("運送区分")]
        [Column("UNSKBN", Order = 0)]
        [Required]
        [Key]
        [MaxLength(5)]
        public string Unskbn { get; set; }

        [Description("運送区分名")]
        [Column("UNSNAM")]
        [Required]
        [MaxLength(20)]
        public string Unsnam { get; set; }

        [Description("事業区分コード")]
        [Column("JGYKBN")]
        [Required]
        [MaxLength(4)]
        public string Jgykbn { get; set; }

        [Description("事業区分コード２")]
        [Column("JGYKB2")]
        [Required]
        [MaxLength(4)]
        public string Jgykb2 { get; set; }

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