using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Models
{

    [Table("m_shukkabasho")]
    public class MShukkabasho
    {

        [Description("出荷場所コード")]
        [Column("SYBCOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Sybcod { get; set; }

        [Description("出荷場所名")]
        [Column("SYBNAM")]
        [Required]
        [MaxLength(20)]
        public string Sybnam { get; set; }

        [Description("出荷場所住所")]
        [Column("JYSYO")]
        [Required]
        [MaxLength(60)]
        public string Jysyo { get; set; }

        [Description("出荷関係Ｆ５")]
        [Column("SCTL05")]
        [Required]
        [MaxLength(1)]
        public string Sctl05 { get; set; }

        [Description("出荷関係Ｆ７")]
        [Column("SCTL07")]
        [Required]
        [MaxLength(1)]
        public string Sctl07 { get; set; }

        [Description("出荷関係Ｆ９")]
        [Column("SCTL09")]
        [Required]
        [MaxLength(1)]
        public string Sctl09 { get; set; }

        [Description("出荷場所編集１５")]
        [Column("SYBC15")]
        [Required]
        [MaxLength(2)]
        public string Sybc15 { get; set; }

        [Description("出荷場所名編集１")]
        [Column("SYBN01")]
        [Required]
        [MaxLength(10)]
        public string Sybn01 { get; set; }

        [Description("出荷場所名編集２")]
        [Column("SYBN02")]
        [Required]
        [MaxLength(10)]
        public string Sybn02 { get; set; }

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