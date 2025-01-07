using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models.ViewModels
{
    public class DashboardCardViewModel
    {
        public int PendingRefundRequestCount { get; set; }
        public int UnsettledTransactionCount { get; set; }
        public int SettledTransactionCount { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public decimal TotalSettlementAmount { get; set; }
        public decimal TotalUnsettledAmount { get; set; }
        public int TotalTransactionCount { get; set; }
        public int TotalRefundCount { get; set; }
    }
}
