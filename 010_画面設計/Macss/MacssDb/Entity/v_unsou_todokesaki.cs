namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_unsou_todokesaki
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string TDKCOD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string TDKNAM { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string TDKNMS { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(80)]
        public string TDKNMK { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(20)]
        public string TDBNAM { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string TDKTAN { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(60)]
        public string JYUSYO { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(20)]
        public string TDKTEL { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string SDEK01 { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string SDEK02 { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(1)]
        public string SDEK03 { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string SDEK04 { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(1)]
        public string SDEK05 { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(1)]
        public string SDEK06 { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string SDEK07 { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(1)]
        public string SDEK08 { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(1)]
        public string SDEK09 { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(1)]
        public string SDEK10 { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(1)]
        public string SDEK11 { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(1)]
        public string SDEK12 { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(1)]
        public string SDEK13 { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(1)]
        public string SDEK14 { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(1)]
        public string SDEK15 { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(40)]
        public string TKJIKO { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(1)]
        public string DTMOTO { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(10)]
        public string YUBINN { get; set; }

        [Key]
        [Column(Order = 26)]
        [StringLength(20)]
        public string FAXNO { get; set; }

        [Key]
        [Column(Order = 27)]
        [StringLength(15)]
        public string KTDKCD { get; set; }

        [Key]
        [Column(Order = 28)]
        [StringLength(15)]
        public string KITDCD { get; set; }

        [Key]
        [Column(Order = 29)]
        [StringLength(1)]
        public string DELFLG { get; set; }

        [Key]
        [Column(Order = 30)]
        [StringLength(8)]
        public string CRTCOD { get; set; }

        [Key]
        [Column(Order = 31)]
        public DateTime CRTYMD { get; set; }

        [Key]
        [Column(Order = 32)]
        [StringLength(8)]
        public string UPDCOD { get; set; }

        [Key]
        [Column(Order = 33)]
        public DateTime UPDYMD { get; set; }
    }
}
