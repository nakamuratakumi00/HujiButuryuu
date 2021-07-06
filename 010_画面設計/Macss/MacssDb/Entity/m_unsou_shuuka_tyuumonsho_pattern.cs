namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_shuuka_tyuumonsho_pattern
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string SYKNO2 { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string INSCOD { get; set; }

        [StringLength(20)]
        public string HAINAM { get; set; }

        public int? MAISUU { get; set; }

        [StringLength(1)]
        public string NOPRTF { get; set; }

        [StringLength(1)]
        public string KIGENF { get; set; }

        [StringLength(20)]
        public string HTYNAM { get; set; }

        [StringLength(10)]
        public string KTYKHO { get; set; }

        [StringLength(8)]
        public string TANCOD { get; set; }

        [StringLength(10)]
        public string TANNAM { get; set; }

        [StringLength(20)]
        public string KTYTEL { get; set; }

        [StringLength(20)]
        public string BASYO { get; set; }

        [StringLength(1)]
        public string UFUTAN { get; set; }

        [StringLength(30)]
        public string KEIFNO { get; set; }

        [StringLength(15)]
        public string TDKCOD { get; set; }

        [StringLength(20)]
        public string TDKTEL { get; set; }

        [StringLength(60)]
        public string JDKJYU { get; set; }

        [StringLength(20)]
        public string TDKNAM { get; set; }

        [StringLength(20)]
        public string TDSNAM { get; set; }

        [StringLength(20)]
        public string TDBNAM { get; set; }

        [StringLength(20)]
        public string TDKTAN { get; set; }

        [StringLength(15)]
        public string HINCD1 { get; set; }

        [StringLength(15)]
        public string HINCD2 { get; set; }

        [StringLength(15)]
        public string HINCD3 { get; set; }

        [StringLength(15)]
        public string HINCD4 { get; set; }

        [StringLength(15)]
        public string HINCD5 { get; set; }

        [StringLength(80)]
        public string HINNM1 { get; set; }

        [StringLength(80)]
        public string HINNM2 { get; set; }

        [StringLength(80)]
        public string HINNM3 { get; set; }

        [StringLength(80)]
        public string HINNM4 { get; set; }

        [StringLength(80)]
        public string HINNM5 { get; set; }

        public decimal? SYKSU1 { get; set; }

        public decimal? SYKSU2 { get; set; }

        public decimal? SYKSU3 { get; set; }

        public decimal? SYKSU4 { get; set; }

        public decimal? SYKSU5 { get; set; }

        [StringLength(40)]
        public string TKJIKO { get; set; }

        [StringLength(9)]
        public string TOKCOD { get; set; }

        [StringLength(4)]
        public string UNSCOD { get; set; }

        [StringLength(2)]
        public string UNSCRS { get; set; }

        [StringLength(2)]
        public string SYBCOD { get; set; }

        [StringLength(8)]
        public string TDKYUB { get; set; }

        [StringLength(2)]
        public string DENKBN { get; set; }

        [StringLength(1)]
        public string LSTFLG { get; set; }

        [StringLength(1)]
        public string TDKHEN { get; set; }

        [StringLength(13)]
        public string IRIMOT { get; set; }

        [StringLength(20)]
        public string IRITEL { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
