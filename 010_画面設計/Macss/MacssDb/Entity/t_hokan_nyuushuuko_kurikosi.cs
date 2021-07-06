namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_hokan_nyuushuuko_kurikosi
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string KURYMD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string KISYUB { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string HINCOD { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(2)]
        public string HOKCOD { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string DKBN { get; set; }

        public decimal? ZANSUU { get; set; }

        public decimal? ZANKIN { get; set; }

        public decimal? SIKSUU { get; set; }

        public decimal? SIKKIN { get; set; }

        public decimal? KAISUU { get; set; }

        public decimal? KAIKIN { get; set; }

        public decimal? SYKSUU { get; set; }

        public decimal? SYKKIN { get; set; }

        [StringLength(9)]
        public string SEIKYU { get; set; }

        [StringLength(1)]
        public string BAITAI { get; set; }

        public decimal? TOUZAN { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
