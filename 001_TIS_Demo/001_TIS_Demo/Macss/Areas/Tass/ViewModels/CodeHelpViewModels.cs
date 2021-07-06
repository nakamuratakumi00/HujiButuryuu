using System;
using System.ComponentModel.DataAnnotations;
namespace Macss.Areas.Tass.ViewModels
{
    public class CodeHelpViewModels
    {

        #region 得意先
        public class TokuisakiViewData
        {

            public TokuisakiSerch Serch { get; set; }

            public TokuisakiInformation Information { get; set; }

        }

        public class TokuisakiSerch
        {

            [Display(Name = "カナ")]
            [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tornmk { get; set; }

            [Display(Name = "コード")]
            [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Torcod { get; set; }

            [Display(Name = "FB本社得意先コード")]
            [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Fbtcod { get; set; }

            [Display(Name = "会社名")]
            [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tornam { get; set; }

            [Display(Name = "部課名")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Buknam { get; set; }

            [Display(Name = "住所")]
            [MaxLength(60, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Jysyo { get; set; }

        }

        public class TokuisakiInformation
        {
            [Display(Name = "カナ")]
            public string Tornmk { get; set; }

            [Display(Name = "コード")]
            public string Torcod { get; set; }

            [Display(Name = "FB本社得意先コード")]
            public string Fbtcod { get; set; }

            [Display(Name = "会社名")]
            public string Tornam { get; set; }

            [Display(Name = "部課名")]
            public string Buknam { get; set; }

            [Display(Name = "住所")]
            public string Jysyo { get; set; }

        }
        #endregion

        #region 仕入先
        public class SiiresakiViewData
        {

            public SiiresakiSerch Serch { get; set; }

            public SiiresakiInformation Information { get; set; }

        }

        public class SiiresakiSerch
        {

            [Display(Name = "カナ")]
            [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string TornmkSi { get; set; }

            [Display(Name = "コード")]
            [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string TorcodSi { get; set; }

            [Display(Name = "FB本社仕入先コード")]
            [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string FbtcodSi { get; set; }

            [Display(Name = "会社名")]
            [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string TornamSi { get; set; }

            [Display(Name = "部課名")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string BuknamSi { get; set; }

            [Display(Name = "住所")]
            [MaxLength(60, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string JysyoSi { get; set; }

        }

        public class SiiresakiInformation
        {
            [Display(Name = "カナ")]
            public string TornmkSi { get; set; }

            [Display(Name = "コード")]
            public string TorcodSi { get; set; }

            [Display(Name = "FB本社仕入先コード")]
            public string FbtcodSi { get; set; }

            [Display(Name = "会社名")]
            public string TornamSi { get; set; }

            [Display(Name = "部課名")]
            public string BuknamSi { get; set; }

            [Display(Name = "住所")]
            public string JysyoSi { get; set; }

        }
        #endregion

        #region 届先
        public class TodokesakiViewData
        {

            public TodokesakihelpSerch Serch { get; set; }

            public TodokesakihelpInformation Information { get; set; }

        }

        public class TodokesakihelpSerch
        {

            [Display(Name = "カナ")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tdknmk { get; set; }

            [Display(Name = "コード")]
            [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tdkcod { get; set; }

            [Display(Name = "届先社名")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tdknam { get; set; }

            [Display(Name = "届先支店名")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tdknms { get; set; }

            [Display(Name = "届先部課名")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tdbnam { get; set; }

            [Display(Name = "顧客届先コード")]
            [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Ktdkcd { get; set; }

            [Display(Name = "住所")]
            [MaxLength(60, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Jyusyo { get; set; }

            [Display(Name = "使用区分")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Tornam { get; set; }

            [Display(Name = "H")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek01 { get; set; }

            [Display(Name = "i")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek02 { get; set; }

            [Display(Name = "M")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek03 { get; set; }

            [Display(Name = "P")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek04 { get; set; }

            [Display(Name = "集")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek05 { get; set; }

            [Display(Name = "構")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek06 { get; set; }

            [Display(Name = "Y")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek07 { get; set; }

            [Display(Name = "半")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek08 { get; set; }

            [Display(Name = "長")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek09 { get; set; }

            [Display(Name = "在")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek10 { get; set; }

            [Display(Name = "需")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek11 { get; set; }

            [Display(Name = "顧")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek12 { get; set; }

            [Display(Name = "得")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek13 { get; set; }

            [Display(Name = "ブ")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek14 { get; set; }

            [Display(Name = "基")]
            [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Sdek15 { get; set; }
        }

        public class TodokesakihelpInformation
        {
            [Display(Name = "カナ")]
            public string Tdknmk { get; set; }

            [Display(Name = "コード")]
            public string Tdkcod { get; set; }

            [Display(Name = "届先社名")]
            public string Tdknam { get; set; }

            [Display(Name = "届先支店名")]
            public string Tdknms { get; set; }

            [Display(Name = "届先部課名")]
            public string Tdbnam { get; set; }

            [Display(Name = "住所")]
            public string Jyusyo { get; set; }

            [Display(Name = "H")]
            public string Sdek01 { get; set; }

            [Display(Name = "i")]
            public string Sdek02 { get; set; }

            [Display(Name = "M")]
            public string Sdek03 { get; set; }

            [Display(Name = "P")]
            public string Sdek04 { get; set; }

            [Display(Name = "集")]
            public string Sdek05 { get; set; }

            [Display(Name = "構")]
            public string Sdek06 { get; set; }

            [Display(Name = "Y")]
            public string Sdek07 { get; set; }

            [Display(Name = "半")]
            public string Sdek08 { get; set; }

            [Display(Name = "長")]
            public string Sdek09 { get; set; }

            [Display(Name = "在")]
            public string Sdek10 { get; set; }

            [Display(Name = "需")]
            public string Sdek11 { get; set; }

            [Display(Name = "顧")]
            public string Sdek12 { get; set; }

            [Display(Name = "得")]
            public string Sdek13 { get; set; }

            [Display(Name = "ブ")]
            public string Sdek14 { get; set; }

            [Display(Name = "基")]
            public string Sdek15 { get; set; }

        }
        #endregion

        #region 品名
        public class HinmeisakiViewData
        {
            public HinmeisakiSerch Serch { get; set; }

            public HinmeisakiInformation Information { get; set; }

        }

        public class HinmeisakiSerch
        {
            [Display(Name = "品名カナ")]
            [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Hinnmk { get; set; }

            [Display(Name = "コード")]
            [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Hincod { get; set; }

            [Display(Name = "品名")]
            [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Hinnam { get; set; }

            [Display(Name = "抽出フラグ")]
            [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Ctlfl1 { get; set; }

            [Display(Name = "顧客品名コード")]
            [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Khincd { get; set; }

        }

        public class HinmeisakiInformation
        {
            [Display(Name = "品名カナ")]
            public string Hinnmk { get; set; }

            [Display(Name = "コード")]
            public string Hincod { get; set; }

            [Display(Name = "品名")]
            public string Hinnam { get; set; }

            [Display(Name = "抽出フラグ")]
            public string Ctlfl1 { get; set; }
        }

        #endregion

        #region 出荷場所
        public class ShukkabashoViewData
        {

            public ShukkabashoSerch Serch { get; set; }

            public ShukkabashoInformation Information { get; set; }

        }

        public class ShukkabashoSerch
        {

            [Display(Name = "コード")]
            [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string SybcodCh { get; set; }

            [Display(Name = "名称")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string SybnamCh { get; set; }

        }

        public class ShukkabashoInformation
        {
            [Display(Name = "コード")]
            public string SybcodCh { get; set; }

            [Display(Name = "名称")]
            public string SybnamCh { get; set; }

        }
        #endregion

        #region 郵便番号
        public class YubinbangoViewData
        {

            public YubinbangoSerch Serch { get; set; }

            public YubinbangoInformation Information { get; set; }

        }

        public class YubinbangoSerch
        {

            [Display(Name = "都道府県")]
            [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Jyusy1 { get; set; }

            [Display(Name = "市町村")]
            [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Jyusy2 { get; set; }

            [Display(Name = "コード")]
            [MaxLength(8, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Yubinn { get; set; }

        }

        public class YubinbangoInformation
        {
            [Display(Name = "都道府県")]
            public string Jyusy1 { get; set; }

            [Display(Name = "市町村")]
            public string Jyusy2 { get; set; }

            [Display(Name = "コード")]
            public string Yubinn { get; set; }
        }
        #endregion

        #region 運送方法
        public class UnsohohoViewData
        {

            public UnsohohoSerch Serch { get; set; }

            public UnsohohoInformation Information { get; set; }

        }

        public class UnsohohoSerch
        {

            [Display(Name = "コード")]
            [MaxLength(5, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Unscod { get; set; }

            [Display(Name = "名称")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Unsnam { get; set; }

        }

        public class UnsohohoInformation
        {
            [Display(Name = "コード")]
            public string Unscod { get; set; }

            [Display(Name = "名称")]
            public string Unsnam { get; set; }
        }
        #endregion

        #region 運送区分
        public class UnsokbnViewData
        {

            public UnsokbnSerch Serch { get; set; }

            public UnsokbnInformation Information { get; set; }

        }

        public class UnsokbnSerch
        {

            [Display(Name = "コード")]
            [MaxLength(5, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Unskbn { get; set; }

            [Display(Name = "名称")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
            public string Unsknam { get; set; }

        }

        public class UnsokbnInformation
        {
            [Display(Name = "コード")]
            public string Unskbn { get; set; }

            [Display(Name = "名称")]
            public string Unsknam { get; set; }
        }
        #endregion

        #region 出荷注文書パターン
        public class TyuumonshoPatternViewData
        {

            public TyuumonshoPatternInformation Information { get; set; }

        }

        public class TyuumonshoPatternInformation
        {

            [Display(Name = "出荷Ｎｏ(3桁)")]
            public string Sykno2 { get; set; }

            [Display(Name = "コード")]
            public string Inscod { get; set; }

            [Display(Name = "配布先名")]
            public string Hainam { get; set; }

            [Display(Name = "配布枚数")]
            public int? Maisuu { get; set; }

            [Display(Name = "出荷Ｎｏ印刷フラグ")]
            public string Noprtf { get; set; }

            [Display(Name = "有効期限印刷フラグ")]
            public string Kigenf { get; set; }

            [Display(Name = "発注元会社名")]
            public string Htynam { get; set; }

            [Display(Name = "課補係長")]
            public string Ktykho { get; set; }

            [Display(Name = "担当者コード")]
            public string Tancod { get; set; }

            [Display(Name = "担当者")]
            public string Tannam { get; set; }

            [Display(Name = "発注元ＴＥＬ")]
            public string Ktytel { get; set; }

            [Display(Name = "現品保管場所")]
            public string Basyo { get; set; }

            [Display(Name = "運賃負担")]
            public string Ufutan { get; set; }

            [Display(Name = "経費負担Ｎｏ")]
            public string Keifno { get; set; }

            [Display(Name = "届先コード")]
            public string Tdkcod { get; set; }

            [Display(Name = "届先電話番号")]
            public string Tdktel { get; set; }

            [Display(Name = "届先住所")]
            public string Jdkjyu { get; set; }

            [Display(Name = "届先社名")]
            public string Tdknam { get; set; }

            [Display(Name = "届先支店名")]
            public string Tdsnam { get; set; }

            [Display(Name = "届先部課名")]
            public string Tdbnam { get; set; }

            [Display(Name = "届先担当者名")]
            public string Tdktan { get; set; }

            [Display(Name = "品名コード１")]
            public string Hincd1 { get; set; }

            [Display(Name = "品名コード２")]
            public string Hincd2 { get; set; }

            [Display(Name = "品名コード３")]
            public string Hincd3 { get; set; }

            [Display(Name = "品名コード４")]
            public string Hincd4 { get; set; }

            [Display(Name = "品名コード５")]
            public string Hincd5 { get; set; }

            [Display(Name = "品名１")]
            public string Hinnm1 { get; set; }

            [Display(Name = "品名２")]
            public string Hinnm2 { get; set; }

            [Display(Name = "品名３")]
            public string Hinnm3 { get; set; }

            [Display(Name = "品名４")]
            public string Hinnm4 { get; set; }

            [Display(Name = "品名５")]
            public string Hinnm5 { get; set; }

            [Display(Name = "出荷数１")]
            public decimal? Syksu1 { get; set; }

            [Display(Name = "出荷数２")]
            public decimal? Syksu2 { get; set; }

            [Display(Name = "出荷数３")]
            public decimal? Syksu3 { get; set; }

            [Display(Name = "出荷数４")]
            public decimal? Syksu4 { get; set; }

            [Display(Name = "出荷数５")]
            public decimal? Syksu5 { get; set; }

            [Display(Name = "特記事項")]
            public string Tkjiko { get; set; }

            [Display(Name = "得意先CD")]
            public string Tokcod { get; set; }

            [Display(Name = "運送方法コード")]
            public string Unscod { get; set; }

            [Display(Name = "運送コースコード")]
            public string Unscrs { get; set; }

            [Display(Name = "出荷場所CD")]
            public string Sybcod { get; set; }

            [Display(Name = "届先郵便番号")]
            public string Tdkyub { get; set; }

            [Display(Name = "伝票区分")]
            public string Denkbn { get; set; }

            [Display(Name = "帳票出力フラグ")]
            public string Lstflg { get; set; }

            [Display(Name = "届先マスタ使用フラグ")]
            public string Tdkhen { get; set; }

            [Display(Name = "登録依頼元")]
            public string Irimot { get; set; }

            [Display(Name = "登録依頼元ＴＥＬ")]
            public string Iritel { get; set; }

            [Display(Name = "登録担当")]
            public string Crtcod { get; set; }

            [Display(Name = "登録日")]
            public DateTime? Crtymd { get; set; }

            [Display(Name = "更新担当")]
            public string Updcod { get; set; }

            [Display(Name = "更新日")]
            public DateTime? Updymd { get; set; }

        }
        #endregion

    }
}