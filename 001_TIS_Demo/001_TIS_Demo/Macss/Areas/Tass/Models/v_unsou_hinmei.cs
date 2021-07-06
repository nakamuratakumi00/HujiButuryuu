namespace Macss.Areas.Tass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_unsou_hinmei
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string HINCOD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(80)]
        public string HINNAM { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(80)]
        public string HINNMK { get; set; }

        [StringLength(9)]
        public string TORCOD { get; set; }

        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(6)]
        public string KISYUB { get; set; }

        [StringLength(1)]
        public string DTMOTO { get; set; }

        [StringLength(5)]
        public string TKCOD { get; set; }

        [StringLength(2)]
        public string SYUBTU { get; set; }

        [StringLength(2)]
        public string CTLFL1 { get; set; }

        [StringLength(15)]
        public string KHINCD { get; set; }

        [StringLength(1)]
        public string CRTCOD { get; set; }

        [StringLength(8)]
        public string DELFLG { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
