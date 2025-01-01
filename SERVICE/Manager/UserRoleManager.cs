using Serilog;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class UserRoleManager : IUserRoleManager
    {
        private readonly IUserRoleRepository _UserRoleRepository;
        private readonly ILogger _logger;
        public UserRoleManager(IUserRoleRepository UserRoleRepository, ILogger logger)
        {
            _UserRoleRepository = UserRoleRepository;
            _logger = logger;
        }
        /// <summary>
        /// creates user role
        /// </summary>
        /// <param name="MonitoringUserRole"></param>
        /// <returns></returns>
        public async Task<int> AddUserRole(UserRole MonitoringUserRole)
        {
            try
            {
                return await _UserRoleRepository.AddUserRole(MonitoringUserRole);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserRoleManager->AddUserRole. Error: {ex.ToString()}");
                throw;
            }
        }
        //deletes user role
        public async Task<int> DeleteUserRole(UserRole MonitoringUserRole)
        {
            try
            {
                return await _UserRoleRepository.DeleteUserRole(MonitoringUserRole);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserRoleManager->DeleteUserRole. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// user role list loading method
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserRole>> GetAllUserRoles()
        {
            try
            {
                return await _UserRoleRepository.GetAllUserRoles();
            }
            catch (Exception ex)
            {
                _logger.Error($"UserRoleManager->GetAllUserRoles. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// gets user role information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRole> GetUserRoleById(int id)
        {
            try
            {
                return await _UserRoleRepository.GetUserRoleById(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserRoleManager->GetUserRoleById. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// update user role information
        /// </summary>
        /// <param name="userRoleViewModel"></param>
        /// <returns></returns>
        public async Task<int> UpdateUserRole(UserRoleViewModel userRoleViewModel)
        {
            int result = 0;
            try
            {
                var data = await _UserRoleRepository.GetUserRoleById(userRoleViewModel.Id);
                if (data != null)
                {
                    data.UserId = userRoleViewModel.UserId;
                    data.RoleId = userRoleViewModel.RoleId;
                    result = await _UserRoleRepository.UpdateUserRole(data);
                }

            }
            catch (Exception ex)
            {
                _logger.Error($"UserRoleManager->UpdateUserRole. Error: {ex.ToString()}");
                throw;
            }
            return result;
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
                return await _UserRoleRepository.GetUserRoleByUserId(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserRoleManager->GetUserRoleByUserId. Error: {ex.ToString()}");
                throw;
            }
        }
    }
}
