namespace Macss.Areas.Tass.Models
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

        [StringLength(20)]
        public string TDKNAM { get; set; }

        [StringLength(20)]
        public string TDKNMS { get; set; }

        [StringLength(80)]
        public string TDKNMK { get; set; }

        [StringLength(20)]
        public string TDBNAM { get; set; }

        [StringLength(20)]
        public string TDKTAN { get; set; }

        [StringLength(60)]
        public string JYUSYO { get; set; }

        [StringLength(20)]
        public string TDKTEL { get; set; }

        [StringLength(1)]
        public string SDEK01 { get; set; }

        [StringLength(1)]
        public string SDEK02 { get; set; }

        [StringLength(1)]
        public string SDEK03 { get; set; }

        [StringLength(1)]
        public string SDEK04 { get; set; }

        [StringLength(1)]
        public string SDEK05 { get; set; }

        [StringLength(1)]
        public string SDEK06 { get; set; }

        [StringLength(1)]
        public string SDEK07 { get; set; }

        [StringLength(1)]
        public string SDEK08 { get; set; }

        [StringLength(1)]
        public string SDEK09 { get; set; }

        [StringLength(1)]
        public string SDEK10 { get; set; }

        [StringLength(1)]
        public string SDEK11 { get; set; }

        [StringLength(1)]
        public string SDEK12 { get; set; }

        [StringLength(1)]
        public string SDEK13 { get; set; }

        [StringLength(1)]
        public string SDEK14 { get; set; }

        [StringLength(1)]
        public string SDEK15 { get; set; }

        [StringLength(40)]
        public string TKJIKO { get; set; }

        [StringLength(1)]
        public string DTMOTO { get; set; }

        [StringLength(10)]
        public string YUBINN { get; set; }

        [StringLength(20)]
        public string FAXNO { get; set; }

        [StringLength(15)]
        public string KTDKCD { get; set; }

        [StringLength(15)]
        public string KITDCD { get; set; }

        [StringLength(1)]
        public string DELFLG { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
