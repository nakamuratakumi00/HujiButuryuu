using System.ComponentModel.DataAnnotations;

namespace Macss.Areas.Fdass.ViewModels
{
    public class SeihinCaptureViewModel
    {

        [Display(Name = "元データ件数")]
        public int Moto { get; set; }

        [Display(Name = "取り込み件数")]
        public int Torikomi { get; set; }

        [Display(Name = "エラー件数")]
        public int Error { get; set; }


        public int Insert { get; set; }
        public int Update { get; set; }

    }
}