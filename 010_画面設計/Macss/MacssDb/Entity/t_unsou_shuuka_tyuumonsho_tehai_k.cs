namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_unsou_shuuka_tyuumonsho_tehai_k
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SYUKNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string CDATE { get; set; }

        public DateTime SYKYMD { get; set; }

        [Required]
        [StringLength(8)]
        public string KISYU { get; set; }

        [Required]
        [StringLength(30)]
        public string KEIFNO { get; set; }

        [Required]
        [StringLength(20)]
        public string FSYKNO { get; set; }

        [Required]
        [StringLength(2)]
        public string SYBCOD { get; set; }

        [Required]
        [StringLength(9)]
        public string TOKCOD { get; set; }

        [Required]
        [StringLength(9)]
        public string SEICOD { get; set; }

        [Required]
        [StringLength(20)]
        public string HTYNAM { get; set; }

        [Required]
        [StringLength(10)]
        public string HTYKAH { get; set; }

        [Required]
        [StringLength(8)]
        public string TANCOD { get; set; }

        [Required]
        [StringLength(10)]
        public string TANNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string HTYTEL { get; set; }

        [Required]
        [StringLength(20)]
        public string BASYO { get; set; }

        [Required]
        [StringLength(15)]
        public string TDKCOD { get; set; }

        [Required]
        [StringLength(10)]
        public string TDKYUB { get; set; }

        [Required]
        [StringLength(60)]
        public string TDKJYU { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TDSNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TDBNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKTAN { get; set; }

        [Required]
        [StringLength(20)]
        public string TDKTEL { get; set; }

        [Required]
        [StringLength(15)]
        public string DHINCOD { get; set; }

        [Required]
        [StringLength(80)]
        public string DHINNAM { get; set; }

        public decimal DSYUKSU { get; set; }

        [Required]
        [StringLength(40)]
        public string TKJIKO { get; set; }

        [Required]
        [StringLength(20)]
        public string COMENT { get; set; }

        [Required]
        [StringLength(2)]
        public string UNSCOD { get; set; }

        [Required]
        [StringLength(2)]
        public string UNSCRS { get; set; }

        [Required]
        [StringLength(9)]
        public string SIRCOD { get; set; }

        [Required]
        [StringLength(5)]
        public string UNSKBN { get; set; }

        [Required]
        [StringLength(2)]
        public string DENKBN { get; set; }

        public int DENMSU { get; set; }

        [Required]
        [StringLength(1)]
        public string UFUTAN { get; set; }

        [Required]
        [StringLength(7)]
        public string YUSONO { get; set; }

        [Required]
        [StringLength(12)]
        public string PCCOD { get; set; }

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
