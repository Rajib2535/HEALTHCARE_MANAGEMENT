
using DATA.Models;

namespace DATA.Interface
{
    public interface ICredentialRepository
    {
        Task<List<ApiCredential>> GetCredentialByClient(int clientType);
    }
}
