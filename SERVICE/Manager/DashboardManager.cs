using Serilog;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CORPORATE_DISBURSEMENT_UTILITY;
using DocumentFormat.OpenXml.Office.CustomUI;
using Newtonsoft.Json;
using DATA.Interface;
using SERVICE.Interface;

namespace SERVICE.Manager
{
    public class DashboardManager : IDashboardManager
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ILogger _logger;

        public DashboardManager(IDashboardRepository dashboardRepository, ILogger logger)
        {
            _dashboardRepository = dashboardRepository;
            _logger = logger;

        }

        public async Task<ResponseEntity> APIConnectivityCheck()
        {
            ResponseEntity responseEntity = new();
            List<APIConnectivityStatus> aPIConnectivityStatuses = [];

            responseEntity.is_valid = true;
            responseEntity.data = aPIConnectivityStatuses;
            return responseEntity;
        }

        public async Task<ResponseEntity> DashboardCards()
        {
            ResponseEntity responseEntity = new();
            try
            {
                DashboardCardViewModel dashboardCardViewModel = new()
                {
                    PendingRefundRequestCount = await _dashboardRepository.PendingRefundRequestCount(),
                    TotalTransactionAmount = await _dashboardRepository.TotalTransactionAmount(),
                    TotalRefundAmount = await _dashboardRepository.TotalRefundAmount(),
                    TotalRefundCount = await _dashboardRepository.TotalRefundCount(),
                    TotalTransactionCount = await _dashboardRepository.TotalTransactionCount(),
                    UnsettledTransactionCount = await _dashboardRepository.UnsettledTransactionCount(),
                    SettledTransactionCount = await _dashboardRepository.SettledTransactionCount(),
                    TotalSettlementAmount = await _dashboardRepository.TotalSettlementAmount(),
                    TotalUnsettledAmount = await _dashboardRepository.TotalUnsettledAmount(),
                };
                responseEntity.data = dashboardCardViewModel;
                responseEntity.is_valid = true;
            }
            catch (Exception ex)
            {
                responseEntity.is_valid = false;
                responseEntity.error_messages = new List<string> { ex.Message };
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return responseEntity;
        }
    }
}
