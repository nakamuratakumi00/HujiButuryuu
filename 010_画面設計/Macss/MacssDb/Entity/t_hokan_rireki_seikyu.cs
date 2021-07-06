namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_hokan_rireki_seikyu
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

        [Key]
        [Column(Order = 3)]
        [StringLength(3)]
        public string KISYUB { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(15)]
        public string KISNAM { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(8)]
        public string HINCOD { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(2)]
        public string HOKCOD { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(12)]
        public string PCCOD { get; set; }

        [Required]
        [StringLength(20)]
        public string HINNMK { get; set; }

        public decimal ZANSUU { get; set; }

        public decimal NYUKSU { get; set; }

        public decimal SYKSUU { get; set; }

        public decimal SEKISU { get; set; }

        public decimal HOKANT { get; set; }

        public decimal HOKANK { get; set; }

        public decimal HOKANR { get; set; }

        public decimal ATUKAI { get; set; }

        public decimal DENSUU { get; set; }

        public decimal NIEKIT { get; set; }

        public decimal NIEKIK { get; set; }

        public decimal NIEKIKR { get; set; }

        [Required]
        [StringLength(1)]
        public string HOKFLG1 { get; set; }

        [Required]
        [StringLength(1)]
        public string HOKFLG2 { get; set; }

        [Required]
        [StringLength(1)]
        public string HOKFLG3 { get; set; }

        [Required]
        [StringLength(1)]
        public string HOKFLG4 { get; set; }

        [Required]
        [StringLength(1)]
        public string HOKFLG5 { get; set; }

        [Required]
        [StringLength(1)]
        public string NIEFLG1 { get; set; }

        [Required]
        [StringLength(1)]
        public string NIEFLG2 { get; set; }

        [Required]
        [StringLength(1)]
        public string NIEFLG3 { get; set; }

        [Required]
        [StringLength(1)]
        public string NIEFLG4 { get; set; }

        [Required]
        [StringLength(1)]
        public string NIEFLG5 { get; set; }

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
