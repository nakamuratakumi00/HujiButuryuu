using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Macss.Models;
using Macss.Areas.Fdass.Models;

using System.Web.Script.Serialization;
namespace Macss.Areas.Fdass.ViewModels
{
    public class EstimatesViewModel
    {

        public EstimatesViewModel() : this(new MSeikyusaki())
        {

        }

        public EstimatesViewModel(MSeikyusaki seikyusaki)
        {
            ModelSeikyu = seikyusaki;
            if (seikyusaki == null)
            {
                ModelSeikyu = new Macss.Models.MSeikyusaki();
            }
        }

        [Display(Name = "本社得意先コード")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE031")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Seicod { get => ModelSeikyu.Seicod; set => ModelSeikyu.Seicod = value; }

        [Display(Name = "得意先名称")]
       // public string Seinam { get; set; }
        public string Seinam { get => ModelSeikyu.Seinam; set => ModelSeikyu.Seinam = value; }


        [ScriptIgnore]
        //請求先コードマスタ
        public MSeikyusaki ModelSeikyu { get; set; }

    }

    public class EstimatesCKViewModel
    {
        public EstimatesCKViewModel() : this(new MHokanKeiyaku())
        {

        }

        public EstimatesCKViewModel(MHokanKeiyaku hokanKeiyaku)
        {
            ModelKeiyaku = hokanKeiyaku;
            if (hokanKeiyaku == null)
            {
                ModelKeiyaku = new Macss.Areas.Fdass.Models.MHokanKeiyaku();
            }
        }

        //本社得意先コード
        public string Fbtcod { get; set; }
        //保守請求用フラグ1
        public string Hokflg1 { get; set; }
        //荷役請求用フラグ1
        public string Nieflg1 { get; set; }
    
        //Fe保管請求契約マスタ
        public MHokanKeiyaku ModelKeiyaku { get; set; }

    }

    //機種A
    public class KisyuAExcelViewModel
    {
        public KisyuAExcelViewModel() : this(new MHokanKeiyaku())
        {
            
        }

        public KisyuAExcelViewModel(MHokanKeiyaku hokanKeiyaku)
        {
            ModelKeiyaku = hokanKeiyaku;
            if (hokanKeiyaku == null)
            {
                ModelKeiyaku = new Macss.Areas.Fdass.Models.MHokanKeiyaku();
            }
        }

        //本社得意先コード
        [Display(Name = "本社得意先コード")]
        public string Fbtcod { get; set; }
        //機種A 
        [Display(Name = "機種A")]
        public string Kisyua { get; set; }
        //機種名
        [Display(Name = "機種名")]
        public string Kisnam { get; set; }
        //請求対象
        [Display(Name = "請求対象")]
        public string Seitai { get; set; }
        //請求先名
        [Display(Name = "請求先名")]
        public string Seinam { get; set; }
        //保管請求用フラグ１
        [Display(Name = "保管請求単位")]
        public string Hokflg1 { get; set; }
        //荷役請求用フラグ１
        [Display(Name = "荷役請求単位")]
        public string Nieflg1 { get; set; }
        //保管請求用フラグ２
        [Display(Name = "保管単価単位")]
        public string Hokflg2 { get; set; }
        //荷役請求用フラグ２
        [Display(Name = "荷役単価単位")]
        public string Nieflg2 { get; set; }
        //保管請求用フラグ３
        [Display(Name = "保管請求体系")]
        public string Hokflg3 { get; set; }
        //荷役請求用フラグ３
        [Display(Name = "荷役請求体系")]
        public string Nieflg3 { get; set; }
        //保管値引率
        [Display(Name = "保管値引率")]
        public decimal? Hnebir { get; set; }
        //荷役値引率
        [Display(Name = "荷役値引率")]
        public decimal? Nnebir { get; set; }
        //保管単価
        [Display(Name = "保管単価")]
        public decimal? Hokant { get; set; }
        //荷役単価
        [Display(Name = "荷役単価")]
        public decimal? Nieant { get; set; }
        //荷役単価(休日用)
        [Display(Name = "荷役単価(休日用)")]
        public decimal? Niekyt { get; set; }
        public string Seiktani{ get; set; }
        public string Tanktani { get; set; }
         public string Seiktaik { get; set; }
        public decimal? nebk { get; set; }
        public decimal? Tanka { get; set; }
        public string HoksTani { get; set; }
        public string NiesTani { get; set; }
        public string HoktTani { get; set; }
        public string NietTani { get; set; }
        public string HoksTik { get; set; }
        public string NiesTik { get; set; }


