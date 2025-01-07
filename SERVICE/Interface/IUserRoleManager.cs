
using DATA.Models;
using DATA.Models.ViewModels.UserPermissionViewModels;

namespace SERVICE.Interface
{
    public interface IUserRoleManager
    {
        Task<List<UserRole>> GetAllUserRoles();
        Task<int> AddUserRole(UserRole UserRole);
        Task<int> UpdateUserRole(UserRoleViewModel UserRole);
        Task<int> DeleteUserRole(UserRole UserRole);
        Task<List<UserRole>> GetUserRoleByUserId(int id);
        Task<UserRole> GetUserRoleById(int id);
    }
}
