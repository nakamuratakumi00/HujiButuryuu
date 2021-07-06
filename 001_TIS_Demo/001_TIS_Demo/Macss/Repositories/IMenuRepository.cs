using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuViewModels>> GetDispMenuAsync();

        Task<List<string>> GetMenuRoleAsync(List<string> role);

        Task<IEnumerable<RoleViewModel>> GetAllMenuAsync();

        Task<IEnumerable<RoleViewModel>> GetMenuByRoleAsync(string roleId);

    }
}