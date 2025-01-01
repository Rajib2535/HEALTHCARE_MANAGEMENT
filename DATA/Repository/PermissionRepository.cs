using Microsoft.EntityFrameworkCore;
using DATA.Interface;
using DATA.Models;

namespace DATA.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PermissionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// saves a menu permission in database
        /// </summary>
        /// <param name="MonitoringPermission"></param>
        /// <returns></returns>
        public async Task<int> AddPermission(Permission MonitoringPermission)
        {
            try
            {
                _dbContext.Permissions.Add(MonitoringPermission);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// deletes a menu permission in database
        /// </summary>
        /// <param name="MonitoringPermission"></param>
        /// <returns></returns>
        public async Task<int> DeletePermission(Permission MonitoringPermission)
        {
            try
            {
                _dbContext.Permissions.Remove(MonitoringPermission);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// gets menu permission list from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Permission>> GetAllPermissions()
        {
            try
            {
                return await _dbContext.Permissions.Include(p => p.Menu).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// gets a single menu permission information by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Permission> GetPermissionById(int id)
        {
            try
            {
                return await _dbContext.Permissions.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// updats a menu permission in database
        /// </summary>
        /// <param name="MonitoringPermission"></param>
        /// <returns></returns>
        public async Task<int> UpdatePermission(Permission MonitoringPermission)
        {
            try
            {
                _dbContext.Entry(MonitoringPermission).State = EntityState.Modified;
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
