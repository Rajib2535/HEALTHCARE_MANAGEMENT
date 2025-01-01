
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_UTILITY;
using DATA.Interface;
using DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SERVICE.Interface;
using System.Net;
using ILogger = Serilog.ILogger;

namespace SERVICE.Manager
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ICommonManager _commonManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICredentialRepository _stakeholderRepository;
        private readonly ITokenHandler _tokenHandler;
        public TokenManager(IConfiguration configuration, IUserRepository userRepository, ILogger logger, ICommonManager commonManager, IHttpContextAccessor httpContextAccessor, ICredentialRepository stakeholderRepository, ITokenHandler tokenHandler)
        {
            _configuration = configuration;
            _logger = logger;
            _commonManager = commonManager;
            _httpContextAccessor = httpContextAccessor;
            _stakeholderRepository = stakeholderRepository;
            _tokenHandler = tokenHandler;
        }
        public async Task<bool> LoginCashBaba()
        {
            CommonResponse commonResponse = new();
            CBTokenResponseModel? tokenResponse = new();
            try
            {
                _logger.Information($"TokenManager/Login ==> request started");

                List<ApiCredential>? credentials = await _stakeholderRepository.GetCredentialByClient((int)CommonEnum.Client.CashBaba);
                if (credentials == null)
                {
                    _logger.Information($"TokenManager/Login ==> CashBaba Credential not found.");
                    return false;
                }
                ApiCredential? grantType = credentials.Where(x => x.CredentialType == (int)CommonEnum.StakeholderCredential.grant_type).FirstOrDefault();
                if (grantType == null)
                {
                    _logger.Information($"TokenManager/Login ==> grantType not found.");
                    return false;
                }
                ApiCredential? clentId = credentials.Where(x => x.CredentialType == (int)CommonEnum.StakeholderCredential.client_id).FirstOrDefault();
                if (clentId == null)
                {
                    _logger.Information($"TokenManager/Login ==> ClentId not found.");
                    return false;
                }
                ApiCredential? clentSecret = credentials.Where(x => x.CredentialType == (int)CommonEnum.StakeholderCredential.client_secret).FirstOrDefault();
                if (clentSecret == null)
                {
                    _logger.Information($"TokenManager/Login ==> clentSecret not found.");
                    return false;
                }
                ApiCredential? baseUrl = credentials.Where(x => x.CredentialType == (int)CommonEnum.StakeholderCredential.cb_base_url).FirstOrDefault();
                if (baseUrl == null)
                {
                    _logger.Information($"TokenManager/Login ==> baseUrl not found.");
                    return false;
                }
                ApiCredential? urlEndPoint = credentials.Where(x => x.CredentialType == (int)CommonEnum.StakeholderCredential.cb_token_url_endpoint).FirstOrDefault();
                if (urlEndPoint == null)
                {
                    _logger.Information($"TokenManager/Login ==> urlEndPoint not found.");
                    return false;
                }

                var formData = new Dictionary<string, string>
                    {
                        { "grant_type", grantType.FieldValue },
                        { "client_id", clentId.FieldValue },
                        { "client_secret", clentSecret.FieldValue }
                    };

                ExternalAPIRequestModel externalAPIRequestModel = new()
                {
                    api_scope = CommonEnum.APIScope.CashBaba,
                    external_api_request_type = CommonEnum.ExternalAPIRequestType.POSTHttpFormURLEncoded,
                    url_encoded_body = formData,
                    request_headers = null,
                    url = $"{baseUrl.FieldValue}{urlEndPoint.FieldValue}",
                    is_rcvc_true = true,
                };

                ResponseModel response = await _commonManager.GetExternalAPIResponse(externalAPIRequestModel);

                _logger.Information("TokenManager/Login==>response from Cashbaba " + JsonConvert.SerializeObject(response));
                try
                {
                    tokenResponse = JsonConvert.DeserializeObject<CBTokenResponseModel>(response.response_body);
                }
                catch (Exception ex)
                {
                    _logger.Error($"TokenManager/Login ==> {WebUtility.HtmlEncode(ex.ToString())}");
                    return false;
                }

                if (tokenResponse == null || tokenResponse.code != (int)StatusCodes.Status200OK)
                {
                    _logger.Error($"TokenManager/Login ==> response null or status code {WebUtility.HtmlEncode(tokenResponse?.code.ToString())}");
                    return false;
                }
                if (tokenResponse != null && tokenResponse.code == 200 && !string.IsNullOrEmpty(tokenResponse.access_token))
                {
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        _httpContextAccessor?.HttpContext?.Session.SetString("token", tokenResponse.access_token);
                        _httpContextAccessor?.HttpContext?.Session.SetString("tokenType", tokenResponse.token_type);
                        _httpContextAccessor?.HttpContext?.Session.SetInt32("expireIn", tokenResponse.expires_in);
                    }
                    else
                    {
                        _tokenHandler.SetToken(tokenResponse.access_token, tokenResponse.token_type, tokenResponse.expires_in);
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.Error($"TokenManager/Login ==> {WebUtility.HtmlEncode(ex.ToString())}");
                return false;
            }

        }
    }
}
