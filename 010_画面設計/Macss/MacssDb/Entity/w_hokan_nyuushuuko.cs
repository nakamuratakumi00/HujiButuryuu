namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class w_hokan_nyuushuuko
    {
        public decimal ID { get; set; }

        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(3)]
        public string KISYUB { get; set; }

        [StringLength(2)]
        public string HOKCOD { get; set; }

        [StringLength(8)]
        public string HINCOD { get; set; }

        public decimal? ZANSUU { get; set; }

        public decimal? ZANKIN { get; set; }

        public decimal? SIKSUU { get; set; }

        public decimal? SIKKIN { get; set; }

        public decimal? KAISUU { get; set; }

        public decimal? KAIKIN { get; set; }

        public decimal? SYKSUU { get; set; }

        public decimal? SYKKIN { get; set; }

        [StringLength(1)]
        public string DKBN { get; set; }

        [StringLength(9)]
        public string SEIKYU { get; set; }

        [StringLength(1)]
        public string BAITAI { get; set; }

        public decimal? TOUZAN { get; set; }

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
