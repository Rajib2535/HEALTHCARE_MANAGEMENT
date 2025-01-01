using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Models;

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
