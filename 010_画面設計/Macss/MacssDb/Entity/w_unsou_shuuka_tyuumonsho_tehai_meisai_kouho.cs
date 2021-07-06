namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string ACTCOD { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ACKYMD { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string SYUKNO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(6)]
        public string CDATE { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RENBAN { get; set; }

        [StringLength(15)]
        public string HINCOD { get; set; }

        [StringLength(80)]
        public string HINNAM { get; set; }

        public decimal? SYUKSU { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
