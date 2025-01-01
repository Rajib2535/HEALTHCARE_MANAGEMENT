using Serilog;

using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class PermissionManager : IPermissionManager
    {
        private readonly IPermissionRepository _iPermissionRepository;
        private readonly ILogger _logger;
        public PermissionManager(IPermissionRepository iPermissionRepository, ILogger logger)
        {
            _iPermissionRepository = iPermissionRepository;
            _logger = logger;
        }
        /// <summary>
        /// creates menu permission
        /// </summary>
        /// <param name="MonitoringPermission"></param>
        /// <returns></returns>
        public async Task<int> AddPermission(Permission MonitoringPermission)
        {
            try
            {
                return await _iPermissionRepository.AddPermission(MonitoringPermission);
            }
            catch (Exception ex)
            {
                _logger.Error($"PermissionManager->AddPermission. Error: {ex.ToString()}");
                throw;
            }

        }
        /// <summary>
        /// deletes menu permission.
        /// </summary>
        /// <param name="MonitoringPermission"></param>
        /// <returns></returns>
        public async Task<int> DeletePermission(Permission MonitoringPermission)
        {
            try
            {
                return await _iPermissionRepository.DeletePermission(MonitoringPermission);
            }
            catch (Exception ex)
            {
                _logger.Error($"PermissionManager->DeletePermission. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// load all menu permission list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Permission>> GetAllPermissions()
        {
            try
            {
                return await _iPermissionRepository.GetAllPermissions();
            }
            catch (Exception ex)
            {
                _logger.Error($"PermissionManager->GetAllPermissions. Error: {ex.ToString()}");
                throw;
            }

        }
        /// <summary>
        /// gets menu permission information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Permission> GetPermissionById(int id)
        {
            try
            {
                return await _iPermissionRepository.GetPermissionById(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"PermissionManager->GetPermissionById. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// update menu permission method
        /// </summary>
        /// <param name="MonitoringPermission"></param>
        /// <returns></returns>
        public async Task<int> UpdatePermission(Permission MonitoringPermission)
        {
            try
            {
                return await _iPermissionRepository.UpdatePermission(MonitoringPermission);
            }
            catch (Exception ex)
            {
                _logger.Error($"PermissionManager->UpdatePermission. Error: {ex.ToString()}");
                throw;
            }

        }
        public async Task<List<DisplayTable>> GetDisplayTablePermissionList()
        {
            List<DisplayTable> result = new List<DisplayTable>();
            try
            {
                var role_list = await _iPermissionRepository.GetAllPermissions();
                if (role_list.Any())
                {
                    result = role_list.Select(x => new DisplayTable
                    {
                        Display = x.Name ?? string.Empty,
                        Key = x.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
    }
}
