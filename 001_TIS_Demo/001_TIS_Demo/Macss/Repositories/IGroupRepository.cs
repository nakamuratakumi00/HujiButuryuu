using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public interface IGroupRepository
    {
        Task<IEnumerable<GroupViewModel>> GetAllGroupAsync();
        Task<IEnumerable<MGroup>> GetSelectGroupAsync(string groupCd);
        Task<IEnumerable<MGroup>> GetSelectUpperGroupAsync(string upperGroupCd);
        Task CreateGroupAsync(MGroup appGroup);
        Task UpdateGroupAsync(MGroup appGroup);
        Task<string> DeleteGroupAsync(string cd);
    }
}