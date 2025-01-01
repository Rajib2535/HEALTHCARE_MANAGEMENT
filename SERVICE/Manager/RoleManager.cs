using Serilog;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using System.Formats.Asn1;
using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class RoleManager : IRoleManager
    {
        private readonly IRoleRepository _iRoleRepository;
        private readonly ILogger _logger;
        public RoleManager(IRoleRepository iRoleRepository, ILogger logger)
        {
            _iRoleRepository = iRoleRepository;
            _logger = logger;
        }
        /// <summary>
        /// create role method
        /// </summary>
        /// <param name="MonitoringRole"></param>
        /// <returns></returns>
        public async Task<int> AddRole(Role MonitoringRole)
        {
            try
            {
                return await _iRoleRepository.AddRole(MonitoringRole);
            }
            catch (Exception ex)
            {
                _logger.Error($"RoleManager->AddRole. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// delete role method
        /// </summary>
        /// <param name="MonitoringRole"></param>
        /// <returns></returns>
        public async Task<int> DeleteRole(Role MonitoringRole)
        {
            try
            {
                return await _iRoleRepository.DeleteRole(MonitoringRole);
            }
            catch (Exception ex)
            {
                _logger.Error($"RoleManager->DeleteRole. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// get all role list method
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                return await _iRoleRepository.GetAllRoles();
            }
            catch (Exception ex)
            {
                _logger.Error($"RoleManager->GetAllRoles. Error: {ex.ToString()}");
                throw;
            }
        }
        public async Task<List<DisplayTable>> GetDisplayTableRoleList()
        {
            List<DisplayTable> result = new List<DisplayTable>();
            try
            {
                var role_list = await _iRoleRepository.GetAllRoles();
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
        /// <summary>
        /// get role information by id method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleById(int id)
        {
            try
            {
                return await _iRoleRepository.GetRoleById(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"RoleManager->GetRoleById. Error: {ex.ToString()}");
                throw;
            }
        }

        public async Task<Role> GetRoleByType(int id)
        {
            try
            {
                return await _iRoleRepository.GetRoleByType(id);
            }
            catch (Exception ex)
            {
                _logger.Error($"RoleManager->GetRoleById. Error: {ex.ToString()}");
                throw;
            }
        }

        /// <summary>
        /// update role information method
        /// </summary>
        /// <param name="MonitoringRole"></param>
        /// <returns></returns>
        public async Task<int> UpdateRole(Role MonitoringRole)
        {
            try
            {
                return await _iRoleRepository.UpdateRole(MonitoringRole);
            }
            catch (Exception ex)
            {
                _logger.Error($"RoleManager->UpdateRole. Error: {ex.ToString()}");
                throw;
            }
        }
    }
}
