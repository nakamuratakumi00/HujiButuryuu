namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_todokesaki_koyuu
    {
        [Key]
        [StringLength(15)]
        public string TDKCOD { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKNMS { get; set; }

        [Required]
        [StringLength(80)]
        public string TDKNMK { get; set; }

        [Required]
        [StringLength(20)]
        public string TDBNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKTAN { get; set; }

        [Required]
        [StringLength(60)]
        public string JYUSYO { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKTEL { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK01 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK02 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK03 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK04 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK05 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK06 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK07 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK08 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK09 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK10 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK11 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK12 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK13 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK14 { get; set; }

        [Required]
        [StringLength(1)]
        public string SDEK15 { get; set; }

        [Required]
        [StringLength(40)]
        public string TKJIKO { get; set; }

        [Required]
        [StringLength(1)]
        public string DTMOTO { get; set; }

        [Required]
        [StringLength(10)]
        public string YUBINN { get; set; }

        [Required]
        [StringLength(20)]
        public string FAXNO { get; set; }

        [Required]
        [StringLength(15)]
        public string KTDKCD { get; set; }

        [Required]
        [StringLength(15)]
        public string KITDCD { get; set; }

        [Required]
        [StringLength(1)]
        public string DELFLG { get; set; }

        [Required]
        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime CRTYMD { get; set; }

        [Required]
        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime UPDYMD { get; set; }
    }
}
