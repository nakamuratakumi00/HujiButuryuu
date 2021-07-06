using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Macss.Models;
using Macss.Areas.Fdass.Models;
namespace Macss.Areas.Fdass.ViewModels
{
    public class SeihinResultVierModel
    {
        [Display(Name = "処理対象年月日")]
        public string DateFrom { get; set; }

        [Display(Name = "処理対象年月日To")]
        public string DateTo { get; set; }

        [Display(Name = "処理対象年月")]
        public string Month { get; set; }

        [Display(Name = "実行日時")]
        public string StartTt { get; set; }

        [Display(Name = "処理ステータス")]
        public string Status { get; set; }

        [Display(Name = "実行者")]
        public string CrtName { get; set; }

        public DateTime StartDateTime { get; set; }
    }

    public class TankaAutoSetList
    {
        //担当部署
        public string Tanbsy { get; set; }
        //処理
        public string Syori { get; set; }
        //品番コード
        public string Hincod { get; set; }
        //保管場所コード
        public string Sybcod { get; set; }
        //保管場所名
        public string Sybnam { get; set; }
        //機種A
        public string Kisyua { get; set; }
        //機種B
        public string Kisyub { get; set; }
        //品名/型式
        public string Hinnmk { get; set; }
        //振替区分
        public string Frikae { get; set; }
        //FP単価
        public decimal? Fptank { get; set; }
        //保管単価
        public decimal? Hokant { get; set; }
        //更新前場所
        public string OSybcod { get; set; }
        //更新前品名型式
        public string OHinnmk { get; set; }
        //更新前機種Ａ
        public string OKisyua { get; set; }
        //更新前機種Ｂ
        public string OKisyub { get; set; }
        //更新前振替区分
        public string OFrikae { get; set; }
        //更新前FP単価
        public decimal? OFptank { get; set; }
        //更新前保管単価
        public decimal? OHokant { get; set; }
        //登録日
        public DateTime? Crtymd { get; set; }
        //更新日
        public DateTime? Updymd { get; set; }


    }
}
