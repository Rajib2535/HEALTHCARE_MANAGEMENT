
using DATA.Models.RequestReponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SERVICE.Interface;
using System.Net;

namespace WEB_APP.Extensions
{
    /// <summary>
    /// checking session for ajax requests
    /// </summary>
    public class CheckSession : IActionFilter
    {
        private readonly IHttpContextAccessor _iHttpContextAccessor;
        private readonly IUserManager _iEFTUserManager;
        public CheckSession(IHttpContextAccessor iHttpContextAccessor, IUserManager iEFTUserManager)
        {
            _iHttpContextAccessor = iHttpContextAccessor;
            _iEFTUserManager = iEFTUserManager;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var appRootFolder = $"{_iHttpContextAccessor.HttpContext.Request.Scheme}://{_iHttpContextAccessor.HttpContext.Request.Host}{_iHttpContextAccessor.HttpContext.Request.PathBase}";
            try
            {
                var sessionInfo = _iHttpContextAccessor.HttpContext.Session.Keys.ToList();
                var route = new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "User"
                });
                if (sessionInfo.Count == 0)
                {

                    context.Result = new JsonResult(new ResponseEntity
                    {
                        html = null,
                        is_valid = false,
                        session_expired = true,
                        redirect_url = "/User/Login"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        ContentType = "application/json",
                    };

                }
            }
            catch (Exception ex)
            {

                context.Result = new JsonResult(new ResponseEntity { html = null, is_valid = false, session_expired = true, redirect_url = $"{appRootFolder}/User/Login" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    ContentType = "application/json",
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var appRootFolder = $"{_iHttpContextAccessor.HttpContext.Request.Scheme}://{_iHttpContextAccessor.HttpContext.Request.Host}{_iHttpContextAccessor.HttpContext.Request.PathBase}";
            try
            {
                var sessionInfo = _iHttpContextAccessor.HttpContext.Session.Keys.ToList();
                if (sessionInfo.Count == 0)
                {

                    context.Result = new JsonResult(new ResponseEntity { html = null, is_valid = false, session_expired = true, redirect_url = $"{appRootFolder}/User/Login" })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        ContentType = "application/json",
                    };

                }
            }
            catch (Exception ex)
            {

                context.Result = new JsonResult(new ResponseEntity { html = null, is_valid = false, session_expired = true, redirect_url = $"{appRootFolder}/User/Login" })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    ContentType = "application/json",
                };
            }
        }
    }
}
