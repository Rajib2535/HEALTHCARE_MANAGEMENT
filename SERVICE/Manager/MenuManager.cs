using Microsoft.AspNetCore.Http;
using Serilog;
using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;
using DATA.Models.ViewModels;
using DATA.Models.ViewModels.UserPermissionViewModels;

namespace SERVICE.Manager
{
    public class MenuManager : IMenuManager
    {
        private readonly IMenuRepository _eFTMenuRepository;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _iHttpContextAccesor;
        private readonly IUserManager _userManager;
        public MenuManager(IMenuRepository eFTMenuRepository, ILogger logger, IHttpContextAccessor httpContextAccessor, IUserManager userManager)
        {
            _eFTMenuRepository = eFTMenuRepository;
            _logger = logger;
            _iHttpContextAccesor = httpContextAccessor;
            _userManager = userManager;
        }
        /// <summary>
        /// menu create method
        /// </summary>
        /// <param name="eftMenu"></param>
        /// <returns></returns>
        public async Task<int> CreateMenu(Menu eftMenu)
        {
            try
            {
                var currentUserInfo = await _userManager.GetCurrentUserInfo();
                eftMenu.CreatedBy = currentUserInfo.Id;
                eftMenu.CreatedDate = DateTime.Now;
                eftMenu.CreatedBy = currentUserInfo.Id;
                eftMenu.UpdatedDate = DateTime.Now;
                return await _eFTMenuRepository.CreateMenu(eftMenu);
            }
            catch (Exception ex)
            {
                _logger.Error($"MenuManager->CreateMenu. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// deletes menu method
        /// </summary>
        /// <param name="eftMenu"></param>
        /// <returns></returns>
        public async Task<int> DeleteMenu(Menu eftMenu)
        {
            try
            {
                return await _eFTMenuRepository.DeleteMenu(eftMenu);
            }
            catch (Exception ex)
            {
                _logger.Error($"MenuManager->DeleteMenu. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// menu update method
        /// </summary>
        /// <param name="eftMenu"></param>
        /// <returns></returns>
        public async Task<int> EditMenu(Menu eftMenu)
        {
            var currentUserInfo = await _userManager.GetCurrentUserInfo();
            eftMenu.UpdatedBy = currentUserInfo.Id;
            eftMenu.UpdatedDate = DateTime.Now;
            return await _eFTMenuRepository.EditMenu(eftMenu);
        }
        /// <summary>
        /// loads single menu information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuById(int id)
        {
            return await _eFTMenuRepository.GetMenuById(id);
        }
        /// <summary>
        /// loads menu by parent menu id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<Menu> GetMenuByParentId(int parentId)
        {
            return await _eFTMenuRepository.GetMenuByParentId(parentId);
        }
        /// <summary>
        /// loads menu list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Menu>> GetMenuList()
        {
            return await _eFTMenuRepository.GetMenuList();
        }
        public async Task<List<DisplayTable>> GetMenuDisplayTableList()
        {
            List<DisplayTable> displayTables = new List<DisplayTable>();
            var menu_list = await _eFTMenuRepository.GetMenuList();
            if (menu_list.Any())
            {
                displayTables = menu_list.Select(x => new DisplayTable
                {
                    Display = x.MenuName ?? string.Empty,
                    Key = x.Id.ToString(),
                }).ToList();
            }
            return displayTables;
        }

        public async Task<List<RoleWiseMenuViewModel>> GetRoleWiseMenuList(string roleName)
        {
            return await _eFTMenuRepository.GetRoleWiseMenuList(roleName);
        }
    }
}
