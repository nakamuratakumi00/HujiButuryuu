namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_shukkabasho
    {
        [Key]
        [StringLength(2)]
        public string SYBCOD { get; set; }

        [Required]
        [StringLength(20)]
        public string SYBNAM { get; set; }

        [Required]
        [StringLength(60)]
        public string JYSYO { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL05 { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL07 { get; set; }

        [Required]
        [StringLength(1)]
        public string SCTL09 { get; set; }

        [Required]
        [StringLength(2)]
        public string SYBC15 { get; set; }

        [Required]
        [StringLength(10)]
        public string SYBN01 { get; set; }

        [Required]
        [StringLength(10)]
        public string SYBN02 { get; set; }

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
