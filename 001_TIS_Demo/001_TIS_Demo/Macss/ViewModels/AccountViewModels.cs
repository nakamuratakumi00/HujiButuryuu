using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Macss.Attributes.Validation;
using Macss.Models;

namespace Macss.ViewModels
{

    public class AccountViewModel
    {
        public AccountViewModel() : this(new MAccount())
        {

        }

        public AccountViewModel(MAccount user)
        {
            Model = user;
            if (user == null)
            {
                Model = new Models.MAccount();
            }
        }

        [Display(Name = "ユーザーID")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string AccountId { get => Model.Id; set => Model.Id = value; }

        [Display(Name = "パスワード")]
        [MaxLength(32,ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [DataType(DataType.Password)]
        public string Password { get => Model.Password; set => Model.Password = value; }

        [Display(Name = "氏名(漢字／カナ)")]
        [Required(ErrorMessage = "氏名(漢字)は必須です。")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string AccountName { get => Model.UserName; set => Model.UserName = value; }

        [Display(Name = "氏名(カナ)")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]

        public string AccountNameKana { get => Model.AccountNameKana; set => Model.AccountNameKana = value; }

        [Display(Name = "部門")]
        //[Required(ErrorMessage = "部門コードは必須です。")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(4, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string BumonCd { get => Model.BumonCd; set => Model.BumonCd = value; }

        [Display(Name = "出荷場所")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string BasyoCd { get => Model.BasyoCd; set => Model.BasyoCd = value; }

        [Display(Name = "照会グループ")]
        //[Required(ErrorMessage = "照会グループは必須です。")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(6, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string GroupCd { get => Model.GroupCd; set => Model.GroupCd = value; }

        [Display(Name ="ロール")]
        public IEnumerable<string> Roles { get; set; }

        [Display(Name = "設定ロール")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public IEnumerable<string> SetRoles { get; set; }

        [Display(Name = "削除フラグ")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public bool DeleteFlg { get => (Model.DeleteFlg == 1) ? true : false; set => Model.DeleteFlg = (value == true ? 1 : 0); }

        [Display(Name = "ユーザーロック")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public bool UserLock { get => (Model.LoginFailureCount >= MaxFailureCount) ? true : false; set => Model.LoginFailureCount = (value == true ? 1000 : 0); }

        [Display(Name = "届先マスタ　使用区分_顧")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Sdek12 { get => Model.Sdek12; set => Model.Sdek12 = value; }

        [Display(Name = "品名マスタ　抽出フラグ")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Ctlfl1 { get => Model.Ctlfl1; set => Model.Ctlfl1 = value; }

        public int Mode { get; set; }

        public decimal? MaxFailureCount { get; set; } = 0;

        [ScriptIgnore]
        public MAccount Model { get; set; }
    }

    public class CreateAccountViewModel : AccountViewModel
    {
        public CreateAccountViewModel() : this(new MAccount())
        {

        }

        public CreateAccountViewModel(MAccount user) : base(user)
        {
        }


        [DataType(DataType.Password)]
        [Display(Name = "確認用パスワード")]
        [Required]
        public string ConfirmationPassword { get; set; } = String.Empty;
    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "現在のパスワード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string CurrentPassword { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "新規パスワード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string Password { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "確認用パスワード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string ConfirmationPassword { get; set; } = String.Empty;

    }

    public class ChangePasswordByAdminViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "新規パスワード")]
        [Required]
        public string Password { get; set; } = String.Empty;
    }

    public class AccountRoleViewModel
    {
        public string AccountId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

    }

    public class AccountInformation
    {
        [Display(Name = "ユーザーID")]
        public string AccountId { get; set; }

        [Display(Name = "氏名")]
        public string AccountName { get; set; }

        [Display(Name = "部門")]
        public string BumonCd { get; set; }

        [Display(Name = "出荷場所")]
        public string BasyoCd { get; set; }

        [Display(Name = "照会グループ")]
        public string GroupCd { get; set; }

        [Display(Name = "ロール")]
        public string Role { get; set; }

        [Display(Name = "削除フラグ")]
        public string DeleteFlg { get; set; }

        [Display(Name = "ユーザーロック")]
        public string RockFlg { get; set; }

        [Display(Name = "登録日時")]
        public String CreateDate { get; set; }

        [Display(Name = "更新日時")]
        public String UpdateDate { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

    }
}