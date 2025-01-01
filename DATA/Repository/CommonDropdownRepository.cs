using DATA.Interface;
using DATA.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Microsoft.EntityFrameworkCore;

namespace DATA.Repository
{
    public class CommonDropdownRepository : ICommonDropdownRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommonDropdownRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CommonDropdown>> GetDropdowns(string identifier)
        {
            //return await _dbContext.CommonDropdown.Where(x => x.Identifier == identifier).ToListAsync();
            return null;
        }
    }
}