        [ScriptIgnore]
        //Fe保管請求契約マスタ
        public MHokanKeiyaku ModelKeiyaku { get; set; }
    }

    //品番コード
    public class HinCodExcelViewModel
    {
        public HinCodExcelViewModel() : this(new MSeikyusaki(), new MHokanKeiyaku(), new MHokanTanka(), new MShukkabasho())
        {

        }

        public HinCodExcelViewModel(MSeikyusaki seikyusaki, MHokanKeiyaku hokanKeiyaku, MHokanTanka hokanTanka, MShukkabasho shukkaBasho)
        {
            ModelSeikyu = seikyusaki;
            if (seikyusaki == null)
            {
                ModelSeikyu = new Macss.Models.MSeikyusaki();
            }
            ModelKeiyaku = hokanKeiyaku;
            if (hokanKeiyaku == null)
            {
                ModelKeiyaku = new Macss.Areas.Fdass.Models.MHokanKeiyaku();
            }
            ModelTanka = hokanTanka;
            if (hokanTanka == null)
            {
                ModelTanka = new Macss.Areas.Fdass.Models.MHokanTanka();
                MShukkabasho = shukkaBasho;
                if (shukkaBasho == null)
                {
                    MShukkabasho = new Macss.Models.MShukkabasho();
                }
            }

        }

        //請求先コードマスタ
        public string Seicod { get; set; }
        //請求先名称
        public string Seinam { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //機種名
        public string Kisnam { get; set; }
        //保管請求用フラグ１
        public string Hokflg1 { get; set; }
        //荷役請求用フラグ１
        public string Nieflg1 { get; set; }
        //保管請求用フラグ２
        public string Hokflg2 { get; set; }
        //荷役請求用フラグ２
        public string Nieflg2 { get; set; }
        //保管請求用フラグ３
        public string Hokflg3 { get; set; }
        //荷役請求用フラグ３
        public string Nieflg3 { get; set; }
        //保管値引率
        public decimal? Hnebir { get; set; }
        //荷役値引率
        public decimal? Nnebir { get; set; }
        //荷役単価
        public decimal? Nieant { get; set; }
        //乙地従価率
        public decimal? Ojyukr { get; set; }
        //丙地従価率
        public decimal? Hjyukr { get; set; }
        //地収受率
        public decimal? Osyjyr { get; set; }
        //丙地収受率
        public decimal? Hsyjyr { get; set; }

        //-------明細-------
        //品番コード
        public string Hincod { get; set; }
        //品番型式
        public string Hinnmk { get; set; }
        //機種B
        public string Kisyub { get; set; }
        //保管場所コード
        public string Sybcod { get; set; }
        //保管場所
        public string Sybnam { get; set; }
        //振替区分
        public string Frikae { get; set; }
        //FP単価
        public decimal? Fptank { get; set; }
        //保管単価
        public decimal? Hokant { get; set; }
        //荷役単価
        public decimal? Niekit { get; set; }

        [ScriptIgnore]
        //請求先コードマスタ
        public MSeikyusaki ModelSeikyu { get; set; }
        //Fe保管請求契約マスタ
        public MHokanKeiyaku ModelKeiyaku { get; set; }
        //Fe保管請求単価マスタ
        public MHokanTanka ModelTanka { get; set; }
        //出荷場所マスタ
        public MShukkabasho MShukkabasho { get; set; }
    }



}