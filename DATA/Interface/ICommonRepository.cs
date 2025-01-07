
using DATA.Models;

namespace DATA.Interface
{
    public interface ICommonRepository
    {
        Task<AuditTrail> InsertAuditTrail(AuditTrail auditTrail);
        Task<ApiUser> GetApiUserByUserName(string userName);
        Task<ApiRequestLog> InsertReqResponseLog(ApiRequestLog logData);
        Task<int> UpdateReqResponseLog(ApiRequestLog apiRequestLog);
    }
}
