using Microsoft.EntityFrameworkCore;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA.Models;
using DATA.Interface;

namespace DATA.Repository
{
    public class CommonRepository : ICommonRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiUser> GetApiUserByUserName(string userName)
        {
            //return await _dbContext.ApiUsers.FirstOrDefaultAsync(x => x.UserName == userName);
            return null;
        }

        public async Task<AuditTrail> InsertAuditTrail(AuditTrail auditTrail)
        {
            await _dbContext.AddAsync(auditTrail);
            await _dbContext.SaveChangesAsync();
            return auditTrail;
        }
        public async Task<ApiRequestLog> InsertReqResponseLog(ApiRequestLog logData)
        {
            await _dbContext.AddAsync(logData);
            await _dbContext.SaveChangesAsync();
            return logData;
        }

        public async Task<int> UpdateReqResponseLog(ApiRequestLog apiRequestLog)
        {
            int result = 0;
            try
            {
                //result = await _dbContext.ApiRequestLogs.Where(a => a.Id == apiRequestLog.Id).UpdateFromQueryAsync(x => new ApiRequestLog
                //{
                //    RequestEndTime = apiRequestLog.RequestEndTime,
                //    ResponseBody = apiRequestLog.ResponseBody,
                //    ResponseCode = apiRequestLog.ResponseCode,
                //    OrderId = apiRequestLog.OrderId,
                //    TransactionId = apiRequestLog.TransactionId,
                //    UserId = apiRequestLog.UserId,
                //});
                //if (data != null)
                //{
                //    data.ResponseBody = apiRequestLog.ResponseBody;
                //    data.ResponseCode = apiRequestLog.ResponseCode;
                //    data.RequestEndTime = apiRequestLog.RequestEndTime;
                //    data.TransactionId=apiRequestLog.TransactionId;
                //    data.OrderId = apiRequestLog.OrderId;
                //    data.UserId = apiRequestLog.UserId;
                //    result = await _dbContext.SaveChangesAsync();
                //}                                
            }
            catch (Exception ex)
            {

                //_logger.LogError(WebUtility.HtmlEncode(ex.ToString()));
            }
            return result;
        }
    }
}
