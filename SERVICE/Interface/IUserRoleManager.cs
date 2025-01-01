using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Models;

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
