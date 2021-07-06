namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_hokan_bumon
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string BASYO { get; set; }

        [StringLength(4)]
        public string BMNCOD { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
