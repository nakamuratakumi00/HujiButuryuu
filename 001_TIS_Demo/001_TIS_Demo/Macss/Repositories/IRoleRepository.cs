using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleViewModel>> GetAllRolesAsync();

        Task<IEnumerable<RoleInformation>> GetAllRolesInfoAsync();

        Task<IEnumerable<MRole>> GetSelectRolesAsync(string roleId);

        Task<List<string>> GetUserRoleAsync(string userId);

        Task<IEnumerable<(string field, string message)>> UpdateRoleAsync(string id, RoleViewModel role, string loginUser);

        Task<string> DeleteRolesAsync(string roleId);
    }
}