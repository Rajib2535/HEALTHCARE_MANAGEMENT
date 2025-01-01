using COMMON_SERVICE.Interface;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using CORPORATE_DISBURSEMENT_UTILITY;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using System.Net;

namespace COMMON_SERVICE.Service
{
    public class CommonService : ICommonService
    {
        private readonly ILogger _logger;
        public CommonService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<List<DropdownViewModel>> GetRefundRequestStatusDropdown()
        {
            List<DropdownViewModel> list = new();
            try
            {
                var currencyCodeDictonary = Enum.GetValues(typeof(CommonEnum.Refund_Request_Status))
               .Cast<CommonEnum.Refund_Request_Status>()
               .ToDictionary(t => (int)t, t => CommonEnum.GetDisplayName(t));
                foreach (var item in currencyCodeDictonary)
                {
                    DropdownViewModel dropdownViewModel = new()
                    {
                        Text = item.Value,
                        Value = item.Key.ToString()
                    };
                    list.Add(dropdownViewModel);
                }
            }
            catch (Exception ex)
            {

                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return list;
        }
        private static List<ResponseMessageViewModel> GetResponseCodes()
        {
            List<ResponseMessageViewModel> list = new();
            var currencyCodeDictonary = Enum.GetValues(typeof(CommonEnum.API_Response_Codes))
               .Cast<CommonEnum.API_Response_Codes>()
               .ToDictionary(t => (int)t, t => CommonEnum.GetDescription(t));
            foreach (var item in currencyCodeDictonary)
            {
                ResponseMessageViewModel dropdownViewModel = new()
                {
                    response_code = item.Key,
                    response_message = item.Value == null ? string.Empty : item.Value.ToString(),
                };
                list.Add(dropdownViewModel);
            }
            return list;
        }
        public async Task<CommonResponse> HandleResponse(int httpStatusCode, CommonEnum.API_Response_Codes error_code, CommonResponse responseEntity, string tag = "", string custom_message = "")
        {
            try
            {
                //string code = ((int)error_code).ToString();
                var responseData = GetResponseCodes().FirstOrDefault(f => f.response_code == (int)error_code);
                responseEntity.status = httpStatusCode.ToString().Trim();
                responseEntity.status_title = ReasonPhrases.GetReasonPhrase(httpStatusCode);
                responseEntity.timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (error_code != CommonEnum.API_Response_Codes.Success)
                {
                    string message = string.Empty;
                    if (responseData != null)
                    {
                        if (!string.IsNullOrWhiteSpace(custom_message))
                        {
                            message = custom_message;
                        }
                        else
                        {
                            message = responseData.response_message;
                        }
                    }
                    else
                    {
                        message = string.Empty;
                    }

                    responseEntity.errors = new List<string> { message ?? string.Empty };

                }
                else
                {

                    responseEntity.errors = new List<string>();
                }
            }
            catch (Exception ex)
            {
                responseEntity.status = StatusCodes.Status500InternalServerError.ToString().Trim();
                responseEntity.errors = new List<string>
                    {
                        ex.Message ?? string.Empty,
                    };
            }
            return responseEntity;
        }
    }
}
