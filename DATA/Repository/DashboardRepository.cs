
using DATA.Interface;
using DATA.Models;

namespace DATA.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public DashboardRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> PendingRefundRequestCount()
        {
            //return await _dbContext.ShohozRefundRequests
            //    .CountAsync(x => x.Status == (int)CommonEnum.Refund_Request_Status.PENDING 
            //&& x.RequestDate.HasValue 
            //&& x.RequestDate.Value.Date<DateTime.Today);
            return 0;
        }

        public async Task<decimal> TotalTransactionAmount()
        {
            //return (decimal)await _dbContext.ShohozTransactionRequests
            //    .Where(x=>x.RequestTime.HasValue 
            //&& x.RequestTime < DateTime.Today).SumAsync(x => x.TotalAmount);
            return 0;
        }

        public async Task<decimal> TotalRefundAmount()
        {
            //return (decimal)await _dbContext.ShohozTransactionRequests
            //    .Where(x => x.RefundedAt.HasValue
            //    && x.RefundedAt.Value.Date < DateTime.Today).SumAsync(x => x.TotalAmount);
            return 0;
        }
        public async Task<decimal> TotalSettlementAmount()
        {
            //return (decimal)await _dbContext.ShohozTransactionRequests
            //    .Where(x => x.SettlementStatus.HasValue
            //    && x.SettlementStatus == (int)CommonEnum.Settlement_Status.SETTLED).SumAsync(x => x.TotalAmount);
            return 0;
        }
        public async Task<decimal> TotalUnsettledAmount()
        {
            //return (decimal)await _dbContext.ShohozTransactionRequests
            //    .Where(x => x.SettlementStatus.HasValue
            //    && x.SettlementStatus == (int)CommonEnum.Settlement_Status.UNSETTLED).SumAsync(x => x.TotalAmount);
            return 0;
        }

        public async Task<int> TotalRefundCount()
        {
            //return await _dbContext.ShohozTransactionRequests
            //    .Where(x => x.RefundedAt.HasValue
            //    && x.RefundedAt.Value.Date < DateTime.Today).CountAsync();
            return 0;
        }

        public async Task<int> TotalTransactionCount()
        {
            //return await _dbContext.ShohozTransactionRequests
            //    .CountAsync(x => x.RequestTime.HasValue 
            //&& x.RequestTime < DateTime.Today);
            return 0;
        }

        public async Task<int> UnsettledTransactionCount()
        {
            //return await _dbContext.ShohozTransactionRequests.CountAsync(x => x.SettlementStatus == (int)CommonEnum.Settlement_Status.UNSETTLED && x.RequestTime.HasValue && x.RequestTime<DateTime.Today);
            return 0;
        }
        public async Task<int> SettledTransactionCount()
        {
            //return await _dbContext.ShohozTransactionRequests.CountAsync(x => x.SettlementStatus == (int)CommonEnum.Settlement_Status.SETTLED && x.RequestTime.HasValue && x.RequestTime < DateTime.Today);
            return 0;
        }
    }
}
