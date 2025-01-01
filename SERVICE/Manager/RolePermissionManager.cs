using Serilog;
using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class RolePermissionManager : IRolePermissionManager
    {
        private readonly IRolePermissionRepository _iRolePermissionRepository;
        private readonly ILogger _logger;
        public RolePermissionManager(IRolePermissionRepository iRolePermissionRepository, ILogger logger)
        {
            _iRolePermissionRepository = iRolePermissionRepository;
            _logger = logger;
        }
        /// <summary>
        /// role permission create method
        /// </summary>
        /// <param name="rolePermission"></param>
        /// <returns></returns>
        public async Task<int> AddRolePermission(RolePermission rolePermission)
        {
            try
            {
                return await _iRolePermissionRepository.AddRolePermission(rolePermission);
            }
            catch (Exception ex)
            {
                _logger.Error($"RolePermissionManager->AddRolePermission. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// delete role permission method
        /// </summary>
        /// <param name="rolePermission"></param>
        /// <returns></returns>
        public async Task<int> DeleteRolePermission(RolePermission rolePermission)
        {
            try
            {
                return await _iRolePermissionRepository.DeleteRolePermission(rolePermission);
            }
            catch (Exception ex)
            {
                _logger.Error($"RolePermissionManager->DeleteRolePermission. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// get role permission information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RolePermission> GetRolePermissionById(int id)
        {
            try
            {
                return await _iRolePermissionRepository.GetRolePermissionById(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"RolePermissionManager->GetRolePermissionById. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// load role permission list method
        /// </summary>
        /// <returns></returns>
        public async Task<List<RolePermission>> GetRolePermissionList()
        {
            try
            {
                return await _iRolePermissionRepository.GetRolePermissionList();
            }
            catch (Exception ex)
            {
                _logger.Error($"RolePermissionManager->GetRolePermissionList. Error: {ex.ToString()}");
                throw;
            }
        }

        public async Task<List<RolePermission>> GetRolePermissionListByRoleId(int roleId)
        {
            try
            {
                return await _iRolePermissionRepository.GetRolePermissionListByRoleId(roleId);
            }
            catch (Exception ex)
            {
                _logger.Error($"RolePermissionManager->GetRolePermissionListByRoleId. Error: {ex.ToString()}");
                throw;
            }
        }
    }
}
