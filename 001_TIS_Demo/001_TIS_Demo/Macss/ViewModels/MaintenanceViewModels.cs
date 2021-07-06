using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Macss.ViewModels
{
    public class MaintenanceViewModels
    {

        public MaintenanceViewModels() 
        {

        }
        
        public string Master { get; set; }

        public int Insert { get; set; }
        public int Update { get; set; }
        public int Delete { get; set; }
        public int Invalid { get; set; }

        [Display(Name = "対象年月")]
        [MaxLength(7, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string YyyyMm { get; set; }

        [Display(Name = "品番コード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(8, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Hincod { get; set; }
    }
}