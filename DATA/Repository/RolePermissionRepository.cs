using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DATA.Interface;
using DATA.Models;

namespace DATA.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RolePermissionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// saves a role permission in database
        /// </summary>
        /// <param name="rolePermission"></param>
        /// <returns></returns>
        public async Task<int> AddRolePermission(RolePermission rolePermission)
        {
            try
            {
                _dbContext.RolePermissions.Add(rolePermission);
                await _dbContext.SaveChangesAsync();
                return rolePermission.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// deletes a role permission from database
        /// </summary>
        /// <param name="rolePermission"></param>
        /// <returns></returns>
        public async Task<int> DeleteRolePermission(RolePermission rolePermission)
        {
            try
            {
                _dbContext.RolePermissions.Remove(rolePermission);
                await _dbContext.SaveChangesAsync();
                return rolePermission.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets a role permission information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RolePermission> GetRolePermissionById(int id)
        {
            try
            {
                return await _dbContext.RolePermissions.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets role permission list from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<RolePermission>> GetRolePermissionList()
        {
            try
            {
                return await _dbContext.RolePermissions.Include(r => r.Permission).Include(r => r.Role).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets role permission list from database by role id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<RolePermission>> GetRolePermissionListByRoleId(int roleId)
        {
            try
            {
                return await _dbContext.RolePermissions.Include(r => r.Permission).Include(r => r.Role)
                           .Where(b => b.RoleId == roleId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetpermissionSelectList(int roleId)
        {
            //desco_app_db_admin_dbcontext db = new desco_app_db_admin_dbcontext();
            List<Permission> listPermissions = await _dbContext.Permissions.Where(b => b.IsActive == true).ToListAsync();
            List<SelectListItem> permissionSelectList = new List<SelectListItem>();
            List<RolePermission> listRolePermissions = await _dbContext.RolePermissions.Where(b => b.RoleId == roleId).ToListAsync();
            foreach (Permission permission in listPermissions)
            {
                SelectListItem Item = new SelectListItem();
                Item.Text = permission.Name;
                Item.Value = permission.Id.ToString();
                foreach (RolePermission rolePerm in listRolePermissions)
                {
                    if (rolePerm.PermissionId == permission.Id)
                    {
                        Item.Selected = true;
                    }
                }
                permissionSelectList.Add(Item);
            }
            return permissionSelectList;
        }
    }
}
