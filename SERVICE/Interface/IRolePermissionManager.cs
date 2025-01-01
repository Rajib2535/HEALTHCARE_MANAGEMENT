using DATA.Models;

namespace SERVICE.Interface
{
    public interface IRolePermissionManager
    {
        Task<List<RolePermission>> GetRolePermissionList();
        Task<RolePermission> GetRolePermissionById(int id);
        Task<List<RolePermission>> GetRolePermissionListByRoleId(int roleId);
        Task<int> AddRolePermission(RolePermission rolePermission);
        Task<int> DeleteRolePermission(RolePermission rolePermission);

    }
}
