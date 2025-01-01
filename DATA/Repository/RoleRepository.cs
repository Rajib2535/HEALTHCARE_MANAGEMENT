using Microsoft.EntityFrameworkCore;
using DATA.Interface;
using DATA.Models;

namespace DATA.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// saves a role in database
        /// </summary>
        /// <param name="MonitoringRole"></param>
        /// <returns></returns>
        public async Task<int> AddRole(Role MonitoringRole)
        {
            try
            {
                _dbContext.Roles.Add(MonitoringRole);
                await _dbContext.SaveChangesAsync();
                return MonitoringRole.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// deletes a role from database
        /// </summary>
        /// <param name="MonitoringRole"></param>
        /// <returns></returns>
        public async Task<int> DeleteRole(Role MonitoringRole)
        {
            try
            {
                _dbContext.Roles.Remove(MonitoringRole);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets list of roles from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                var data = await _dbContext.Roles.ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// gets a single role information by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleById(int id)
        {
            try
            {
                return await _dbContext.Roles.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Role> GetRoleByType(int id)
        {
            try
            {
                return await _dbContext.Roles.Where(x=>x.Type == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// updates a role information in database
        /// </summary>
        /// <param name="MonitoringRole"></param>
        /// <returns></returns>
        public async Task<int> UpdateRole(Role MonitoringRole)
        {
            try
            {
                _dbContext.Entry(MonitoringRole).State = EntityState.Modified;
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
