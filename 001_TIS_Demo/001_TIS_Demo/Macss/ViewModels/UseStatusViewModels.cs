using Macss.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.ViewModels
{
    public class UseStatusViewModel
    {

        public UseStatusViewModel() : this(new TUseStatus())
        {
        }

        public UseStatusViewModel(TUseStatus useStatus)
        {
            Model = useStatus;
            if (useStatus == null)
            {
                Model = new Models.TUseStatus();
            }
        }

        [Display(Name = "ユーザーID")]
        public string AccountId { get; set; }

        [Display(Name = "氏名")]
        public string AccountName { get; set; }

        [Display(Name = "部門")]
        public string BumonCd { get; set; }

        [Display(Name = "機能")]
        public string TitleName { get; set; }

        [Display(Name = "最終操作日時")]
        public String UpdateDate { get; set; }

        public DateTime UpdateDateTime { get; set; }

        [ScriptIgnore]
        public TUseStatus Model { get; set; }

    }
}