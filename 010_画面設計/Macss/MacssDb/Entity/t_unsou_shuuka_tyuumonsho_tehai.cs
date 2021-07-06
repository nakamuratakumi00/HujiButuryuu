namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_unsou_shuuka_tyuumonsho_tehai
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SYUKNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string CDATE { get; set; }

        public DateTime? SYKYMD { get; set; }

        [StringLength(8)]
        public string KISYU { get; set; }

        [StringLength(30)]
        public string KEIFNO { get; set; }

        [StringLength(20)]
        public string FSYKNO { get; set; }

        [StringLength(2)]
        public string SYBCOD { get; set; }

        [StringLength(9)]
        public string TOKCOD { get; set; }

        [StringLength(9)]
        public string SEICOD { get; set; }

        [StringLength(20)]
        public string HTYNAM { get; set; }

        [StringLength(10)]
        public string HTYKAH { get; set; }

        [StringLength(8)]
        public string TANCOD { get; set; }

        [StringLength(10)]
        public string TANNAM { get; set; }

        [StringLength(20)]
        public string HTYTEL { get; set; }

        [StringLength(20)]
        public string BASYO { get; set; }

        [StringLength(15)]
        public string TDKCOD { get; set; }

        [StringLength(10)]
        public string TDKYUB { get; set; }

        [StringLength(60)]
        public string TDKJYU { get; set; }

        [StringLength(20)]
        public string TDKNAM { get; set; }

        [StringLength(20)]
        public string TDSNAM { get; set; }

        [StringLength(20)]
        public string TDBNAM { get; set; }

        [StringLength(20)]
        public string TDKTAN { get; set; }

        [StringLength(20)]
        public string TDKTEL { get; set; }

        [StringLength(15)]
        public string DHINCOD { get; set; }

        [StringLength(80)]
        public string DHINNAM { get; set; }

        public decimal? DSYUKSU { get; set; }

        [StringLength(40)]
        public string TKJIKO { get; set; }

        [StringLength(20)]
        public string COMENT { get; set; }

        [StringLength(2)]
        public string UNSCOD { get; set; }

        [StringLength(2)]
        public string UNSCRS { get; set; }

        [StringLength(9)]
        public string SIRCOD { get; set; }

        [StringLength(5)]
        public string UNSKBN { get; set; }

        [StringLength(2)]
        public string DENKBN { get; set; }

        public int? DENMSU { get; set; }

        [StringLength(1)]
        public string UFUTAN { get; set; }

        [StringLength(7)]
        public string YUSONO { get; set; }

        [StringLength(1)]
        public string DENF { get; set; }

        public DateTime? HKUYMD { get; set; }

        [StringLength(8)]
        public string HKUCOD { get; set; }

        [StringLength(12)]
        public string PCCOD { get; set; }

        [StringLength(1)]
        public string YUBFLG { get; set; }

        [StringLength(1)]
        public string MUKOUKBN { get; set; }

        [StringLength(20)]
        public string MUKOURIYUU { get; set; }

        [StringLength(6)]
        public string INSCOD { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
