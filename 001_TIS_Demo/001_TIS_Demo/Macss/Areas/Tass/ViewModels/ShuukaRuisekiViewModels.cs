using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Macss.Areas.Tass.ViewModels
{
    public class ShuukaRuisekiViewModel
    {
        public int Insert1 { get; set; }
        public int Delete1 { get; set; }
        public int Insert2 { get; set; }
        public int Delete2 { get; set; }
        public int InsertR { get; set; }
        public int DeleteR { get; set; }

        [Display(Name = "月次期間終了年月")]
        [MaxLength(7, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string YyyyMm { get; set; }
    }
}