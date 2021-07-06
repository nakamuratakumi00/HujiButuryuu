using Macss.Areas.Tass.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.Areas.Tass.ViewModels
{

    public class TodokesakiViewModel
    {

        public TodokesakiViewModel() : this(new MUnsouTodokesakiKoyuu())
        {

        }

        public TodokesakiViewModel(MUnsouTodokesakiKoyuu todokesaki)
        {
            Model = todokesaki;
            if (todokesaki == null)
            {
                Model = new Models.MUnsouTodokesakiKoyuu();
            }
        }

        [Display(Name = "届先コード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tdkcod { get => Model.Tdkcod; set => Model.Tdkcod = value; }

        [Display(Name = "顧客届先コード")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Ktdkcd { get => Model.Ktdkcd; set => Model.Ktdkcd = value; }

        [Display(Name = "社名（漢字）")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdknam { get => Model.Tdknam; set => Model.Tdknam = value; }

        [Display(Name = "支店名")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdknms { get => Model.Tdknms; set => Model.Tdknms = value; }

        [Display(Name = "社名（カナ）")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tdknmk { get => Model.Tdknmk; set => Model.Tdknmk = value; }

        [Display(Name = "部課名")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdbnam { get => Model.Tdbnam; set => Model.Tdbnam = value; }

        [Display(Name = "担当者名")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdktan { get => Model.Tdktan; set => Model.Tdktan = value; }

        [Display(Name = "郵便番号")]
        [MaxLength(7, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        public string Yubinn { get => Model.Yubinn; set => Model.Yubinn = value; }

        [Display(Name = "電話番号")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[0-9-()]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE114")]
        public string Tdktel { get => Model.Tdktel; set => Model.Tdktel = value; }

        [Display(Name = "ＦＡＸ番号")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[0-9-()]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE114")]
        public string Faxno { get => Model.Faxno; set => Model.Faxno = value; }

        [Display(Name = "住所")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(60, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Jyusyo { get => Model.Jyusyo; set => Model.Jyusyo = value; }

        [Display(Name = "Ｈ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE118")]
        public string Sdek01 { get => Model.Sdek01; set => Model.Sdek01 = value; }

        [Display(Name = "ｉ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE119")]
        public string Sdek02 { get => Model.Sdek02; set => Model.Sdek02 = value; }

        [Display(Name = "Ｍ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE120")]
        public string Sdek03 { get => Model.Sdek03; set => Model.Sdek03 = value; }

        [Display(Name = "Ｐ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE121")]
        public string Sdek04 { get => Model.Sdek04; set => Model.Sdek04 = value; }

        [Display(Name = "集")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE122")]
        public string Sdek05 { get => Model.Sdek05; set => Model.Sdek05 = value; }

        [Display(Name = "構")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE123")]
        public string Sdek06 { get => Model.Sdek06; set => Model.Sdek06 = value; }

        [Display(Name = "Ｙ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE124")]
        public string Sdek07 { get => Model.Sdek07; set => Model.Sdek07 = value; }

        [Display(Name = "半")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE125")]
        public string Sdek08 { get => Model.Sdek08; set => Model.Sdek08 = value; }

        [Display(Name = "長")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE126")]
        public string Sdek09 { get => Model.Sdek09; set => Model.Sdek09 = value; }

        [Display(Name = "在")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE127")]
        public string Sdek10 { get => Model.Sdek10; set => Model.Sdek10 = value; }

        [Display(Name = "需")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE128")]
        public string Sdek11 { get => Model.Sdek11; set => Model.Sdek11 = value; }

        [Display(Name = "顧")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE129")]
        public string Sdek12 { get => Model.Sdek12; set => Model.Sdek12 = value; }

        [Display(Name = "得")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE130")]
        public string Sdek13 { get => Model.Sdek13; set => Model.Sdek13 = value; }

        [Display(Name = "ブ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE131")]
        public string Sdek14 { get => Model.Sdek14; set => Model.Sdek14 = value; }

        [Display(Name = "基")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE132")]
        public string Sdek15 { get => Model.Sdek15; set => Model.Sdek15 = value; }

        [Display(Name = "特記事項")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        [MaxLength(40, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tkjiko { get => Model.Tkjiko; set => Model.Tkjiko = value; }

        [Display(Name = "基幹届先コード")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Kitdcd { get => Model.Kitdcd; set => Model.Kitdcd = value; }

        [Display(Name = "データ管理元")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Dtmoto { get => Model.Dtmoto; set => Model.Dtmoto = value; }

        [Display(Name = "削除フラグ")]
        public string Delfkg { get; set; }

        public DateTime CrtymdDt { get; set; }
        [Display(Name = "登録日")]
        public string Crtymd { get; set; }

        public DateTime UpdymdDt { get; set; }
        [Display(Name = "更新日")]
        public string Updymd { get; set; }

        [Display(Name = "操作モード")]
        public string Mode { get; set; }

        [Display(Name = "採番FLG")]
        public int NumberingFlg { get; set; }

        [ScriptIgnore]
        public MUnsouTodokesakiKoyuu Model { get; set; }

    }

    public class TodokesakiData
    {

        public TodokesakiSerch Serch { get; set; }

        public TodokesakiInformation Information { get; set; }

        [Display(Name = "操作モード")]
        public string Mode { get; set; }

    }

    public class TodokesakiSerch
    {

        [Display(Name = "社名（カナ）")]
        [MaxLength(80, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdknmk { get; set; }

        [Display(Name = "届先コード")]
        public string Tdkcod { get; set; }

        [Display(Name = "顧客届先コード")]
        public string Ktdkcd { get; set; }

        [Display(Name = "社名")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdknam { get; set; }

        [Display(Name = "支店名")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdknms { get; set; }

        [Display(Name = "部課名")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdbnam { get; set; }

        [Display(Name = "担当者名")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdktan { get; set; }

        [Display(Name = "住所")]
        [MaxLength(60, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Jyusyo { get; set; }

        [Display(Name = "データ管理元")]
        public string Dtmoto { get; set; }

        [Display(Name = "検索FLG")]
        public int SearchFlg { get; set; }

        [Display(Name = "Ｈ")]
        public string Sdek01 { get; set; }

        [Display(Name = "ｉ")]
        public string Sdek02 { get; set; }

        [Display(Name = "Ｍ")]
        public string Sdek03 { get; set; }

        [Display(Name = "Ｐ")]
        public string Sdek04 { get; set; }

        [Display(Name = "集")]
        public string Sdek05 { get; set; }

        [Display(Name = "構")]
        public string Sdek06 { get; set; }

        [Display(Name = "Ｙ")]
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


    public class TodokesakiInformation
    {

        [Display(Name = "届先C")]
        public string Tdkcod { get; set; }

        [Display(Name = "顧客届先C")]
        public string Ktdkcd { get; set; }

        [Display(Name = "届先名")]
        public string Tdknam { get; set; }

        [Display(Name = "住所")]
        public string Jyusyo { get; set; }

        [Display(Name = "社名（カナ）")]
        public string Tdknmk { get; set; }

        [Display(Name = "管")]
        public string Dtmoto { get; set; }

        public string Del { get; set; }
        public string Tdknms { get; set; }
        public string Tdbnam { get; set; }
        public string Tdktan { get; set; }
    }

    public class TodokesakiFileInformation
    {
        public string TDKCOD { get; set; }

        public string TDKNAM { get; set; }

        public string TDKNMS { get; set; }

        public string TDKNMK { get; set; }

        public string TDBNAM { get; set; }

        public string TDKTAN { get; set; }

        public string JYUSYO { get; set; }

        public string TDKTEL { get; set; }

        public string SDEK01 { get; set; }

        public string SDEK02 { get; set; }

        public string SDEK03 { get; set; }

        public string SDEK04 { get; set; }

        public string SDEK05 { get; set; }

        public string SDEK06 { get; set; }

        public string SDEK07 { get; set; }

        public string SDEK08 { get; set; }

        public string SDEK09 { get; set; }

        public string SDEK10 { get; set; }

        public string SDEK11 { get; set; }

        public string SDEK12 { get; set; }

        public string SDEK13 { get; set; }

        public string SDEK14 { get; set; }

        public string SDEK15 { get; set; }

        public string TKJIKO { get; set; }

        public string DTMOTO { get; set; }

        public string YUBINN { get; set; }

        public string FAXNO { get; set; }

        public string KTDKCD { get; set; }

        public string KITDCD { get; set; }

        public string DELFLG { get; set; }

        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }

    }

}