using Macss.Attributes.Custom;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_todokesaki_koyuu")]
    public class MUnsouTodokesakiKoyuu
    {

        [Description("届先コード")]
        [Column("TDKCOD", Order = 0)]
        [Required]
        [FileDoubleRequired]
        [Key]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tdkcod { get; set; }

        [Description("届先名社名（漢字）")]
        [Column("TDKNAM")]
        [Required]
        [FileRequired]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdknam { get; set; }

        [Description("届先名支店名（漢字）")]
        [Column("TDKNMS")]
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdknms { get; set; }

        [Description("届先名社名（カナ）")]
        [Column("TDKNMK")]
        [Required]
        [FileRequired]
        [MaxLength(80)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tdknmk { get; set; }

        [Description("届先部課名")]
        [Column("TDBNAM")]
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdbnam { get; set; }

        [Description("届先担当者名")]
        [Column("TDKTAN")]
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdktan { get; set; }

        [Description("届先住所（漢字）")]
        [Column("JYUSYO")]
        [Required]
        [FileRequired]
        [MaxLength(60)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Jyusyo { get; set; }

        [Description("届先電話番号")]
        [Column("TDKTEL")]
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[0-9-() ]+")]
        public string Tdktel { get; set; }

        [Description("使用区分_Ｈ")]
        [Column("SDEK01")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek01 { get; set; }

        [Description("使用区分_ｉ")]
        [Column("SDEK02")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek02 { get; set; }

        [Description("使用区分_Ｍ")]
        [Column("SDEK03")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek03 { get; set; }

        [Description("使用区分_Ｐ")]
        [Column("SDEK04")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek04 { get; set; }

        [Description("使用区分_集")]
        [Column("SDEK05")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek05 { get; set; }

        [Description("使用区分_構")]
        [Column("SDEK06")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek06 { get; set; }

        [Description("使用区分_Ｙ")]
        [Column("SDEK07")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek07 { get; set; }

        [Description("使用区分_半")]
        [Column("SDEK08")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek08 { get; set; }

        [Description("使用区分_長")]
        [Column("SDEK09")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek09 { get; set; }

        [Description("使用区分_在")]
        [Column("SDEK10")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek10 { get; set; }

        [Description("使用区分_需")]
        [Column("SDEK11")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek11 { get; set; }

        [Description("使用区分_顧")]
        [Column("SDEK12")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek12 { get; set; }

        [Description("使用区分_得")]
        [Column("SDEK13")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek13 { get; set; }

        [Description("使用区分_ブ")]
        [Column("SDEK14")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek14 { get; set; }

        [Description("使用区分_基")]
        [Column("SDEK15")]
        [Required]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sdek15 { get; set; }

        [Description("特記事項")]
        [Column("TKJIKO")]
        [Required]
        [MaxLength(40)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tkjiko { get; set; }

        [Description("データ管理元")]
        [Column("DTMOTO")]
        [Required]
        [FileDoubleRequired]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Dtmoto { get; set; }

        [Description("郵便番号")]
        [Column("YUBINN")]
        [Required]
        [MaxLength(10)]
        [RegularExpression(@"[0-9 ]+")]
        public string Yubinn { get; set; }

        [Description("ＦＡＸ番号")]
        [Column("FAXNO")]
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"[0-9-() ]+")]
        public string Faxno { get; set; }

        [Description("顧客届先コード")]
        [Column("KTDKCD")]
        [Required]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Ktdkcd { get; set; }

        [Description("基幹届先コード")]
        [Column("KITDCD")]
        [Required]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Kitdcd { get; set; }

        [Description("削除フラグ")]
        [Column("DELFLG")]
        [Required]
        [MaxLength(1)]
        public string Delflg { get; set; }

        [Description("登録担当")]
        [Column("CRTCOD")]
        [Required]
        [MaxLength(8)]
        public string Crtcod { get; set; }

        [Description("登録日")]
        [Column("CRTYMD")]
        [Required]
        public DateTime? Crtymd { get; set; }

        [Description("更新担当")]
        [Column("UPDCOD")]
        [Required]
        [MaxLength(8)]
        public string Updcod { get; set; }

        [Description("更新日")]
        [Column("UPDYMD")]
        [Required]
        public DateTime? Updymd { get; set; }

    }

}