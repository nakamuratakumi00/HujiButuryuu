namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_unsou_shuuka_tyuumonsho_tehai_mk
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SYUKNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string CDATE { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RENBAN { get; set; }

        [Required]
        [StringLength(15)]
        public string HINCOD { get; set; }

        [Required]
        [StringLength(80)]
        public string HINNAM { get; set; }

        public decimal SYUKSU { get; set; }

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
