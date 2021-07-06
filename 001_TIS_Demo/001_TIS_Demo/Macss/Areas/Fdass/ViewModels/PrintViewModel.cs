using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Macss.Models;
using Macss.Areas.Fdass.Models;

namespace Macss.Areas.Fdass.ViewModels
{
    public class PrintViewModel
    {

        [Display(Name = "処理対象年月")]
        public string Month { get; set; }

        [Display(Name = "実行日時")]
        public string StartTt { get; set; }

        [Display(Name = "処理ステータス")]
        public string Status { get; set; }

        [Display(Name = "実行者")]
        public string CrtName { get; set; }

        [Display(Name = "Fe保管請求PCコードデータ作成エラーリスト")]
        public bool PcCodeErrorList { get; set; }
        [Display(Name = "Fe保管請求請求漏れ機種確認リスト")]
        public bool KisyuMoreList { get; set; }
        [Display(Name = "Fe入出庫繰越データリスト")]
        public bool NyuSyukoKurikosiList { get; set; }
        [Display(Name = "Fe保管請求請求先別集計表")]
        public bool SekiyusakiShukeiList { get; set; }
        [Display(Name = "倉庫料明細書（品番コード単位）")]
        public bool SoukoHinCodList { get; set; }
        [Display(Name = "倉庫料明細書（機種A単位）")]
        public bool SoukoKisyuAList{ get; set; }
        [Display(Name = "倉庫料明細書（倉庫使用料単位）")]
        public bool SoukoSiyouryoList { get; set; }
        [Display(Name = "Fe保管請求拠点別データ")]
        public bool KyotenbetuList { get; set; }
        [Display(Name = "FPS様保管請求拠点別データ")]
        public bool FPSKyotenbetuList { get; set; }

        public DateTime StartDateTime { get; set; }
    }

    public class PcCodeErrorListPrint
    {
        public PcCodeErrorListPrint() : this(new THokanSeikyu())
        {

        }

        public PcCodeErrorListPrint(THokanSeikyu tHokanSeikyu)
        {
            TSeikyu = tHokanSeikyu;
            if (tHokanSeikyu == null)
            {
                TSeikyu = new Macss.Areas.Fdass.Models.THokanSeikyu();
            }
        }

        //請求先コード
        public string Seicod { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //PCコード
        public string Pccod { get; set; }
        //機種B
        public string Kisyub { get; set; }
        //保管場所
        public string Hokcod { get; set; }
        //保管料
        public decimal? Hokank { get; set; }
        //保管料差額
        public decimal? HokankS { get; set; }
        //荷役料
        public decimal? Niekik { get; set; }
        //荷役料差額
        public decimal? NiekikS { get; set; }
        //識別
        public string Shikibetu { get; set; }
        //金額 保管
        public decimal? HKingaku { get; set; }
        //金額 荷役
        public decimal? NKingaku { get; set; }
        //金額
        public decimal? Kingaku { get; set; }
        //エラー内容
        public string Errorstr { get; set; }
        //Fe保管請求請求データ
        public THokanSeikyu TSeikyu { get; set; }

    }
    
    public class KisyuMoreList
    {
        public KisyuMoreList() : this(new THokanNyuushuuko())
        {

        }
        public KisyuMoreList(THokanNyuushuuko tHokanNyuushuuko)
        {
            TNyuushuuko = tHokanNyuushuuko;
            if (tHokanNyuushuuko == null)
            {
                TNyuushuuko = new Macss.Areas.Fdass.Models.THokanNyuushuuko();
            }
        }
        //機種A
        public string Kisyua { get; set; }
        //機種B
        public string Kisyub { get; set; }
        //品番コード
        public string Hincod { get; set; }
        //保管場所
        public string Hokcod { get; set; }
        //前月繰越
        public decimal? Zansuu { get; set; }
        //仕入倉入数
        public decimal? Siksuu { get; set; }
        //改造倉入数
        public decimal? Kaisuu { get; set; }
        //出庫数
        public decimal? Syksuu { get; set; }
        //Fe入出庫データ
        public THokanNyuushuuko TNyuushuuko { get; set; }
    }

    public class NyuSyukoKurikosiList
    {
        //担当部署
        public string Tanbsy { get; set; }
        //品番コード
        public string Hincod { get; set; }
        //保管場所
        public string Hokcod { get; set; }
        //保管場所名
        public string Sybnam { get; set; }
        // 機種A
        public string Kisyua { get; set; }
        //機種B
        public string Kisyub { get; set; }
        //前月繰越
        public decimal? Zansuu { get; set; }
        //仕入倉入数
        public decimal? Siksuu { get; set; }
        //改造倉入数
        public decimal? Kaisuu { get; set; }
        //入出庫数
        public decimal? Nskosu { get; set; }
        //繰越年月日
        public string Kurymd { get; set; }
    }

    public class SekiyusakiShukeiList
    {
        public SekiyusakiShukeiList() : this(new THokanSeikyu())
        {

        }
        public SekiyusakiShukeiList(THokanSeikyu tHokanSeikyu)
        {
            THokanSeikyu = tHokanSeikyu;
            if (tHokanSeikyu == null)
            {
                THokanSeikyu = new Macss.Areas.Fdass.Models.THokanSeikyu();
            }
        }

