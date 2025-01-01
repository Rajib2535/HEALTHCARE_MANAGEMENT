using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net;
using ILogger = Serilog.ILogger;
using CORPORATE_DISBURSEMENT_UTILITY;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using CORPORATE_DISBURSEMENT_ADMIN.Extensions;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DATA.Interface;
using SERVICE.Interface;

namespace WEB_APP.Controllers
{
    /// <summary>
    /// used for CRUD operation of User, Login and Logout functionality
    /// </summary>
    public class SettingsController : Controller
    {

        #region Global Variable

        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManager _UserManager;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMenuManager _menuManager;
        public SettingsController(IHttpContextAccessor httpContextAccessor, IConfiguration config, IUserManager UserManager, ILogger logger, IUserRepository userRepository,IMenuManager menuManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _UserManager = UserManager;
            _logger = logger;
            _userRepository = userRepository;
            _menuManager = menuManager;
        }


        #endregion
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<IActionResult> PasswordSetting()
        {
            var currentUserInfo = await _UserManager.GetCurrentUserInfo();
            string? secretKey = _config.GetValue<string>("ApplicationConfiguration:SecretHashKey");
            var resetPasswordModel = new ResetPasswordViewModel
            {
                UserId = currentUserInfo.Id,
                Password = null,
                ConfirmPassword = null,
                CurrentPassword = AESEncryption.DecryptString(secretKey ?? String.Empty, currentUserInfo.Password ?? string.Empty)
            };
            return View("PasswordSetting", resetPasswordModel);
        }
        /// <summary>
        /// action to update user password
        /// </summary>
        /// <param name="resetPasswordViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _UserManager.UpdateResetPassword(resetPasswordViewModel);
                    TempData["PasswordChangeMessage"] = "Password updated successfully";
                    
                }
                else
                {
                    ViewBag.failed = "Please try again";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
                ViewBag.failed = ex.ToString();
            }
            return View("PasswordSetting", resetPasswordViewModel);
        }
    }
   
}
