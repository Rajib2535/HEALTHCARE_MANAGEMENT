using DATA.Interface;
using DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace DATA.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// delete user from database
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public async Task<int> DeleteUser(User UserInfo)
        {
            try
            {
                _dbContext.Users.Remove(UserInfo);
                return await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// gets a user info by username from database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        public async Task<List<User>> GetUserInfoByUserName(string userName)
        {
            try
            {
                return await _dbContext.Users.Where(b => b.Name == userName).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<User> GetUserInfoByEmail(string email)
        {
            try
            {
                return await _dbContext.Users.Where(b => b.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// gets user info list data
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUserInfos()
        {
            return await _dbContext.Users.ToListAsync();
        }
        /// <summary>
        /// gets usre infor information from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserInfoById(int id)
        {
            try
            {
                return await _dbContext.Users.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> SaveUserInfoByDummyRoleName(User UserInfo)
        {
            try
            {
                _dbContext.Users.Add(UserInfo);
                await _dbContext.SaveChangesAsync();
                return UserInfo.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> AddUser(User userInfo)
        {
            try
            {
                await _dbContext.Users.AddAsync(userInfo);
                await _dbContext.SaveChangesAsync();
                return userInfo.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> EditUser(User userInfo)
        {
            try
            {
                //_dbContext.Update(userInfo);
                //await _dbContext.SaveChangesAsync();
                var data = await _dbContext.Users.FindAsync(userInfo.Id);
                if (data != null)
                {
                    data.UpdateTime = userInfo.UpdateTime;
                    data.UpdatedBy = userInfo.UpdatedBy;
                    data.CreatedBy = userInfo.CreatedBy;
                    data.CreateTime = userInfo.CreateTime;
                    data.Email = userInfo.Email;
                    data.IsPasswordReset = userInfo.IsPasswordReset;
                    data.LoginType = userInfo.LoginType;
                    data.Type = userInfo.Type;
                    data.Password = userInfo.Password;
                    data.Status = userInfo.Status;
                    data.Userid = userInfo.Userid;
                    data.Mobile = userInfo.Mobile;
                    data.Name = userInfo.Name;
                    await _dbContext.SaveChangesAsync();
                }
                return userInfo.Id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetMaxUserId()
        {
            return await _dbContext.Users.MaxAsync(a => Convert.ToInt32(a.Userid));
        }
        public async Task<string> getRoleofUser(string userName)
        {
            //desco_app_db_admin_dbcontext db = new desco_app_db_admin_dbcontext();
            string roleName = "";
            List<User> listUsers = await _dbContext.Users.Where(b => b.Name == userName).ToListAsync();
            if (listUsers != null)
            {
                if (listUsers.Count > 0)
                {
                    int userId = listUsers[0].Id;
                    List<UserRole> listUserRole = await _dbContext.UserRoles.Where(b => b.UserId == userId).ToListAsync();
                    if (listUserRole != null)
                    {
                        if (listUserRole.Count > 0)
                        {
                            int roleId = listUserRole.First().RoleId;
                            List<Role> listRole = await _dbContext.Roles.Where(b => b.Id == roleId).ToListAsync();
                            roleName = listRole.First().Name ?? string.Empty;
                        }
                    }
                }
            }
            return roleName;
        }

        public async Task<int> AddPatientInfo(Patient patient)
        {
            await _dbContext.Patients.AddAsync(patient);
            await _dbContext.SaveChangesAsync();
            return patient.Id;
        }
    }
}
