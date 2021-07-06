using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Macss.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "ユーザーID")]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }
    }
}