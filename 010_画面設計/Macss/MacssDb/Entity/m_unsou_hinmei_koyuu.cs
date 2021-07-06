namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_hinmei_koyuu
    {
        [Key]
        [StringLength(15)]
        public string HINCOD { get; set; }

        [Required]
        [StringLength(80)]
        public string HINNAM { get; set; }

        [Required]
        [StringLength(80)]
        public string HINNMK { get; set; }

        [Required]
        [StringLength(9)]
        public string TORCOD { get; set; }

        [Required]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Required]
        [StringLength(6)]
        public string KISYUB { get; set; }

        [Required]
        [StringLength(1)]
        public string DTMOTO { get; set; }

        [Required]
        [StringLength(5)]
        public string TKCOD { get; set; }

        [Required]
        [StringLength(2)]
        public string SYUBTU { get; set; }

        [Required]
        [StringLength(2)]
        public string CTLFL1 { get; set; }

        [Required]
        [StringLength(15)]
        public string KHINCD { get; set; }

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
