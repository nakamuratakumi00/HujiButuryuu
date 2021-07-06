using Macss.Areas.Tass.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.Areas.Tass.ViewModels
{

    public class HinmeiViewModel
    {

        public HinmeiViewModel() : this(new MUnsouHinmeiKoyuu())
        {

        }

        public HinmeiViewModel(MUnsouHinmeiKoyuu hinmei)
        {
            Model = hinmei;
            if (hinmei == null)
            {
                Model = new Models.MUnsouHinmeiKoyuu();
            }
        }

        [Display(Name = "品名コード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Hincod { get => Model.Hincod; set => Model.Hincod = value; }

        [Display(Name = "顧客品名コード")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Khincd { get => Model.Khincd; set => Model.Khincd = value; }

        [Display(Name = "品名")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Hinnam { get => Model.Hinnam; set => Model.Hinnam = value; }

        [Display(Name = "品名カナ")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Hinnmk { get => Model.Hinnmk; set => Model.Hinnmk = value; }

        [Display(Name = "得意先コード")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Torcod { get => Model.Torcod; set => Model.Torcod = value; }

        [Display(Name = "得意先名")]
        public string Tornam { get; set; }

        [Display(Name = "機種A")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Kisyua { get => Model.Kisyua; set => Model.Kisyua = value; }

        [Display(Name = "機種B")]
        [MaxLength(6, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Kisyub { get => Model.Kisyub; set => Model.Kisyub = value; }

        [Display(Name = "単価コード")]
        [MaxLength(5, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tkcod { get => Model.Tkcod; set => Model.Tkcod = value; }

        [Display(Name = "種別")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Syubtu { get => Model.Syubtu; set => Model.Syubtu = value; }

        [Display(Name = "抽出フラグ")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Ctlfl1 { get => Model.Ctlfl1; set => Model.Ctlfl1 = value; }

        [Display(Name = "データ管理元")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Dtmoto { get => Model.Dtmoto; set => Model.Dtmoto = value; }

        [Display(Name = "削除フラグ")]
        public string Delfkg { get; set; }

        public DateTime CrtymdDt { get; set; }
        [Display(Name = "登録日")]
        public string Crtymd { get ; set ; }

        public DateTime UpdymdDt { get; set; }
        [Display(Name = "更新日")]
        public string Updymd { get; set; }

        [Display(Name = "操作モード")]
        public string Mode { get; set; }

        [Display(Name = "採番FLG")]
        public int NumberingFlg { get; set; }

        [ScriptIgnore]
        public MUnsouHinmeiKoyuu Model { get; set; }

    }


    public class HinmeiData
    {

        public HinmeiSerch Serch { get; set; }

        public HinmeiInformation Information { get; set; }

        [Display(Name = "操作モード")]
        public string Mode { get; set; }

    }


    public class HinmeiSerch
    {

        [Display(Name = "品名カナ")]
        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Hinnmk { get; set; }

        [Display(Name = "品名コード")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Hincod { get; set; }

        [Display(Name = "顧客品名コード")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Khincd { get; set; }

        [Display(Name = "品名")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Hinnam { get; set; }

        [Display(Name = "抽出フラグ")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Ctlfl1 { get; set; }

        [Display(Name = "データ管理元")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Dtmoto { get; set; }

        [Display(Name = "検索FLG")]
        public int SearchFlg { get; set; }

        [Display(Name = "担当者選択")]
        public string CuCodCh { get; set; }

        [Display(Name = "担当者")]
        public string CuName { get; set; }

        [Display(Name = "削除済のみ表示")]
        public bool Del { get; set; }

        [Display(Name = "日選択")]
        public string CuDateCh { get; set; }

        [Display(Name = "日付From")]
        public string CuFrom { get; set; }

        [Display(Name = "時分From")]
        public string CuTFrom { get; set; }

        [Display(Name = "日付日To")]
        public string CuTo { get; set; }

        [Display(Name = "時分To")]
        public string CuTTo { get; set; }

    }


    public class HinmeiInformation
    {
        [Display(Name = "抽")]
        public string Ctlfl1 { get; set; }

        [Display(Name = "品名C")]
        public string Hincod { get; set; }

        [Display(Name = "顧客品名C")]
        public string Khincd { get; set; }

        [Display(Name = "品名")]
        public string Hinnam { get; set; }

        [Display(Name = "品名カナ")]
        public string Hinnmk { get; set; }

        [Display(Name = "管")]
        public string Dtmoto { get; set; }

        public string Del { get; set; }

    }

    public class HinmeiFileInformation
    {
        public string HINCOD { get; set; }

        public string HINNAM { get; set; }

        public string HINNMK { get; set; }

        public string TORCOD { get; set; }

        public string KISYUA { get; set; }

        public string KISYUB { get; set; }

        public string DTMOTO { get; set; }

        public string TKCOD { get; set; }

        public string SYUBTU { get; set; }

        public string CTLFL1 { get; set; }

        public string KHINCD { get; set; }

        public string CRTCOD { get; set; }

        public string DELFLG { get; set; }

        public DateTime? CRTYMD { get; set; }

        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}