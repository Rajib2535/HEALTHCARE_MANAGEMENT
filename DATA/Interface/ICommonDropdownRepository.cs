using DATA.Models;

namespace DATA.Interface
{
    public interface ICommonDropdownRepository
    {
        public Task<List<CommonDropdown>> GetDropdowns(string identifier);
    }
}
