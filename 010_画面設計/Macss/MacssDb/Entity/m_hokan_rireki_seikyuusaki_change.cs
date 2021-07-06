namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_hokan_rireki_seikyuusaki_change
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string MONTH { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(9)]
        public string SEICOD { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(9)]
        public string CHGCOD { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
