namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_unsouhouhou
    {
        [Key]
        [StringLength(2)]
        public string UNSCOD { get; set; }

        [Required]
        [StringLength(20)]
        public string UNSNAM { get; set; }

        [Required]
        [StringLength(6)]
        public string RYKNAM { get; set; }

        [Required]
        [StringLength(9)]
        public string TORCOD { get; set; }

        [Required]
        [StringLength(5)]
        public string UNSKBN { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL02 { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL03 { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL06 { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL08 { get; set; }

        [Required]
        [StringLength(1)]
        public string PCTL05 { get; set; }

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
