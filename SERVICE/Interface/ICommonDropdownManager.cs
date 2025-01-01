using DATA.Models;

namespace SERVICE.Interface
{
    public interface ICommonDropdownManager
    {
        public Task<List<CommonDropdown>> GetDropdowns(string identifier);
    }
}
