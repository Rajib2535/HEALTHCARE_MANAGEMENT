using DATA.Models;

namespace DATA.Interface
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(int id);
        Task<Role> GetRoleByType(int id);
        Task<int> AddRole(Role MonitoringRole);
        Task<int> UpdateRole(Role MonitoringRole);
        Task<int> DeleteRole(Role MonitoringRole);
    }
}
