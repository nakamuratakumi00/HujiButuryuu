using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Macss.Attributes.Validation;
using Macss.Models;

namespace Macss.ViewModels
{

    public class RoleViewModel
    {
        public RoleViewModel() : this(new MRole())
        {

        }

        public RoleViewModel(MRole role)
        {
            Model = role;
            if (role == null)
            {
                Model = new Models.MRole();
            }
        }

        [Display(Name = "ロールID")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Id { get => Model.RoleId; set => Model.RoleId = value; }

        [Display(Name = "ロール名")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(64, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Name { get => Model.RoleName; set => Model.RoleName = value; }

        [Display(Name = "ロール概要")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string RoleCmt { get => Model.RoleCmt; set => Model.RoleCmt = value; }

        [Display(Name = "メニュー設定")]
        public IEnumerable<string> Menu { get; set; }

        [Display(Name = "メニュー設定")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public IEnumerable<string> SetMenu { get; set; }

        public int Mode { get; set; }

        [ScriptIgnore]
        public MRole Model { get; set; }

    }

    public class RoleInformation
    {

        [Display(Name = "ロールID")]
        public string Id { get; set; }

        [Display(Name = "ロール名")]
        public string Name { get; set; }

        [Display(Name = "ロール概要")]
        public string RoleCmt { get; set; }

    }

}