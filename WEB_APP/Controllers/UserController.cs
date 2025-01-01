using CORPORATE_DISBURSEMENT_ADMIN.Extensions;
using COMMON_SERVICE.Service;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using CORPORATE_DISBURSEMENT_UTILITY;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Text;
using ILogger = Serilog.ILogger;
using DATA.Interface;
using DATA.Models.ViewModels;
using SERVICE.Interface;
using DATA.Models;

namespace WEB_APP.Controllers
{
    /// <summary>
    /// used for CRUD operation of User, Login and Logout functionality
    /// </summary>
    public class UserController : Controller
    {

        #region Global Variable

        //desco_app_db_admin_dbcontext db = new desco_app_db_admin_dbcontext();
        const string userName = "";
        const string roleName = "";
        public static string auditdata = "";

        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserManager _UserManager;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMenuManager _menuManager;
        private readonly IRoleManager _roleManager;
        private readonly IUserRoleManager _userRoleManager;
       
       
        public UserController(IHttpContextAccessor httpContextAccessor, IConfiguration config, IUserManager UserManager, ILogger logger, IUserRepository userRepository, IMenuManager menuManager, IRoleManager roleManager,IUserRoleManager userRoleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _UserManager = UserManager;
            _logger = logger;
            _userRepository = userRepository;
            _menuManager = menuManager;
            _roleManager = roleManager;
            _userRoleManager = userRoleManager;
        }


        #endregion

