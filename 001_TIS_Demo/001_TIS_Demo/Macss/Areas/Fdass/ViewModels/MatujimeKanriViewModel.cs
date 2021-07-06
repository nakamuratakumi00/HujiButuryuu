using System;
using System.ComponentModel.DataAnnotations;

namespace Macss.Areas.Fdass.ViewModels
{

    public class MatujimeKanriViewModel
    {
        [Display(Name = "前回処理対象年月")]
        public string Month { get; set; }

        [Display(Name = "前回実行日時")]
        public string StartTt { get; set; }

        [Display(Name = "前回実行日時")]
        public string EndTt { get; set; }

        [Display(Name = "前回処理ステータス")]
        public string Status { get; set; }

        [Display(Name = "前回実行者")]
        public string CrtName { get; set; }


        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

    }
}