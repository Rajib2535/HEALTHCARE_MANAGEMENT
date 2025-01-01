using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Interface;
using DATA.Models;

namespace DATA.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MenuRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// saves menu in database
        /// </summary>
        /// <param name="Menu"></param>
        /// <returns></returns>
        public async Task<int> CreateMenu(Menu Menu)
        {
            try
            {
                Menu apiMenus = new()
                {
                    Id = Menu.Id,
                    Url = Menu.Url,
                    ParentMenu = Menu.ParentMenu,
                    IsActive = Menu.IsActive,
                    MenuName = Menu.MenuName,
                    MenuOrder = Menu.MenuOrder
                };
                _dbContext.Menus.Add(apiMenus);
                await _dbContext.SaveChangesAsync();
                return apiMenus.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// deletes a menu from database
        /// </summary>
        /// <param name="Menu"></param>
        /// <returns></returns>
        public async Task<int> DeleteMenu(Menu Menu)
        {
            try
            {
                _dbContext.Menus.Remove(Menu);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// updates a menu information in database
        /// </summary>
        /// <param name="Menu"></param>
        /// <returns></returns>
        public async Task<int> EditMenu(Menu Menu)
        {
            try
            {
                _dbContext.Entry(Menu).State = EntityState.Modified;
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// gets menu information by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuById(int id)
        {
            return await _dbContext.Menus.FindAsync(id);
        }
        /// <summary>
        /// gets menu information by parent id from database
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuByParentId(int parentId)
        {
            return await _dbContext.Menus.FindAsync(parentId);
        }
        /// <summary>
        /// gets full menu list from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetMenuList()
        {
            return await _dbContext.Menus.ToListAsync();
        }

        public async Task<List<RoleWiseMenuViewModel>> GetRoleWiseMenuListOld(string roleName)
        {
            List<RoleWiseMenuViewModel> roleWiseMenuViewModels = new();

            Role role = await _dbContext.Roles.Where(a => a.Name == roleName).FirstOrDefaultAsync();
            if (role != null)
            {
                List<RolePermission> rolePermissions = await _dbContext.RolePermissions.Where(a => a.RoleId == role.Id).ToListAsync();
                if (rolePermissions.Count > 0)
                {
                    foreach (var item in rolePermissions)
                    {
                        item.Permission = await _dbContext.Permissions.Include(x => x.Menu).Where(a => a.Id == item.PermissionId).FirstOrDefaultAsync();
                        //item.Permission.MonitoringMenu = await _dbContext.MonitoringMenus.Where(a => a.Id == item.Permission.MenuId).FirstOrDefaultAsync();
                        if (item.Permission?.Menu?.IsActive == true && item.Permission?.Menu?.ParentMenu == 0)
                        {
                            roleWiseMenuViewModels.Add(new RoleWiseMenuViewModel
                            {
                                MenuId = item.Permission?.MenuId ?? 0,
                                MenuName = item.Permission?.Menu.MenuName,
                                MenuURL = item.Permission?.Menu.Url,
                                MenuIcon = "fa fa-times",
                                ControllerName = item.Permission?.Menu?.Url?.Split('/')[0],
                                ActionName = item.Permission?.Menu?.Url?.Split('/')[1],
                                Priority = item.Permission?.Menu.MenuOrder
                            });
                        }
                    }
                    if (roleWiseMenuViewModels.Count > 0)
                    {
                        foreach (var item in roleWiseMenuViewModels)
                        {
                            var childMenus = await _dbContext.Menus.Where(a => a.ParentMenu == item.MenuId && a.IsActive == true).OrderBy(a => a.MenuOrder).ToListAsync();
                            if (childMenus.Count > 0)
                            {
                                item.ChildMenuViewModels = new List<RoleWiseMenuViewModel.ChildMenuViewModel>();
                                foreach (var childMenu in childMenus)
                                {
                                    var permission = await _dbContext.Permissions.Where(a => a.MenuId == childMenu.Id).FirstOrDefaultAsync();
                                    if (permission != null && rolePermissions.Any(a => a.PermissionId == permission.Id))
                                    {
                                        item.ChildMenuViewModels.Add(new RoleWiseMenuViewModel.ChildMenuViewModel
                                        {
                                            MenuId = childMenu.Id,
                                            MenuName = childMenu.MenuName,
                                            MenuIcon = "fa fa-times",
                                            MenuURL = childMenu.Url,
                                            ControllerName = childMenu.Url?.Split('/')[0],
                                            ActionName = childMenu.Url?.Split('/')[1],
                                            Priority = childMenu.MenuOrder
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return roleWiseMenuViewModels.OrderBy(a => a.Priority).ToList();
        }
        public async Task<List<RoleWiseMenuViewModel>> GetRoleWiseMenuList(string role_name)
        {
            List<RoleWiseMenuViewModel> roleWiseMenuViewModels = new();
            Role role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == role_name);
            if (role == null)
            {
                return roleWiseMenuViewModels;
            }
            List<RolePermission> rolePermissions = await _dbContext.RolePermissions.Where(x => x.RoleId == role.Id).ToListAsync();
            if (!rolePermissions.Any())
            {
                return roleWiseMenuViewModels;
            }
            List<Permission> permissions = await _dbContext.Permissions.Where(x => rolePermissions.Select(y => y.PermissionId).Contains(x.Id)).ToListAsync();
            if (!permissions.Any())
            {
                return roleWiseMenuViewModels;
            }
            List<Menu> menus = await _dbContext.Menus.Where(x => permissions.Select(y => y.MenuId).Contains(x.Id)).ToListAsync();
            if (!menus.Any())
            {
                return roleWiseMenuViewModels;
            }
            roleWiseMenuViewModels = menus.Where(x => x.ParentMenu == null || x.ParentMenu == 0).OrderBy(x => x.MenuOrder).Select(item => new RoleWiseMenuViewModel
            {
                MenuId = item.Id,
                MenuName = item.MenuName,
                MenuIcon = "fa fa-times",
                MenuURL = item.Url,
                Priority = item.MenuOrder,
                ControllerName = item.Url == null ? string.Empty : item.Url.Split('/')[0],
                ActionName = item.Url == null ? string.Empty : item.Url.Split('/')[1],
                ChildMenuViewModels = menus.Where(x => x.ParentMenu == item.Id).OrderBy(x => x.MenuOrder).Select(x => new RoleWiseMenuViewModel.ChildMenuViewModel
                {
                    MenuId = x.Id,
                    MenuName = x.MenuName,
                    MenuIcon = "fa fa-times",
                    MenuURL = x.Url,
                    Priority = x.MenuOrder,
                    ControllerName = x.Url == null ? string.Empty : x.Url.Split('/')[0],
                    ActionName = x.Url == null ? string.Empty : x.Url.Split('/')[1],
                }).ToList()
            }).ToList();
            return roleWiseMenuViewModels;
        }
    }
}
