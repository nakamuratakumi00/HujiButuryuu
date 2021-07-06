namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_hokan_rireki_seikyu_kyoten
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string MONTH { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string BASYO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(3)]
        public string KISYUB { get; set; }

        [StringLength(20)]
        public string BASNAM { get; set; }

        [StringLength(3)]
        public string KISYBN { get; set; }

        [StringLength(9)]
        public string SEICOD { get; set; }

        public decimal? ZANSUU { get; set; }

        public decimal? ZANKIN { get; set; }

        public decimal? SIKSUU { get; set; }

        public decimal? SIKKIN { get; set; }

        public decimal? KAISUU { get; set; }

        public decimal? KAIKIN { get; set; }

        public decimal? NYUKSU { get; set; }

        public decimal? NYUKIN { get; set; }

        public decimal? SYKSUU { get; set; }

        public decimal? SYKKIN { get; set; }

        public decimal? ZAIKSU { get; set; }

        public decimal? ZAIKIN { get; set; }

        public decimal? DENSUU { get; set; }

        public decimal? DENSKY { get; set; }

        public decimal? HOKANK { get; set; }

        public decimal? NIEKIK { get; set; }

        public decimal? NIEKYJ { get; set; }

        [StringLength(12)]
        public string PCCODH { get; set; }

        [StringLength(12)]
        public string PCCODN { get; set; }

        [StringLength(4)]
        public string DATAYM { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
