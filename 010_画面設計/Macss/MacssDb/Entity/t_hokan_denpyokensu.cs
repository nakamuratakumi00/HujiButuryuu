namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_hokan_denpyokensu
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string KISYUB { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string BASYO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string DENKUB { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime INPYMD { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(8)]
        public string HINCOD { get; set; }

        public decimal? DENSUU { get; set; }

        [StringLength(9)]
        public string SEIKYU { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime KEIYMD { get; set; }

        public decimal? NSKOSU { get; set; }

        [StringLength(2)]
        public string EIGSOK { get; set; }

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
