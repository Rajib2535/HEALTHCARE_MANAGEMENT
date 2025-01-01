
using DATA.Interface;
using DATA.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;

namespace DATA.Repository
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        public CredentialRepository(ILogger logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<List<ApiCredential>> GetCredentialByClient(int clientType)
        {
            _logger.Information($"CredentialRepository/GetCredentialByClient ==> request started with param => Client: {clientType}");
            List<ApiCredential> credentials = null;
            try
            {
                //credentials = await _dbContext.ApiCredentials.Where(d => d.ClientType == clientType).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"CredentialRepository/GetCredentialByClient ==> {WebUtility.HtmlEncode(ex.ToString())}");
            }
            return credentials;
        }
    }
}
