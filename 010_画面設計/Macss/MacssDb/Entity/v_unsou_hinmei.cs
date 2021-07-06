namespace Macss.Database.Entity
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

        [Key]
        [Column(Order = 3)]
        [StringLength(9)]
        public string TORCOD { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(2)]
        public string KISYUA { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(6)]
        public string KISYUB { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string DTMOTO { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(5)]
        public string TKCOD { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(2)]
        public string SYUBTU { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(2)]
        public string CTLFL1 { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(15)]
        public string KHINCD { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string DELFLG { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(8)]
        public string CRTCOD { get; set; }

        [Key]
        [Column(Order = 13)]
        public DateTime CRTYMD { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(8)]
        public string UPDCOD { get; set; }

        [Key]
        [Column(Order = 15)]
        public DateTime UPDYMD { get; set; }
    }
}
