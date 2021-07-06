namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_unsou_shuuka_tehai
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SYUKNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string DATAYM { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime SYKYMD { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string KISYU { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(30)]
        public string KEIFNO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string FSYKNO { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(8)]
        public string TANCOD { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(10)]
        public string TANNAM { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(15)]
        public string TDKCOD { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(60)]
        public string TDKJYU { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(20)]
        public string TDKNAM { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(20)]
        public string TDSNAM { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(20)]
        public string TDBNAM { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(20)]
        public string TDKTAN { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(20)]
        public string TDKTEL { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(8)]
        public string TDKYUB { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(15)]
        public string HINCOD { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(80)]
        public string HINNAM { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(2)]
        public string UNSCOD { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(2)]
        public string UNSCRS { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(9)]
        public string SIRCOD { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(20)]
        public string SGENNO { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(5)]
        public string UNSKBN { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(2)]
        public string SYBCOD { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(9)]
        public string TOKCOD { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(9)]
        public string SEICOD { get; set; }

        [Key]
        [Column(Order = 26)]
        [StringLength(2)]
        public string DENKBN { get; set; }

        [Key]
        [Column(Order = 27)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DENMSU { get; set; }

        [Key]
        [Column(Order = 28)]
        [StringLength(40)]
        public string TKJIKO { get; set; }

        [Key]
        [Column(Order = 29)]
        public decimal HOSOSU { get; set; }

        [Key]
        [Column(Order = 30)]
        public DateTime NFDATE { get; set; }

        [Key]
        [Column(Order = 31)]
        [StringLength(20)]
        public string DAIHNO { get; set; }

        [Key]
        [Column(Order = 32)]
        [StringLength(4)]
        public string DAIHNOYM { get; set; }

        [Key]
        [Column(Order = 33)]
        public decimal JYURYO { get; set; }

        [Key]
        [Column(Order = 34)]
        public decimal HOSOS3 { get; set; }

        [Key]
        [Column(Order = 35)]
        public decimal JYURY3 { get; set; }

        [Key]
        [Column(Order = 36)]
        [StringLength(1)]
        public string UFUTAN { get; set; }

        [Key]
        [Column(Order = 37)]
        [StringLength(7)]
        public string YUSONO { get; set; }

        [Key]
        [Column(Order = 38)]
        [StringLength(1)]
        public string CTLF19 { get; set; }

        [Key]
        [Column(Order = 39)]
        [StringLength(1)]
        public string CTLF28 { get; set; }

        [Key]
        [Column(Order = 40)]
        [StringLength(1)]
        public string CTLF29 { get; set; }

        [Key]
        [Column(Order = 41)]
        [StringLength(1)]
        public string MEHKBN { get; set; }

        [Key]
        [Column(Order = 42)]
        [StringLength(1)]
        public string JSKKBN { get; set; }

        [Key]
        [Column(Order = 43)]
        public DateTime TOCYMD { get; set; }

        [Key]
        [Column(Order = 44)]
        [StringLength(1)]
        public string TAKSIZ { get; set; }

        [Key]
        [Column(Order = 45)]
        [StringLength(1)]
        public string SEIKYU { get; set; }

        [Key]
        [Column(Order = 46)]
        [StringLength(1)]
        public string GEPPOU { get; set; }

        [Key]
        [Column(Order = 47)]
        [StringLength(12)]
        public string PCCOD { get; set; }

        [Key]
        [Column(Order = 48)]
        [StringLength(20)]
        public string SGENN2 { get; set; }

        [Key]
        [Column(Order = 49)]
        [StringLength(10)]
        public string STOUCD { get; set; }

        [Key]
        [Column(Order = 50)]
        [StringLength(2)]
        public string KENCOD { get; set; }

        [Key]
        [Column(Order = 51)]
        [StringLength(8)]
        public string JISCOD { get; set; }

        [Key]
        [Column(Order = 52)]
        [StringLength(9)]
        public string TENSIR { get; set; }

        [Key]
        [Column(Order = 53)]
        [StringLength(8)]
        public string HATCOD { get; set; }

        [Key]
        [Column(Order = 54)]
        [StringLength(20)]
        public string SGEN35 { get; set; }

        [Key]
        [Column(Order = 55)]
        [StringLength(1)]
        public string ECOFLG { get; set; }

        [Key]
        [Column(Order = 56)]
        public decimal SYUKSU { get; set; }

        [Key]
        [Column(Order = 57)]
        [StringLength(1)]
        public string SYUTUF { get; set; }

        [Key]
        [Column(Order = 58)]
        [StringLength(1)]
        public string DELFLG { get; set; }

        [Key]
        [Column(Order = 59)]
        [StringLength(8)]
        public string CRTCOD { get; set; }

        [Key]
        [Column(Order = 60)]
        public DateTime CRTYMD { get; set; }

        [Key]
        [Column(Order = 61)]
        [StringLength(8)]
        public string UPDCOD { get; set; }

        [Key]
        [Column(Order = 62)]
        public DateTime UPDYMD { get; set; }
    }
}
