using DATA.Models;

namespace DATA.Interface
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllPermissions();
        Task<Permission> GetPermissionById(int id);
        Task<int> AddPermission(Permission MonitoringPermission);
        Task<int> UpdatePermission(Permission MonitoringPermission);
        Task<int> DeletePermission(Permission MonitoringPermission);
    }
}
