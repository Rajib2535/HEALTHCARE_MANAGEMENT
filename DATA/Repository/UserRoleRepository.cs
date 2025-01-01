using Microsoft.EntityFrameworkCore;
using DATA.Interface;
using DATA.Models;

namespace DATA.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// saves a user role in database
        /// </summary>
        /// <param name="MonitoringUserRole"></param>
        /// <returns></returns>
        public async Task<int> AddUserRole(UserRole MonitoringUserRole)
        {
            try
            {

                _dbContext.UserRoles.Add(MonitoringUserRole);
                await _dbContext.SaveChangesAsync();
                return MonitoringUserRole.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// deletes a user role from database
        /// </summary>
        /// <param name="MonitoringUserRole"></param>
        /// <returns></returns>
        public async Task<int> DeleteUserRole(UserRole MonitoringUserRole)
        {
            try
            {
                _dbContext.UserRoles.Remove(MonitoringUserRole);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets user role list data from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserRole>> GetAllUserRoles()
        {
            try
            {
                return await _dbContext.UserRoles.Include(u => u.Role).Include(u => u.User).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets a single user role information by id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRole> GetUserRoleById(int id)
        {
            try
            {
                return await _dbContext.UserRoles.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets user role information by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<UserRole>> GetUserRoleByUserId(int id)
        {
            try
            {
                return await _dbContext.UserRoles.Include(r => r.User).Include(r => r.Role).Where(b => b.UserId == id).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// updates user rinformation from database
        /// </summary>
        /// <param name="MonitoringUserRole"></param>
        /// <returns></returns>
        public async Task<int> UpdateUserRole(UserRole MonitoringUserRole)
        {
            try
            {
                _dbContext.Entry(MonitoringUserRole).State = EntityState.Modified;
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
