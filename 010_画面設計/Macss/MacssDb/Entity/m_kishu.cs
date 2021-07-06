namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_kishu
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string KISYB1 { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string KISYB2 { get; set; }

        [Required]
        [StringLength(20)]
        public string KISNAM { get; set; }

        [Required]
        [StringLength(1)]
        public string CTLF03 { get; set; }

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
