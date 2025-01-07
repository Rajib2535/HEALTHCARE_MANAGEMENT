
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Interface
{
    public interface IDashboardRepository
    {
        Task<int> PendingRefundRequestCount();
        Task<int> UnsettledTransactionCount();
        Task<decimal> TotalRefundAmount();
        Task<decimal> TotalTransactionAmount();
        Task<int> TotalTransactionCount();
        Task<int> TotalRefundCount();
        Task<int> SettledTransactionCount();
        Task<decimal> TotalSettlementAmount();
        Task<decimal> TotalUnsettledAmount();
    }
}
