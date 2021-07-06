namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_postalcode
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string YUBINN { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string JYUSY1 { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(40)]
        public string JYUSY2 { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(40)]
        public string JYUSY3 { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string JYKANA { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string KENJY1 { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(40)]
        public string KENJY2 { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(40)]
        public string KENJY3 { get; set; }

        [Required]
        [StringLength(8)]
        public string JISCOD { get; set; }

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
