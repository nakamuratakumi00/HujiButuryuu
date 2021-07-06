namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_torihikisaki
    {
        [Key]
        [StringLength(9)]
        public string TORCOD { get; set; }

        [Required]
        [StringLength(40)]
        public string TORNAM { get; set; }

        [Required]
        [StringLength(80)]
        public string TORNMK { get; set; }

        [Required]
        [StringLength(20)]
        public string BUKNAM { get; set; }

        [Required]
        [StringLength(10)]
        public string TANNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TELNO { get; set; }

        [Required]
        [StringLength(20)]
        public string FAXNO { get; set; }

        [Required]
        [StringLength(8)]
        public string YUBINN { get; set; }

        [Required]
        [StringLength(60)]
        public string JYSYO { get; set; }

        [Required]
        [StringLength(2)]
        public string SIMDAY { get; set; }

        [Required]
        [StringLength(9)]
        public string SECO01 { get; set; }

        [Required]
        [StringLength(9)]
        public string SECO06 { get; set; }

        [Required]
        [StringLength(9)]
        public string FBTCDM { get; set; }

        [Required]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Required]
        [StringLength(1)]
        public string LSTFLG { get; set; }

        [Required]
        [StringLength(9)]
        public string FBTCOD { get; set; }

        [Required]
        [StringLength(4)]
        public string KOKCOD { get; set; }

        [Required]
        [StringLength(1)]
        public string MEHK01 { get; set; }

        [Required]
        [StringLength(9)]
        public string FBTCDS { get; set; }

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
