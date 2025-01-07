
using CORPORATE_DISBURSEMENT_UTILITY;
using DATA.Models;
using DATA.Models.RequestReponseModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SERVICE.Interface;
using System.Net;
using UTILITY;

namespace SERVICE.Manager
{
    public class CommonManager : ICommonManager
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientExtensions _httpClientExtensions;
        public CommonManager(ILogger logger, IHttpContextAccessor httpContextAccessor, IHttpClientExtensions httpClientExtensions)
        {

            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClientExtensions = httpClientExtensions;
        }
        public async Task<ResponseModel> GetExternalAPIResponse(ExternalAPIRequestModel externalAPIRequestViewModel)
        {
            ResponseModel responseModel = new();
            try
            {
                string user_id = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value;
                if (user_id == null)
                {
                    user_id = Convert.ToString(_httpContextAccessor.HttpContext?.Items["user_id"]);
                }
                string transaction_id = Convert.ToString(_httpContextAccessor.HttpContext?.Items["transaction_id"]);
                string order_id = Convert.ToString(_httpContextAccessor.HttpContext?.Items["order_id"]);
                string request_id = Convert.ToString(_httpContextAccessor.HttpContext?.Items["request_id"]);
                var logData = new ApiRequestLog
                {
                    ModuleName = GetType().FullName,
                    FunctionName = GetType().Name,
                    RequestBody = externalAPIRequestViewModel.request_body,
                    CreatedAt = DateTime.Now,
                    Scope = externalAPIRequestViewModel.api_scope.ToString(),
                    UserId = user_id,
                    RequestStartTime = DateTime.Now,
                    OrderId = order_id,
                    RequestUrl = externalAPIRequestViewModel.url,
                };
                if (_httpContextAccessor.HttpContext != null)
                {
                    logData.Ip = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For") ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() : _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "X-Forwarded-For").Value;
                    logData.Headers = JsonConvert.SerializeObject(_httpContextAccessor.HttpContext.Request.Headers.ToList());
                    logData.AppVersion = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("app-version") ? string.Empty : _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "app-version").Value;
                    logData.DeviceId = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("device-id") ? string.Empty : _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "device-id").Value;
                    logData.DeviceOs = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("device-os") ? string.Empty : _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "device-os").Value;
                    logData.DeviceModel = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("device-model") ? string.Empty : _httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "device-model").Value;
                    logData.Latitude = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("latitude") ? 0 : Convert.ToDecimal(_httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "latitude").Value);
                    logData.Longitude = !_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("longitude") ? 0 : Convert.ToDecimal(_httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(a => a.Key == "longitude").Value);
                    logData.RequestMethod = _httpContextAccessor.HttpContext.Request.Method;
                }
                else
                {
                    logData.Ip = string.Empty;
                    logData.Headers = JsonConvert.SerializeObject(new object());
                    logData.AppVersion = string.Empty;
                    logData.DeviceId = string.Empty;
                    logData.DeviceOs = string.Empty;
                    logData.DeviceModel = string.Empty;
                    logData.DeviceOs = string.Empty;
                    logData.Latitude = 0;
                    logData.Longitude = 0;
                    logData.RequestMethod = string.Empty;
                }
                try
                {
                    //logData = await _commonRepository.InsertReqResponseLog(logData);
                    _logger.Information($"GetExternalAPIResponse ==> api request entity: {WebUtility.HtmlEncode(JsonConvert.SerializeObject(logData))}");
                }
                catch (Exception ex)
                {
                    _logger.Error($"CommonManager/GetExternalAPIResponse ==> {WebUtility.HtmlEncode(ex.ToString())}");
                }
                if (externalAPIRequestViewModel.external_api_request_type == CommonEnum.ExternalAPIRequestType.POSTHttpFormURLEncoded)
                {
                    responseModel = await _httpClientExtensions.PostHttpFormURLEncodedRequest(externalAPIRequestViewModel.url ?? string.Empty, externalAPIRequestViewModel.url_encoded_body, externalAPIRequestViewModel.request_headers, externalAPIRequestViewModel.is_timeout, externalAPIRequestViewModel.timeout_seconds, externalAPIRequestViewModel.is_rcvc_true);
                }
                else if (externalAPIRequestViewModel.external_api_request_type == CommonEnum.ExternalAPIRequestType.POST)
                {
                    responseModel = await _httpClientExtensions.PostRequest(externalAPIRequestViewModel.url ?? string.Empty, externalAPIRequestViewModel.request_body ?? string.Empty, externalAPIRequestViewModel.request_headers, externalAPIRequestViewModel.is_timeout, externalAPIRequestViewModel.timeout_seconds, externalAPIRequestViewModel.is_rcvc_true);
                }
                else if (externalAPIRequestViewModel.external_api_request_type == CommonEnum.ExternalAPIRequestType.GET)
                {
                    responseModel = await _httpClientExtensions.GetRequest(externalAPIRequestViewModel.url ?? string.Empty, externalAPIRequestViewModel.request_headers, externalAPIRequestViewModel.is_get_request_body_available == false ? string.Empty : externalAPIRequestViewModel.request_body ?? string.Empty, externalAPIRequestViewModel.is_timeout, externalAPIRequestViewModel.timeout_seconds, externalAPIRequestViewModel.is_rcvc_true);
                }
                else if (externalAPIRequestViewModel.external_api_request_type == CommonEnum.ExternalAPIRequestType.POSTXML)
                {
                    responseModel = await _httpClientExtensions.PostXMLRequest(externalAPIRequestViewModel.url ?? string.Empty, externalAPIRequestViewModel.request_body ?? string.Empty, externalAPIRequestViewModel.request_headers, externalAPIRequestViewModel.is_timeout, externalAPIRequestViewModel.timeout_seconds, externalAPIRequestViewModel.is_rcvc_true);
                }
                if (logData != null)
                {
                    logData.ResponseBody = responseModel.response_body;
                    logData.ResponseCode = responseModel.response_code.ToString();
                    logData.RequestEndTime = DateTime.Now;
                    ///await _commonRepository.UpdateRequestResponseLog(logData);
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        if (_httpContextAccessor.HttpContext.Items.ContainsKey("external_api_response_body"))
                        {
                            _httpContextAccessor.HttpContext.Items["external_api_response_body"] = logData.ResponseBody;
                        }
                        else
                        {
                            _httpContextAccessor.HttpContext.Items.Add("external_api_response_body", logData.ResponseBody);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return responseModel;
        }
    }
}
