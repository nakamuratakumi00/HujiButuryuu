using System.ComponentModel.DataAnnotations;

namespace MacssWeb.ViewModels.Account
{
    public class LoginViewModel
    {

        public string LoginValidationMessage { get; set; }

        [Required(ErrorMessage = "{0}は必須です。")]
        [Display(Name = "ユーザーID")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

    }
}