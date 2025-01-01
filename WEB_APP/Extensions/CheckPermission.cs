using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SERVICE.Interface;
using System.Net;
using ILogger = Serilog.ILogger;

namespace CORPORATE_DISBURSEMENT_ADMIN.Extensions
{
    /// <summary>
    /// this class is used for checking whether the user has permission on the current page or not
    /// if the user doesn't have the permission, he/she will be redirected to an error page
    /// </summary>
    public class CheckPermission : Attribute, IActionFilter
    {
        private readonly IHttpContextAccessor _iHttpContextAccessor;
        private readonly IUserManager _iUserManager;
        //private readonly INotificationManager _iNotificationManager;
        private readonly ILogger _logger;
        public CheckPermission(IHttpContextAccessor iHttpContextAccessor, IUserManager iUserManager, ILogger logger)
        {
            _iHttpContextAccessor = iHttpContextAccessor;
            _iUserManager = iUserManager;
            //_iNotificationManager = NotificationManager;
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                Controller? controller = context.Controller as Controller;
                if (controller == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "PermissionDenied",
                        controller = "Home"
                    }));
                    return;
                }
                string actionName = controller.ControllerContext.ActionDescriptor.ActionName.ToString();
                string controllerName = controller.ControllerContext.ActionDescriptor.ControllerName.ToString();
                string menuURL = controllerName + "/" + actionName;
                string? role_wise_screen_permissions = _iHttpContextAccessor?.HttpContext?.Session.GetString("RoleWiseScreenPermission");
                //List<RoleWiseMenuViewModel> roleWiseScreenPermissions =CORPORATE_DISBURSEMENT_UTILITY.SessionExtensions.GetComplexData<List<RoleWiseMenuViewModel>>(_iHttpContextAccessor.HttpContext.Session, "RoleWiseScreenPermission");
                if (string.IsNullOrWhiteSpace(role_wise_screen_permissions))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "PermissionDenied",
                        controller = "Home"
                    }));
                    return;
                }
                List<RoleWiseMenuViewModel>? roleWiseScreenPermissions = JsonConvert.DeserializeObject<List<RoleWiseMenuViewModel>>(role_wise_screen_permissions);
                if (roleWiseScreenPermissions == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "PermissionDenied",
                        controller = "Home"
                    }));
                    return;
                }
                if (!roleWiseScreenPermissions.Any(x => !string.IsNullOrWhiteSpace(x.MenuURL) && x.MenuURL.ToLower() == menuURL.ToLower()))
                {
                    bool isMenuAvailable = false;
                    foreach (var item in roleWiseScreenPermissions)
                    {
                        if (item.ChildMenuViewModels.Any(x => !string.IsNullOrWhiteSpace(x.MenuURL) && x.MenuURL.ToLower() == menuURL.ToLower()))
                        {
                            isMenuAvailable = true;
                            break;
                        }
                    }
                    if (!isMenuAvailable)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            action = "PermissionDenied",
                            controller = "Home"
                        }));
                        return;
                    }
                }
                //if (!roleWiseScreenPermissions.Any(a => a.ControllerName == controllerName))
                //{
                //    bool isMenuAvailable = false;
                //    foreach (var item in roleWiseScreenPermissions)
                //    {
                //        if (item.ChildMenuViewModels != null && item.ChildMenuViewModels.Count > 0 && !item.ChildMenuViewModels.Any(a => a.MenuURL == menuURL))
                //        {
                //            isMenuAvailable = false;
                //        }
                //        else
                //        {
                //            isMenuAvailable = true;
                //            break;
                //        }
                //    }
                //    if (isMenuAvailable == false)
                //    {
                //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //        {
                //            action = "PermissionDenied",
                //            controller = "Home"
                //        }));
                //    }
                //}
                //else
                //{
                //    foreach (var item in roleWiseScreenPermissions)
                //    {
                //        if (item.ChildMenuViewModels != null && item.ChildMenuViewModels.Count > 0 && item.ChildMenuViewModels.Any(a => a.ControllerName == controllerName) && !item.ChildMenuViewModels.Any(a => a.ActionName == actionName))
                //        {
                //            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //            {
                //                action = "PermissionDenied",
                //                controller = "Home"
                //            }));
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.Information(WebUtility.HtmlEncode(ex.ToString()));
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "PermissionDenied",
                    controller = "Home"
                }));
                return;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                Controller? controller = context.Controller as Controller;
                if (controller == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "PermissionDenied",
                        controller = "Home"
                    }));
                    return;
                }
                string actionName = controller.ControllerContext.ActionDescriptor.ActionName.ToString();
                string controllerName = controller.ControllerContext.ActionDescriptor.ControllerName.ToString();
                string menuURL = controllerName + "/" + actionName;
                string? role_wise_screen_permissions = _iHttpContextAccessor?.HttpContext?.Session.GetString("RoleWiseScreenPermission");
                if (string.IsNullOrWhiteSpace(role_wise_screen_permissions))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "PermissionDenied",
                        controller = "Home"
                    }));
                    return;
                }
                List<RoleWiseMenuViewModel>? roleWiseScreenPermissions = JsonConvert.DeserializeObject<List<RoleWiseMenuViewModel>>(role_wise_screen_permissions);
                if (roleWiseScreenPermissions == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "PermissionDenied",
                        controller = "Home"
                    }));
                    return;
                }
                if (!roleWiseScreenPermissions.Any(x => !string.IsNullOrWhiteSpace(x.MenuURL) && x.MenuURL.ToLower() == menuURL.ToLower()))
                {
                    bool isMenuAvailable = false;
                    foreach (var item in roleWiseScreenPermissions)
                    {
                        if (item.ChildMenuViewModels.Any(x => !string.IsNullOrWhiteSpace(x.MenuURL) && x.MenuURL.ToLower() == menuURL.ToLower()))
                        {
                            isMenuAvailable = true;
                            break;
                        }
                    }
                    if (!isMenuAvailable)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            action = "PermissionDenied",
                            controller = "Home"
                        }));
                        return;
                    }
                }
                //if (!roleWiseScreenPermissions.Any(a => a.ControllerName == controllerName))
                //{
                //    bool isMenuAvailable = false;
                //    foreach (var item in roleWiseScreenPermissions)
                //    {
                //        if (item.ChildMenuViewModels != null && item.ChildMenuViewModels.Count > 0 && !item.ChildMenuViewModels.Any(a => a.MenuURL == menuURL))
                //        {
                //            isMenuAvailable = false;
                //        }
                //        else
                //        {
                //            isMenuAvailable = true;
                //            break;
                //        }
                //    }
                //    if (isMenuAvailable == false)
                //    {
                //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //        {
                //            action = "PermissionDenied",
                //            controller = "Home"
                //        }));
                //    }
                //}
                //else
                //{
                //    foreach (var item in roleWiseScreenPermissions)
                //    {
                //        if (item.ChildMenuViewModels != null && item.ChildMenuViewModels.Count > 0 && item.ChildMenuViewModels.Any(a => a.ControllerName == controllerName) && !item.ChildMenuViewModels.Any(a => a.ActionName == actionName))
                //        {
                //            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //            {
                //                action = "PermissionDenied",
                //                controller = "Home"
                //            }));
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.Information(WebUtility.HtmlEncode(ex.ToString()));
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "PermissionDenied",
                    controller = "Home"
                }));
            }
        }
    }
}
