using Microsoft.AspNetCore.Mvc.Rendering;
using DATA.Models;

namespace DATA.Interface
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetRolePermissionList();
        Task<RolePermission> GetRolePermissionById(int id);
        Task<List<RolePermission>> GetRolePermissionListByRoleId(int roleId);
        Task<int> AddRolePermission(RolePermission rolePermission);
        Task<int> DeleteRolePermission(RolePermission rolePermission);
        Task<IEnumerable<SelectListItem>> GetpermissionSelectList(int roleId);
    }
}
