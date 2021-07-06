namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_hokan_rireki_keiyaku
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string MONTH { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(15)]
        public string KISNAM { get; set; }

        [StringLength(1)]
        public string SEITAI { get; set; }

        [StringLength(40)]
        public string SEINAM { get; set; }

        [StringLength(15)]
        public string TANBSY { get; set; }

        [StringLength(9)]
        public string FBTCOD { get; set; }

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

        public decimal? HNEBIR { get; set; }

        public decimal? HOKANT { get; set; }

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

        public decimal? NIEANT { get; set; }

        public decimal? NIEKYT { get; set; }

        public decimal? NNEBIR { get; set; }

        public decimal? OJYUKR { get; set; }

        public decimal? OSYJYR { get; set; }

        public decimal? HJYUKR { get; set; }

        public decimal? HSYJYR { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
