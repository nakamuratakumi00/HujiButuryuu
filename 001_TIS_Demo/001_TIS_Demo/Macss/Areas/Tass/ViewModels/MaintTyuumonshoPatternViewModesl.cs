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
    public class MaintTyuumonshoPatternViewModel 
    {

        public MaintTyuumonshoPatternViewModel() : this(new MUnsouShuukaTyuumonshoPattern())
        {

        }

        public MaintTyuumonshoPatternViewModel(MUnsouShuukaTyuumonshoPattern tuumonshoPattern)
        {
            Model = tuumonshoPattern;
            if (tuumonshoPattern == null)
            {
                tuumonshoPattern = new Macss.Areas.Tass.Models.MUnsouShuukaTyuumonshoPattern();
            }
        }

        [Display(Name = "出荷No")]
        [MaxLength(3, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sykno2 { get => Model.Sykno2; set => Model.Sykno2 = value; }

        [ScriptIgnore]
        public MUnsouShuukaTyuumonshoPattern Model { get; set; }
    }
}