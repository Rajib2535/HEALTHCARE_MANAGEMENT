using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using DATA.Models;

namespace SERVICE.Interface
{
    public interface IRoleManager
    {
        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(int id);
        Task<Role> GetRoleByType(int id);
        Task<int> AddRole(Role MonitoringRole);
        Task<int> UpdateRole(Role MonitoringRole);
        Task<int> DeleteRole(Role MonitoringRole);
        Task<List<DisplayTable>> GetDisplayTableRoleList();
    }
}
