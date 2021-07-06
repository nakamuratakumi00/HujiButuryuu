using Macss.Areas.Tass.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.Areas.Tass.ViewModels
{
    public class WShuukaTyuumonshoTorikomiData
    {

        public TorikomiSerch Serch { get; set; }

        public TorikomiInformation Information { get; set; }

    }

    public class TorikomiSerch
    {

        [Display(Name = "取得担当")]
        public string Actcod { get; set; }

        [Display(Name = "取得日")]
        public decimal Ackymd { get; set; }

    }

    public class TorikomiInformation
    {

        [Display(Name = "出荷日")]
        public string Sykymd { get; set; }

        [Display(Name = "出荷No")]
        public string Syukno { get; set; }

        [Display(Name = "経費負担No／振替出荷No")]
        public string KeFsno { get; set; }

        [Display(Name = "届先名")]
        public string Tdknam { get; set; }

        [Display(Name = "品名")]
        public string Dhinnam { get; set; }

        [Display(Name = "場所")]
        public string Sybcod { get; set; }

        [Display(Name = "得意先CD")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先")]
        public string Toknam { get; set; }

        [Display(Name = "データ作成日(非表示)")]
        public string Cdate { get; set; }

        [Display(Name = "出荷日(変換用 非表示)")]
        public DateTime TSykymd { get; set; }
    }

}