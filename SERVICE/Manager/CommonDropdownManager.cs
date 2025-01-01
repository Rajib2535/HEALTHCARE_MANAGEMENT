using DATA.Interface;
using DATA.Models;
using Serilog;
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class CommonDropdownManager : ICommonDropdownManager
    {
        private readonly ILogger _logger;
        private readonly ICommonDropdownRepository _commonDropdownRepository;
        public CommonDropdownManager(ILogger logger, ICommonDropdownRepository commonDropdownRepository)
        {
            _logger = logger;
            _commonDropdownRepository = commonDropdownRepository;
        }
        public async Task<List<CommonDropdown>> GetDropdowns(string identifier)
        {
            try
            {
                return await _commonDropdownRepository.GetDropdowns(identifier);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
    }
}
