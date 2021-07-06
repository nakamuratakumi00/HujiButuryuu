using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Macss.Attributes.Validation;
using Macss.Areas.Tass.Models;

namespace Macss.Areas.Tass.ViewModels
{
    public class MaintHinmeiViewModel
    {
        public MaintHinmeiViewModel() : this(new MUnsouHinmeiKoyuu())
        {

        }

        public MaintHinmeiViewModel(MUnsouHinmeiKoyuu hinmei)
        {
            Model = hinmei;
            if (hinmei == null)
            {
                hinmei = new Macss.Areas.Tass.Models.MUnsouHinmeiKoyuu();
            }
        }


        [Display(Name = "抽出フラグ")]
        public string Ctlfl1 { get => Model.Ctlfl1; set => Model.Ctlfl1 = value; }

        [ScriptIgnore]
        public MUnsouHinmeiKoyuu Model { get; set; }

    }
}