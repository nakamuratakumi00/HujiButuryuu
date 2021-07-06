using Macss.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.ViewModels
{
    public class ControlViewModel
    {
        public ControlViewModel() : this(new MControl())
        {

        }

        public ControlViewModel(MControl control)
        {
            Model = control;
            if (control == null)
            {
                Model = new Models.MControl();
            }
        }

        [Display(Name = "投稿日：投稿者")]
        public string InPostUser { get; set; }

        [Display(Name = "投稿日")]
        public string InPostDate { get; set; }

        [Display(Name = "掲載期限")]
        public string InDateFrom { get; set; }

        public string InDateTo { get; set; }

        [Display(Name = "投稿日：投稿者")]
        public string OutPostUser { get; set; }

        [Display(Name = "投稿日")]
        public string OutPostDate { get; set; }

        [Display(Name = "掲載期限")]
        public string OutDateFrom { get; set; }

        public string OutDateTo { get; set; }

        [Display(Name = "社内お知らせ")]
        [MaxLength(512, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string InInformation { get; set; }

        [Display(Name = "社外お知らせ")]
        [MaxLength(512, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string OutInformation { get; set; }

        [Display(Name = "更新日：更新担当")]
        public string PsPostUser { get; set; }

        [Display(Name = "投稿日")]
        public string PsPostDate { get; set; }

        [Display(Name = "パスワード桁数")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        public string PassKetaFrom { get; set; }

        [Display(Name = "パスワード桁数")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        public string PassKetaTo { get; set; }

        [Display(Name = "パスワード有効期限(月)")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        public string PassMonth { get; set; }

        [Display(Name = "パスワードミス許容回数")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        public string PassMiss { get; set; }

        [Display(Name = "パスワード世代管理")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        public string PassGene { get; set; }

        [Display(Name = "パスワード文字種類")]
        public bool PassType1 { get; set; }

        [Display(Name = "パスワード文字種類")]
        public bool PassType2 { get; set; }

        [Display(Name = "パスワード文字種類")]
        public bool PassType3 { get; set; }

        [Display(Name = "パスワード文字種類")]
        public bool PassType4 { get; set; }

        [ScriptIgnore]
        public MControl Model { get; set; }
    }
}