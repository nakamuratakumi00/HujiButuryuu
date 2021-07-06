namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_hokan_denpyokensu
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string KISYUB { get; set; }

        [StringLength(15)]
        public string KISNAM { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string BASYO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string HINCOD { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime INPYMD { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string KYUJIT { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(2)]
        public string DENKUB { get; set; }

        public decimal? DENSUU { get; set; }

        [StringLength(9)]
        public string SEIKYU { get; set; }

        [StringLength(1)]
        public string HOKFLG1 { get; set; }

        [StringLength(1)]
        public string HOKFLG2 { get; set; }

        [StringLength(1)]
        public string HOKFLG3 { get; set; }

        [StringLength(1)]
        public string HOKFLG4 { get; set; }

        [StringLength(1)]
        public string HOKFLG5 { get; set; }

        [StringLength(1)]
        public string HOKANS { get; set; }

        [StringLength(1)]
        public string NIEFLG1 { get; set; }

        [StringLength(1)]
        public string NIEFLG2 { get; set; }

        [StringLength(1)]
        public string NIEFLG3 { get; set; }

        [StringLength(1)]
        public string NIEFLG4 { get; set; }

        [StringLength(1)]
        public string NIEFLG5 { get; set; }

        public decimal? NNEBIR { get; set; }

        public decimal? NIEANT { get; set; }

        public decimal? NIEKYT { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string NIEKIS { get; set; }

        [StringLength(12)]
        public string PCNIE { get; set; }
    }
}
