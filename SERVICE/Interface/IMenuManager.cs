
using DATA.Models;
using DATA.Models.ViewModels;
using DATA.Models.ViewModels.UserPermissionViewModels;

namespace SERVICE.Interface
{
    public interface IMenuManager
    {
        Task<List<Menu>> GetMenuList();
        Task<Menu> GetMenuById(int id);
        Task<Menu> GetMenuByParentId(int parentId);
        Task<int> CreateMenu(Menu eftMenu);
        Task<int> EditMenu(Menu eftMenu);
        Task<int> DeleteMenu(Menu eftMenu);
        Task<List<DisplayTable>> GetMenuDisplayTableList();
        Task<List<RoleWiseMenuViewModel>> GetRoleWiseMenuList(string roleName);
    }
}
