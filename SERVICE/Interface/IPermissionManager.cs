using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using DATA.Models;

namespace SERVICE.Interface
{
    public interface IPermissionManager
    {
        Task<List<Permission>> GetAllPermissions();
        Task<Permission> GetPermissionById(int id);
        Task<int> AddPermission(Permission MonitoringPermission);
        Task<int> UpdatePermission(Permission MonitoringPermission);
        Task<int> DeletePermission(Permission MonitoringPermission);
        Task<List<DisplayTable>> GetDisplayTablePermissionList();
    }
}
