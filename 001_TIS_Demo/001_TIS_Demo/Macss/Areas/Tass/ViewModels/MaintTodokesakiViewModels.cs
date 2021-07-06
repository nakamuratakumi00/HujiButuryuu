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
    public class MaintTodokesakiViewModel
    {

        public MaintTodokesakiViewModel() : this(new MUnsouTodokesakiKoyuu())
        {

        }

        public MaintTodokesakiViewModel(MUnsouTodokesakiKoyuu todokesaki)
        {
            Model = todokesaki;
            if (todokesaki == null)
            {
                todokesaki = new Macss.Areas.Tass.Models.MUnsouTodokesakiKoyuu();
            }
        }

        [Display(Name = "Ｈ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek01 { get => Model.Sdek01; set => Model.Sdek01 = value; }

        [Display(Name = "ｉ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek02 { get => Model.Sdek02; set => Model.Sdek02 = value; }

        [Display(Name = "Ｍ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek03 { get => Model.Sdek03; set => Model.Sdek03 = value; }

        [Display(Name = "Ｐ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek04 { get => Model.Sdek04; set => Model.Sdek04 = value; }

        [Display(Name = "集")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek05 { get => Model.Sdek05; set => Model.Sdek05 = value; }

        [Display(Name = "構")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek06 { get => Model.Sdek06; set => Model.Sdek06 = value; }

        [Display(Name = "Ｙ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek07 { get => Model.Sdek07; set => Model.Sdek07 = value; }

        [Display(Name = "半")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek08 { get => Model.Sdek08; set => Model.Sdek08 = value; }

        [Display(Name = "長")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek09 { get => Model.Sdek09; set => Model.Sdek09 = value; }

        [Display(Name = "在")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek10 { get => Model.Sdek10; set => Model.Sdek10 = value; }

        [Display(Name = "需")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek11 { get => Model.Sdek11; set => Model.Sdek11 = value; }

        [Display(Name = "顧")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek12 { get => Model.Sdek12; set => Model.Sdek12 = value; }

        [Display(Name = "得")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek13 { get => Model.Sdek13; set => Model.Sdek13 = value; }

        [Display(Name = "ブ")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek14 { get => Model.Sdek14; set => Model.Sdek14 = value; }

        [Display(Name = "基")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Sdek15 { get => Model.Sdek15; set => Model.Sdek15 = value; }

        [ScriptIgnore]
        public MUnsouTodokesakiKoyuu Model { get; set; }

    }
}