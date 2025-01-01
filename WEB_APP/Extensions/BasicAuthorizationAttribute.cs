using COMMON_SERVICE.Interface;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_UTILITY;
using DATA.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace CORPORATE_DISBURSEMENT_ADMIN
{
    public sealed class BasicAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ICommonService _commonManager;
        private readonly ICommonRepository _commonRepository;
        private readonly IConfiguration _configuration;
        public BasicAuthorizationAttribute(ICommonService commonManager, ICommonRepository commonRepository, IConfiguration configuration)
        {
            _commonManager = commonManager;
            _commonRepository = commonRepository;
            _configuration = configuration;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            CommonResponse commonResponse = new();
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }
            else
            {
                if (context.HttpContext.Request.Headers.Any(a => a.Key == "Authorization"))
                {
                    var authHeader = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers["Authorization"]);
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                    var username = credentials[0];
                    var password = credentials[1];
                    var user = await _commonRepository.GetApiUserByUserName(username);
                    if (user != null)
                    {
                        bool user_status = user.Status ?? false;
                        if (user_status)
                        {
                            string? secretKey = _configuration.GetValue<string>("application_configuration:secret_hash_key");
                            string? decryptedPassword = AESEncryption.DecryptString(secretKey ?? string.Empty, user.Password ?? String.Empty);
                            if (decryptedPassword != password)
                            {
                                commonResponse = await _commonManager.HandleResponse((int)HttpStatusCode.Unauthorized, CommonEnum.API_Response_Codes.Unauthorized, commonResponse);
                                // set 'WWW-Authenticate' header to trigger login popup in browsers                            
                                context.Result = new JsonResult(commonResponse) { StatusCode = StatusCodes.Status401Unauthorized };
                            }
                            else
                            {
                                context.HttpContext.Items.Add("User", JsonConvert.SerializeObject(user));
                            }
                        }
                        else
                        {
                            commonResponse = _commonManager.HandleResponse((int)HttpStatusCode.Unauthorized, CommonEnum.API_Response_Codes.Unauthorized, commonResponse, String.Empty, "User is inactive").Result;
                            // set 'WWW-Authenticate' header to trigger login popup in browsers
                            context.Result = new JsonResult(commonResponse) { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                    }
                    else
                    {
                        commonResponse = _commonManager.HandleResponse((int)HttpStatusCode.Unauthorized, CommonEnum.API_Response_Codes.Unauthorized, commonResponse).Result;
                        context.Result = new JsonResult(commonResponse) { StatusCode = StatusCodes.Status401Unauthorized };
                        // set 'WWW-Authenticate' header to trigger login popup in browsers
                        context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"\", charset=\"UTF-8\"";
                    }
                }
                else
                {
                    commonResponse = _commonManager.HandleResponse((int)HttpStatusCode.Unauthorized, CommonEnum.API_Response_Codes.Unauthorized, commonResponse).Result;
                    context.Result = new JsonResult(commonResponse) { StatusCode = StatusCodes.Status401Unauthorized };
                    // set 'WWW-Authenticate' header to trigger login popup in browsers
                    context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"\", charset=\"UTF-8\"";
                }

            }

        }
    }
}
