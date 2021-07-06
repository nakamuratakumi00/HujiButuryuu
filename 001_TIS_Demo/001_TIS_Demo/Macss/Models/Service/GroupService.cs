using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public class GroupService
    {

        public async Task<IEnumerable<(string field, string message)>> UpdatGroupAsync(GroupViewModel group, string loginUser, IGroupRepository groupRepository)
        {
            var errors = new List<(string field, string message)>();
            var appGroup = (await groupRepository.GetSelectGroupAsync(group.GroupCd)).FirstOrDefault();

            // 新規の場合
            if (group.Mode == 0)
            {
                // 重複確認
                if (appGroup != null)
                {
                    errors.Add((String.Empty, String.Format(Resources.Message.CE057, "グループコード")));
                    return errors;
                }
                appGroup = group.Model;
                appGroup.CreateId = loginUser;
                appGroup.CreateDate = DateTime.Now;
                appGroup.UpdateId = loginUser;
                appGroup.UpdateDate = DateTime.Now;
                await groupRepository.CreateGroupAsync(appGroup);
            }
            // 更新の場合
            else
            {
                var lowGroup = (await groupRepository.GetSelectUpperGroupAsync(group.GroupCd));

                // 下位グループの有無確認
                if (appGroup.UpperClassCd != group.UpperClassCd && lowGroup.Count() > 0)
                {
                    errors.Add((String.Empty, (String.Format(Resources.Message.CE058, "下位グループ", "上位グループは変更"))));
                    return errors;
                }
                // 上位グループが自分自身でないことの確認
                if (group.UpperClassCd == group.GroupCd)
                {
                    errors.Add((String.Empty, (String.Format(Resources.Message.CE033, "上位グループ", "編集中のグループは設定"))));
                    return errors;
                }
                appGroup.GroupName = group.GroupName;
                appGroup.UpperClassCd = group.UpperClassCd;
                appGroup.Kbn = group.Kbn;
                appGroup.UpdateId = loginUser;
                appGroup.UpdateDate = DateTime.Now;
                await groupRepository.UpdateGroupAsync(appGroup);
            }  
            return errors;
        }

        public string AddUpperClass(IEnumerable<GroupViewModel> groupList,string dispData, string upperCd)
        {
            var appUpperGroup = groupList.SingleOrDefault(x => x.GroupCd == upperCd);

            //上位グループの有無確認
            if (appUpperGroup == null)
            {
                return dispData;
            }

            //再帰的に上位グループの名前を取得し、文字列に追加
            var edit = " > " + appUpperGroup.GroupName + dispData;
            return AddUpperClass(groupList, edit, appUpperGroup.UpperClassCd);          
        }

        public List<GroupViewModel> GetUpperAsync(IEnumerable<GroupViewModel>groupList)
        {
            return groupList.Select(x => new GroupViewModel()
                {
                    GroupName = x.GroupName,
                    GroupCd = x.GroupCd,
                    UpperClassInformation = AddUpperClass(groupList, String.Empty, x.UpperClassCd) == String.Empty ? 
                                                String.Empty : AddUpperClass(groupList, String.Empty, x.UpperClassCd).Remove(0,3)
                }).ToList();                   
        }
    }
}