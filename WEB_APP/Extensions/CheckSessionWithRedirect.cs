using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SERVICE.Interface;
using System.Net;
using ILogger = Serilog.ILogger;

namespace CORPORATE_DISBURSEMENT_ADMIN.Extensions
{
    /// <summary>
    /// This class is used for checking the session variables.
    /// If the session is expired, it redirects the application into the login page
    /// </summary>
    public class CheckSessionWithRedirect : Attribute, IActionFilter
    {
        private readonly IHttpContextAccessor _iHttpContextAccessor;
        private readonly IUserManager _iUserManager;
        private readonly ILogger _logger;
        public CheckSessionWithRedirect(IHttpContextAccessor iHttpContextAccessor, IUserManager iUserManager, ILogger logger)
        {
            _iHttpContextAccessor = iHttpContextAccessor;
            _iUserManager = iUserManager;
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Exception != null && context.Controller is Controller controller)
                {

                    context.ExceptionHandled = true;
                    controller.TempData["SessionExpired"] = context.Exception.Message.ToString();
                    context.Result = new RedirectToRouteResult
                       (
                       new RouteValueDictionary(new
                       {
                           action = "Login",
                           controller = "User"
                       }));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "User"
                }));
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var appRootFolder = $"{_iHttpContextAccessor.HttpContext?.Request.Scheme}://{_iHttpContextAccessor.HttpContext?.Request.Host}{_iHttpContextAccessor.HttpContext?.Request.PathBase}";
                var sessionInfo = _iHttpContextAccessor.HttpContext?.Session.Keys.ToList();
                if (context.Controller is Controller controller && (sessionInfo == null || sessionInfo.Count == 0))
                {
                    controller.TempData["SessionExpired"] = "Session Expired! Please log in again using your credential!";
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "Login",
                        controller = "User"
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "User"
                }));
            }
        }
    }
}
