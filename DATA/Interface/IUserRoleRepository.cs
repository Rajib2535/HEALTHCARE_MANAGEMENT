using DATA.Models;

namespace DATA.Interface
{
    public interface IUserRoleRepository
    {
        Task<List<UserRole>> GetAllUserRoles();
        Task<UserRole> GetUserRoleById(int id);
        Task<int> AddUserRole(UserRole MonitoringUserRole);
        Task<int> UpdateUserRole(UserRole MonitoringUserRole);
        Task<int> DeleteUserRole(UserRole MonitoringUserRole);
        Task<List<UserRole>> GetUserRoleByUserId(int id);
    }
}
