using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Macss.Models;

namespace Macss.ViewModels
{

    public class GroupViewModel
    {
        public GroupViewModel() : this(new MGroup())
        {

        }

        public GroupViewModel(MGroup group)
        {
            Model = group;
            if (group == null)
            {
                Model = new Models.MGroup();
            }
        }

        [Display(Name = "上位グループ")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string UpperClassCd { get => Model.UpperClassCd; set => Model.UpperClassCd = value; }
        
        [Display(Name = "グループコード")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string GroupCd { get => Model.GroupCd; set => Model.GroupCd = value; }

        [Display(Name = "グループ名")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(64, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string GroupName { get => Model.GroupName; set => Model.GroupName = value; }

        [Display(Name = "区分")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Kbn { get => Model.Kbn; set => Model.Kbn = value; }

        [Display(Name = "上位グループ情報")]
        public string UpperClassInformation { get; set; }

        public int Mode { get; set; }

        [ScriptIgnore]
        public MGroup Model { get; set; }

    }
}