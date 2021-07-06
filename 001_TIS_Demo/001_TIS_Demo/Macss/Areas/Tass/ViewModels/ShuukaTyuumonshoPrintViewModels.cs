using Macss.Areas.Tass.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.Areas.Tass.ViewModels
{

    //画面初期情報
    public class DispData
    {
        [Display(Name = "アカウントID")]
        public string Id { get; set; }
        [Display(Name = "照会グループ")]
        public string GroupCd { get; set; }
        [Display(Name = "出荷日")]
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string Sybcod { get; set; }
    }

    #region 出荷注文書印刷
    public class ShuukaTyuumonshoPrintViewData
        {
            [Display(Name = "アカウントID")]
            public string Id { get; set; }
            [Display(Name = "照会グループ")]
            public string GroupCd { get; set; }
            [Display(Name = "出荷日")]
            public string DateFrom { get; set; }
            public string DateTo { get; set; }

            public ShuukaTyuumonshoPrintSerch Serch { get; set; }

            public ShuukaTyuumonshoPrintInformation Information { get; set; }

            public ckData CkData { get; set; }
            public ShuukaTyuumonshoPrintData PrintData { get; set; }

    }

    //検索情報
    public class ShuukaTyuumonshoPrintSerch
    {
        [Display(Name = "発行状況")]
        public string Jokyo { get; set; }

        [Display(Name = "未発行")]
        public bool Mihakkou { get; set; }
        [Display(Name = "発行済")]
        public bool Hakousumi { get; set; }
        [Display(Name = "発行区分")]
        public string Hkukbn { get; set; }
        [Display(Name = "発行ステータス")]
        public bool Hstatus { get; set; }

        [Display(Name = "最終発行日時")]
        public string Hkuymd { get; set; }

        [Display(Name = "出荷場所コード")]
        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sybcod { get; set; }

        [Display(Name = "登録担当者名")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tannam { get; set; }

        [Display(Name = "出荷日")]
        public string DateFrom { get; set; }
        [Display(Name = "出荷日To")]
        public string DateTo { get; set; }

        [Display(Name = "出荷No")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Syukno { get; set; }

        [Display(Name = "得意先コード")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tokcod { get; set; }

        [Display(Name = "発行日")]
        public string PrintFrom { get; set; }
        public string PrintTo { get; set; }

        [Display(Name = "発行者")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string UserName { get; set; }

        [Display(Name = "アカウントID")]
        public string Id { get; set; }

        [Display(Name = "照会グループ")]
        public string GroupCd { get; set; }


    }
    //結果一覧
    public class ShuukaTyuumonshoPrintInformation
    {
        [Display(Name = "印")]
        public bool Ckprint { get; set; }
        
        [Display(Name = "発")]
        public string Hstatus { get; set; }

        [Display(Name = "最終発行日時(非表示 変換用)")]
        public DateTime THkuymd { get; set; }

        [Display(Name = "最終発行日")]
        public string Hkuymd { get; set; }

        [Display(Name = "発行者")]
        public string Hknam { get; set; }

        [Display(Name = "出荷日(非表示 変換用)")]
        public DateTime TSyukdt { get; set; }

        [Display(Name = "出荷日")]
        public string  Syukdt { get; set; }

        [Display(Name = "出荷No")]//出荷場所コードのこと
        public string Syukno { get; set; }

        [Display(Name = "作成日")]
        public string Cdate { get; set; }

        [Display(Name = "場")]
        public string Sybcod { get; set; }

        [Display(Name = "届先名(非表示 変換用)")]
        public string TTdkjyu { get; set; }

        [Display(Name = "届先名")]
        public string Todonam { get; set; }

        [Display(Name = "品名")]
        public string Dhinnam { get; set; }

        [Display(Name = "得意先C")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        public string Tornam { get; set; }

        [Display(Name = "登録担当")]
        public string Updcod { get; set; }

    }

    //印刷更新
    public class ckData
    {
        [Display(Name = "発行")]
        public string Hstatus { get; set; }

        [Display(Name = "出荷No")]
        public string Sybcod { get; set; }

    }

    //帳票出力
    public class ShuukaTyuumonshoPrintData
    {
        //各ヘッダー
        [Display(Name = "発送扱者")]
        public string Hsatukai { get; set; }
        [Display(Name = "再発行フラグ")]
        public string Denf { get; set; }
        [Display(Name = "発行時間(変換用)")]
        public DateTime THkuymd { get; set; }
        [Display(Name = "発行時間")]
        public string Hkuymd { get; set; }
        [Display(Name = "ページ番号")]
        public string page { get; set; }
        [Display(Name = "発送扱者郵便番号")]
        public string hsyubno { get; set; }
        [Display(Name = "発送扱者電話番号")]
        public string hstel { get; set; }
        [Display(Name = "発送扱者住所")]
        public string hsjyusyo { get; set; }

        //明細
        [Display(Name = "発送元(会社名)")]
        public string Htynam { get; set; }
        [Display(Name = "課補・係長")]
        public string Htykah { get; set; }
        [Display(Name = "担当者")]
        public string Tannam { get; set; }
        [Display(Name = "電話番号")]
        public string Htytel { get; set; }
        [Display(Name = "出荷No")]
        public string Syukno { get; set; }
        [Display(Name = "出荷No(バーコード用)")]
        public string Bacode { get; set; }
        [Display(Name = "枝番/総枝番")]
        public string Edaban { get; set; }
        [Display(Name = "現品保管場所")]
        public string Basyo { get; set; }
        [Display(Name = "運賃負担コード")]
        public string Ufutan { get; set; }
        [Display(Name = "運賃負担名")]
        public string Ufutanmei { get; set; }
        [Display(Name = "経費負担No")]
        public string Keifno { get; set; }
        [Display(Name = "振替出荷No")]
        public string Fsykno { get; set; }
        [Display(Name = "出荷日(変換用)")]
        public DateTime TSykymd { get; set; }
        [Display(Name = "出荷日")]
        public string Sykymd { get; set; }
        [Display(Name = "お届け先郵便番号")]
        public string Tdkyub { get; set; }
        [Display(Name = "お届け先電話番号")]
        public string Tdktel { get; set; }
        [Display(Name = "お届け先コード")]
        public string Tdkcod { get; set; }
        [Display(Name = "お届け先住所(変換用)")]
        public string TTdkjyu { get; set; }
        [Display(Name = "お届け先住所１")]
        public string Tdkjyu1 { get; set; }
        [Display(Name = "お届け先住所２")]
        public string Tdkjyu2 { get; set; }
        [Display(Name = "お届け先名(変換用)")]
        public string TTdknam { get; set; }
        [Display(Name = "お届け先名１")]
        public string Tdknam1 { get; set; }
        [Display(Name = "お届け先名２")]
        public string Tdknam2 { get; set; }
        [Display(Name = "お届け先名３")]
        public string Tdknam3 { get; set; }
        [Display(Name = "品名コード１")]
        public string Hincod1 { get; set; }
        [Display(Name = "品名・型式・寸法・図番１")]
        public string Hinnam1 { get; set; }
        [Display(Name = "出荷数１")]
        public decimal? Syuksu1 { get; set; }
        [Display(Name = "品名コード２")]
        public string Hincod2 { get; set; }
        [Display(Name = "品名・型式・寸法・図番２")]
        public string Hinnam2 { get; set; }
        [Display(Name = "出荷数２")]
        public decimal? Syuksu2 { get; set; }
        [Display(Name = "品名コード３")]
        public string Hincod3 { get; set; }
        [Display(Name = "品名・型式・寸法・図番３")]
        public string Hinnam3 { get; set; }
        [Display(Name = "出荷数３")]
        public decimal? Syuksu3 { get; set; }
        [Display(Name = "品名コード４")]
        public string Hincod4 { get; set; }
        [Display(Name = "品名・型式・寸法・図番４")]
        public string Hinnam4 { get; set; }
        [Display(Name = "出荷数４")]
        public decimal? Syuksu4 { get; set; }
        [Display(Name = "品名コード５")]
        public string Hincod5 { get; set; }
        [Display(Name = "品名・型式・寸法・図番５")]
        public string Hinnam5 { get; set; }
        [Display(Name = "出荷数５")]
        public decimal? Syuksu5 { get; set; }
        [Display(Name = "特記事項(変換用)")]
        public string TTkjiko { get; set; }
        [Display(Name = "特記事項１")]
        public string Tkjiko1 { get; set; }
        [Display(Name = "特記事項２")]
        public string Tkjiko2 { get; set; }
        [Display(Name = "コメント")]
        public string Coment { get; set; }
        [Display(Name = "得意先コード")]
        public string Tokcod { get; set; }
        [Display(Name = "出荷場所コード")]
        public string Sybcod { get; set; }
        [Display(Name = "運送方法コード")]
        public string Unscod { get; set; }
        [Display(Name = "運送コースコード")]
        public string Unscrs { get; set; }

    }

    #endregion

}