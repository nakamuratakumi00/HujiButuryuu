using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Macss.Areas.Fdass.ViewModels
{
    public class TightenViewModel
    {

        [Display(Name = "処理対象年月")]
        [MaxLength(7, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string YyyyMm { get; set; }

        public MatujimeKanriViewModel MatujimeKanri { get; set; }

    }
}