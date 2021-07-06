namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class w_hokan_seihin
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string HINCOD { get; set; }

        [StringLength(80)]
        public string HINNAM { get; set; }

        [StringLength(80)]
        public string HINNMK { get; set; }

        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(6)]
        public string KISYUB { get; set; }

        [StringLength(2)]
        public string SYBCOD { get; set; }

        public decimal? FPTANK { get; set; }

        public DateTime? MENTBI { get; set; }

        [StringLength(1)]
        public string ZANTEI { get; set; }

        public decimal? ZAIKSU { get; set; }

        [StringLength(2)]
        public string HOKCOD { get; set; }

        [StringLength(1)]
        public string FRIKAE { get; set; }

        [StringLength(1)]
        public string DTMOTO { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
