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

    public class LogViewModel
    {

        [Display(Name = "処理日時")]
        [MaxLength(32)]
        public string ExcectionDate { get; set; }

        [Display(Name = "処理年月日")]
        public string ExcectionFrom { get; set; }
        public string ExcectionTo { get; set; }

        [Display(Name = "処理ユーザーID")]
        //[MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE113")]
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Display(Name = "出力対象機能")]
        [MaxLength(32)]
        public string ProcessingId { get; set; }
        public string RoleName { get; set; }

        public string[] ProcessingIdList { get; set; }

        [Display(Name = "処理内容")]
        [MaxLength(4)]
        public string Function { get; set; }
        public string FunctionName { get; set; }

        public string Purpose1 { get; set; }
        public string Purpose2 { get; set; }

        [Display(Name = "出力対象区分")]
        public string MenuName { get; set; }

    }
}