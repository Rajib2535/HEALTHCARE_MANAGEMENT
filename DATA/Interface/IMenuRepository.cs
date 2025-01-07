
using DATA.Models;
using DATA.Models.ViewModels.UserPermissionViewModels;

namespace DATA.Interface
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetMenuList();
        Task<Menu> GetMenuById(int id);
        Task<Menu> GetMenuByParentId(int parentId);
        Task<int> CreateMenu(Menu eftMenu);
        Task<int> EditMenu(Menu eftMenu);
        Task<int> DeleteMenu(Menu eftMenu);
        Task<List<RoleWiseMenuViewModel>> GetRoleWiseMenuList(string roleName);
    }
}