        //請求先
        public string Seicod { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //機種名
        public string Kisnam { get; set; }
        //値引前保管料
        public decimal? HOKANSUM { get; set; }
        //値引前荷役料
        public decimal? NIYAKUSUM { get; set; }
        //値引前合計
        public decimal? beNEBIKISUM { get; set; }
        //保管料値引
        public decimal? Hokannbk { get; set; }
        //荷役料値引
        public decimal? Niekinbk { get; set; }
        //値引合計
        public decimal? afNEBIKISUM { get; set; }
        //保管料
        public decimal? Hokank { get; set; }
        //荷役料
        public decimal? Niekik { get; set; }
        //合計
        public decimal? goukei { get; set; }
        //Fe保管請求請求データ
        public THokanSeikyu THokanSeikyu { get; set; }
    }

    //Fe寄託保管請求別データ
    public class KyotenbetuList
    {
        public string Basyo { get; set; }
        public string Basnam { get; set; }
        public string Kisyua { get; set; }
        public string Kisyub { get; set; }
        public string Kisybn { get; set; }
        public string Seicod { get; set; }
        public decimal? Zansuu { get; set; }
        public decimal? Zankin { get; set; }
        public decimal? Nyuksu { get; set; }
        public decimal? Nyukin { get; set; }
        public decimal? Syksuu { get; set; }
        public decimal? Sykkin { get; set; }
        public decimal? Zaiksu { get; set; }
        public decimal? Zaikin { get; set; }
        public decimal? Densuu { get; set; }
        public decimal? Densky { get; set; }
        public decimal? Hokank { get; set; }
        public decimal? Niekik { get; set; }
        public decimal? Niekyj { get; set; }
        public string Pccodh { get; set; }
        public string Pccodn { get; set; }
        public string Dataym { get; set; }

    }

    public class SoukoKisyuAList
    {
        //-------ヘッダー-------
        //FB本社得意先コード
        public string Torcod { get; set; }
        //取引先名１
        public string Tornam1 { get; set; }
        //取引先名2
        public string Tornam2 { get; set; }
        //請求コード
        public string Seicod { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //機種名
        public string Kisnam { get; set; }
        //数量説明
        public string Suryoum { get; set; }
        //請求コード単位の総計数
        public int Cntpg { get; set; }
        //請求コード単位の元枚数
        public int Maxpg { get; set; }

        //-------明細-------
        //機種B
        public string Kisyub { get; set; }
        //品名(合計行のみ使用)
        public string Hinnam { get; set; }
        //前月残数
        public decimal? Zansuu { get; set; }
        //入庫数
        public decimal? Nyuksu { get; set; }
        //積数
        public decimal? Sekisu { get; set; }
        //保管単価
        public decimal? Hokant { get; set; }
        //保管料
        public decimal? Hokank { get; set; }
        //荷役単価
        public decimal? Niekit { get; set; }
        //荷役料
        public decimal? Niekik { get; set; }
        //数量
        public decimal? Suryou { get; set; }
        //合計
        public decimal? Goukei { get; set; }

        //保管フラグ1
        public string Hokflg1 { get; set; }
        //荷役フラグ1
        public string Nieflg1 { get; set; }
        //保管コード
        public string Hokcod { get; set; }

    }

    public class SoukoHinCodList
    {
        //-------ヘッダー-------
        //FB本社得意先コード
        public string Torcod { get; set; }
        //取引先名１
        public string Tornam1 { get; set; }
        //取引先名2
        public string Tornam2 { get; set; }
        //請求コード
        public string Seicod { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //機種名
        public string Kisnam { get; set; }
        //数量説明
        public string Suryoum { get; set; }

        //-------明細-------
        //機種B
        public string Kisyub { get; set; }
        //前月残数
        public decimal? Zansuu { get; set; }
        //品名型式
        public string Hinnmk { get; set; }
        //品番コード$説明
        public string Hincod { get; set; }
        //入庫数
        public decimal? Nyuksu { get; set; }
        //積数
        public decimal? Sekisu { get; set; }
        //保管単価
        public decimal? Hokant { get; set; }
        //保管料
        public decimal? Hokank { get; set; }
        //荷役単価
        public decimal? Niekit { get; set; }
        //荷役料
        public decimal? Niekik { get; set; }
        //数量
        public decimal? Suryou { get; set; }

        //合計
        public decimal? Goukei { get; set; }

        //----CK用----
        //保管フラグ1
        public string Hokflg1 { get; set; }
        //荷役フラグ1
        public string Nieflg1 { get; set; }
        //保管コード
        public string Hokcod { get; set; }
        //請求コード単位の総計数
        public int Cntpg { get; set; }
        //請求コード単位の元枚数
        public int Maxpg { get; set; }
    }

    public class SoukoSiyouryoList
    {
        //-------ヘッダー-------
        //FB本社得意先コード
        public string Torcod { get; set; }
        //取引先名１
        public string Tornam1 { get; set; }
        //取引先名2
        public string Tornam2 { get; set; }
        //請求コード
        public string Seicod { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //機種名
        public string Kisnam { get; set; }
        //数量説明
        public string Suryoum { get; set; }

        //-------明細-------
        //前月残数
        public decimal? Zansuu { get; set; }
        //入庫数
        public decimal? Nyuksu { get; set; }
        //面積(坪)：積数
        public decimal? Sekisu { get; set; }
        //坪単価：保管単価
        public decimal? Hokant { get; set; }
        //使用料：保管料
        public decimal? Hokank { get; set; }
        //荷役単価
        public decimal? Niekit { get; set; }
        //荷役料
        public decimal? Niekik { get; set; }
        //数量
        public decimal? Suryou { get; set; }
        //品番コード$説明
        public string HincodStr { get; set; }

        //合計
        public decimal? Goukei { get; set; }
        //請求コード単位の総計数
        public int Cntpg { get; set; }
        //請求コード単位の元枚数
        public int Maxpg { get; set; }
    }

}