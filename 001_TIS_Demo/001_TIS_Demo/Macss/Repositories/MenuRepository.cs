using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        #region 定数

        public enum MenuKbn
        {
            Hide = 0,
            Disp = 1,
        }

        public enum RoleKbn
        {
            Hide = 0,
            Disp = 1,
        }

        #endregion

        private readonly ApplicationDB dbContext;

        public MenuRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<MenuViewModels>> GetDispMenuAsync()
        {
            return await dbContext.MMenu
                            .OrderBy(x => x.MenuSort)
                            .Select(x => new MenuViewModels()
                            {
                                MenuId = x.MenuId,
                                MenuName = x.MenuName,
                                RoleName = x.RoleName,
                                ActionName = x.ActionName,
                                ControllerName = x.ControllerName,
                                TitleName = x.TitleName,
                                ParentId = x.ParentId,
                                MenuSort = x.MenuSort,
                                MenuKbn = x.MenuKbn,
                                RoleKbn = x.RoleKbn
                            }).ToListAsync();
        }

        public async Task<List<string>> GetMenuRoleAsync(List<string> role)
        {
            return await dbContext.MMenuRole.Where(x => role.Contains(x.RoleId)).Select(x => x.MenuId).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllMenuAsync()
        {
            return await dbContext.MMenu.Where(x => x.RoleKbn ==1).OrderBy(x => x.MenuSort)
                .Select(x => new RoleViewModel() { Id = x.MenuId, Name = x.RoleName }).ToListAsync();
        }
        
        public async Task<IEnumerable<RoleViewModel>> GetMenuByRoleAsync(string roleId)
        {
            return await dbContext.MMenuRole.Where(x => x.RoleId == roleId)
                .Join(dbContext.MMenu, x => x.MenuId, x => x.MenuId, (mr, m) => new { mr, m })
                .Select(x => new RoleViewModel() { Id = x.m.MenuId, Name = x.m.RoleName }).ToListAsync();
        }
    }
}