        #region Index
        /// <summary>
        /// Action to load all user information page
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<IActionResult> Index()
        {
            var data = await _UserManager.GetUserInfos();
            _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonConvert.SerializeObject(data))}");
            return View(data);
        }
        /// <summary>
        /// action to load information of a user by id or new user popup in the page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AddOrEdit(int id = 0)
        {
            try
            {
              
                if (id == 0)
                {
                    var EmailConfiguration = new UserInfoViewModel();
                    return View(EmailConfiguration);
                }
                else
                {
                    var data = await _UserManager.GetUserInfoById(id);
                    return View(data);
                }
            }
            catch (Exception ex)
            {

                _logger.Error($"{GetActionWithControllerName} ==> {ex}");
                throw;
            }
        }
        /// <summary>
        /// action to save or update user information
        /// </summary>
        /// <param name="userInfoViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrEdit(UserInfoViewModel userInfoViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonConvert.SerializeObject(userInfoViewModel))}");
                    if (userInfoViewModel.Id == 0)
                    {

                        int result = await _UserManager.AddUser(userInfoViewModel);
                        userInfoViewModel.Id = result;
                    }
                    else
                    {
                        int result = await _UserManager.EditUser(userInfoViewModel);
                        var httpContext = _httpContextAccessor.HttpContext;
                        httpContext.Session.SetString("UserName", userInfoViewModel.Name);
                    }
                    return Json(new ResponseEntity { is_valid = true, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "_ViewAll", await _UserManager.GetUserInfos()) });
                }
                return Json(new ResponseEntity { is_valid = false, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "AddOrEdit", userInfoViewModel) });

            }
            catch (Exception ex)
            {
                _logger.Error($"{GetActionWithControllerName} ==> {WebUtility.HtmlEncode(ex.ToString())}");
                return Json(new ResponseEntity { is_valid = false, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "AddOrEdit", userInfoViewModel) });
            }

        }
        [HttpGet]
        public async Task<ActionResult> PatientRegistration(int id = 0)
        {
            try
            {
                    var model = new PatientRegistrationViewModel();
                    return View(model);
              
            }
            catch (Exception ex)
            {

                _logger.Error($"{GetActionWithControllerName} ==> {ex}");
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> PatientRegistration(PatientRegistrationViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    
                    UserInfoViewModel userInfoViewModel = new UserInfoViewModel()
                    {
                        Name = model.UserName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        Password = model.Password,
                        Status = true,
                        IsPasswordReset = false,
                        CreateTime = DateTime.Now,
                    };
                    var result = await _UserManager.AddUser(userInfoViewModel);
                    if ( result> 0)
                    {
                        Patient patient = new Patient()
                        {
                            Name = model.UserName,
                            Email = model.Email,
                            Address = model.Address,
                            Age = model.Age,
                            EmergencyContactName = model.EmergencyContactName,
                            Gender = model.Gender,
                            MedicalHistory = model.MedicalHistory,
                            PhoneNumber = model.Mobile,
                            EmergencyContactPhone = model.EmergencyContactPhone,
                            UserId = result,
                            Weight = model.Weight,
                            CreatedAt = DateTime.Now,
                        };
                        if (await _UserManager.AddPatientInfo(patient) > 0) {
                            var role = await _roleManager.GetRoleByType(3);
                            UserRole userRole = new UserRole()
                            {
                                RoleId = role.Id,
                                UserId = result,
                            };
                            await _userRoleManager.AddUserRole(userRole);
                            TempData["PatientRegistation"] = "User Create successfully";
                            return RedirectToAction("Login");
                        }
                       
                    }
                    return View(model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error($"{GetActionWithControllerName} ==> {WebUtility.HtmlEncode(ex.ToString())}");
                return View(model);
            }
        }
        /// <summary>
        /// action to delete/disable a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {


                var result = await _UserManager.DeactivateUser(id);
                _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonConvert.SerializeObject(result))}");
                return Json(new ResponseEntity { is_valid = true, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "_ViewAll", await _UserManager.GetUserInfos()) });


            }
            catch (Exception ex)
            {
                _logger.Error($"{GetActionWithControllerName} ==> {WebUtility.HtmlEncode(ex.ToString())}");
                return Json(new ResponseEntity { is_valid = false, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "_ViewAll", await _UserManager.GetUserInfos()) });
            }

        }
        /// <summary>
        /// action to reset the password of a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int id)
        {
            try
            {


                var result = await _UserManager.ResetUserPassword(id);
                _logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonConvert.SerializeObject(result))}");
                return Json(new ResponseEntity { is_valid = true, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "_ViewAll", await _UserManager.GetUserInfos()) });


            }
            catch (Exception ex)
            {
                _logger.Error($"{GetActionWithControllerName} ==> {WebUtility.HtmlEncode(ex.ToString())}");
                return Json(new ResponseEntity { is_valid = false, session_expired = false, html = RazorHelper.RenderRazorViewToString(this, "_ViewAll", await _UserManager.GetUserInfos()) });
            }

        }
        /// <summary>
        /// to get the current action and controller name
        /// </summary>
        /// <returns></returns>
        private string GetActionWithControllerName()
        {
            string result = string.Empty;
            try
            {
                string actionName = Convert.ToString(ControllerContext.RouteData.Values["action"]);
                string controllerName = Convert.ToString(ControllerContext.RouteData.Values["controller"]);
                result = controllerName + "/" + actionName;
                _logger.Information("URL Generation ==> " + WebUtility.HtmlEncode(result));
            }
            catch (Exception ex)
            {
                _logger.Error($"{WebUtility.HtmlEncode(ex.ToString())}");
                result = string.Empty;
            }
            return WebUtility.HtmlEncode(result);
        }

        #endregion

        #region Login Entity
        /// <summary>
        /// action to load the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            _logger.Information($"{GetActionWithControllerName()} ==> Login Initialized!");
            return View();
        }
        /// <summary>
        /// action to authorize user login by username and password
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            //_logger.Information($"{GetActionWithControllerName()} ==> {WebUtility.HtmlEncode(JsonSerializer.Serialize(login))}");
            try
            {
                if (ModelState.IsValid)
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    if (await _UserManager.IsFirstAdminExist(login.userName ?? string.Empty, login.password ?? string.Empty) == true && httpContext != null)

                    {
                        string roleName = await _userRepository.getRoleofUser(login.userName ?? string.Empty);
                        string userName = login.userName ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(roleName))
                        {


                            httpContext.Session.SetString("UserName", userName);
                            httpContext.Session.SetString("RoleName", roleName);
                            var data = await _menuManager.GetRoleWiseMenuList(roleName);
                            //httpContext.Session.SetComplexData("RoleWiseScreenPermission", data);
                            httpContext.Session.SetString("RoleWiseScreenPermission", JsonConvert.SerializeObject(data));
                            string role_name = httpContext.Session.GetString("RoleName");
                            string user_name = httpContext.Session.GetString("UserName");
                            //DBContext dBContext = new(role_name ?? string.Empty, user_name ?? string.Empty);
                            //ErrorController errorController = new ErrorController(_httpContextAccessor.HttpContext.Session.GetString(roleName).ToString());


                            ViewBag.userName = user_name;
                            ViewBag.roleName = role_name;

                            _logger.Information("login successful from username: " + WebUtility.HtmlEncode(userName) + "|| rolename: " + WebUtility.HtmlEncode(roleName) + "");


                            var currentUserInfo = await _UserManager.GetCurrentUserInfo();
                            if (currentUserInfo != null && currentUserInfo.Status == true)
                            {
                                if (currentUserInfo.IsPasswordReset == true)
                                {
                                    string secretKey = _config.GetValue<string>("ApplicationConfiguration:SecretHashKey");
                                    var resetPasswordModel = new ResetPasswordViewModel
                                    {
                                        UserId = currentUserInfo.Id,
                                        Password = null,
                                        ConfirmPassword = null,
                                        CurrentPassword = AESEncryption.DecryptString(secretKey ?? string.Empty, currentUserInfo.Password ?? string.Empty)
                                    };
                                    return View("ResetPassword", resetPasswordModel);
                                }
                                else
                                {
                                    
                                    httpContext.Session.SetString("user_id", currentUserInfo.Id.ToString());
                                    if (data.Any(a => a.ControllerName == "Home" && a.ActionName == "Dashboard"))
                                    {
                                        return RedirectToAction("Dashboard", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }

                                }
                            }
                            else
                            {
                                ViewBag.failed = "User is inactive, please contact the admininistrator.";
                                httpContext.Session.Clear();
                            }


                        }
                        else
                        {
                            ViewBag.failed = "Role not found, please contact the admininistrator.";
                            httpContext.Session.Clear();
                        }
                    }
                    else
                    {
                        ViewBag.failed = "Username or password is incorrect";
                    }


                    //else
                    //{
                    //    if (loginValidation("", login.userName, login.password))
                    //    {
                    //        string roleName = LoginService.getRoleofUser(login.userName);
                    //        if (roleName != "")
                    //        {
                    //            var httpContext = _httpContextAccessor.HttpContext;
                    //            httpContext.Session.SetString("UserName", login.userName);
                    //            httpContext.Session.SetString("RoleName", roleName);
                    //            DBContext dBContext = new DBContext(_httpContextAccessor.HttpContext.Session.GetString("RoleName").ToString(),
                    //                _httpContextAccessor.HttpContext.Session.GetString("UserName").ToString());

                    //            ViewBag.userName = _httpContextAccessor.HttpContext.Session.GetString("UserName").ToString();
                    //            ViewBag.roleName = _httpContextAccessor.HttpContext.Session.GetString("RoleName").ToString();



                    //        }

                    //        _logger.Information("login successful from username: " + WebUtility.HtmlEncode(userName) + "|| rolename: " + WebUtility.HtmlEncode(roleName) + "");

                    //        //return RedirectToAction("Action", "Error");
                    //        return RedirectToAction("show", "Dashboard");
                    //    }
                    //    else
                    //    {
                    //        _httpContextAccessor.HttpContext.Session.Clear();
                    //        _logger.Error("login failure from username: " + WebUtility.HtmlEncode(login.userName) + "");

                    //        ViewBag.failed = "Username or password is incorrect";
                    //        return View(login);
                    //    }
                    //}
                }
                else
                {
                    _logger.Error("login failure: " + WebUtility.HtmlEncode(JsonConvert.SerializeObject(login)) + "");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {

                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }


            return View(login);
        }

        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public async Task<IActionResult> ResetPasswordSettings(string email)
        {
            string secretKey = _config.GetValue<string>("ApplicationConfiguration:SecretHashKey");
            var data = AESEncryption.DecryptString(secretKey ?? string.Empty, email ?? string.Empty);
            var user = await _UserManager.GetUserInfoByEmail(data);
            var resetPasswordModel = new ResetPasswordViewModel
            {
                UserId = user.Id,
                Password = null,
                ConfirmPassword = null,
                CurrentPassword = AESEncryption.DecryptString(secretKey ?? string.Empty, user.Password ?? string.Empty)
            };
            return View("ResetPassword", resetPasswordModel);
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
                    return RedirectToAction("Login");
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
            return View("ResetPassword", resetPasswordViewModel);
        }
        /// <summary>
        /// active directory method for windows login
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool loginValidation(string domainName, string username, string password)
        {
            try
            {
                var adconfigpath = _config.GetValue<string>("ADDirectory:ADDirectoryPath"); // "Information"
                //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //string path = (string)settingsReader.GetValue("AD_CONFIG", typeof(String));
                //path = "LDAP://ou=users,ou=bd-dhaka,ou=measa,dc=alico,dc=corp";
                //string path = "LDAP://DC=sslwireless,DC=com";

                string path = adconfigpath.ToString();
                DirectoryEntry de = new DirectoryEntry(path, username, password, AuthenticationTypes.Secure);
                DirectorySearcher ds = new DirectorySearcher(de);

                try
                {
                    ds.FindOne();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    //return true;
                }
            }
            catch (Exception e)
            {
                //return false;
                return true;
            }
        }



        [HttpGet]
        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);
            var user = await _UserManager.GetUserInfoByEmail(forgotPasswordModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));


            string secretKey = _config.GetValue<string>("ApplicationConfiguration:SecretHashKey");
            var email = AESEncryption.EncryptString(secretKey ?? string.Empty, user.Email ?? string.Empty);
            var callback = Url.Action(nameof(ResetPasswordSettings), "User", new { email }, Request.Scheme);
           

            var smtpClient = _config.GetValue<string>("Smtp:Host");
            var fromAdd = _config.GetValue<string>("Smtp:FromMail");
            var smtpPort = _config.GetValue<int>("Smtp:Port");
            var smtpUser = _config.GetValue<string>("Smtp:Username");
            var smtpPass = _config.GetValue<string>("Smtp:Password");

           
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [TypeFilter(typeof(CheckSessionWithRedirect), Order = 1)]
        [TypeFilter(typeof(CheckPermission), Order = 2)]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        #endregion
        /// <summary>
        /// action to log out a user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        #region Log Out
        public async Task<IActionResult> Logout()
        {
            bool status = true;
            try
            {


                #region Audit Log
                //DBContext dB = new DBContext(_httpContextAccessor.HttpContext.Session.GetString(roleName).ToString(),
                //    _httpContextAccessor.HttpContext.Session.GetString(userName).ToString());
                var sessionInfo = _httpContextAccessor.HttpContext.Session.Keys.ToList();
                string client_id = string.Empty;
                string remarks = string.Empty;
                string roleName = string.Empty;
                if (sessionInfo.Count > 0)
                {
                    client_id = _httpContextAccessor.HttpContext.Session.GetString("UserName");
                    roleName = _httpContextAccessor.HttpContext.Session.GetString("RoleName");
                    remarks = "Logged out successfully by user: " + client_id + " || role: " + roleName + "";
                    _logger.Information(remarks);
                }

                #endregion

                _httpContextAccessor.HttpContext.Session.SetString("UserName", "");
                _httpContextAccessor.HttpContext.Session.SetString("RoleName", "");

                //DBContext dBContext = new DBContext(_httpContextAccessor.HttpContext.Session.GetString(roleName).ToString(),
                //     _httpContextAccessor.HttpContext.Session.GetString(userName).ToString());
                _httpContextAccessor.HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                status = false;
                return RedirectToAction("Login");
            }
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

    }



    #region Domain Manager
    public static class DomainManager
    {
        static DomainManager()
        {
            Domain domain = null;
            DomainController domainController = null;
            try
            {
                domain = Domain.GetCurrentDomain();
                DomainName = domain.Name;
                domainController = domain.PdcRoleOwner;
                DomainControllerName = domainController.Name.Split('.')[0];
                ComputerName = Environment.MachineName;
            }
            finally
            {
                if (domain != null)
                    domain.Dispose();
                if (domainController != null)
                    domainController.Dispose();
            }
        }

        public static string DomainControllerName { get; private set; }

        public static string ComputerName { get; private set; }

        public static string DomainName { get; private set; }

        public static string DomainPath
        {
            get
            {
                bool bFirst = true;
                StringBuilder sbReturn = new StringBuilder(200);
                string[] strlstDc = DomainName.Split('.');
                foreach (string strDc in strlstDc)
                {
                    if (bFirst)
                    {
                        sbReturn.Append("DC=");
                        bFirst = false;
                    }
                    else
                        sbReturn.Append(",DC=");

                    sbReturn.Append(strDc);
                }
                return sbReturn.ToString();
            }
        }

        public static string RootPath
        {
            get
            {
                return string.Format("LDAP://{0}/{1}", DomainName, DomainPath);
            }
        }
    }

    #endregion
}
