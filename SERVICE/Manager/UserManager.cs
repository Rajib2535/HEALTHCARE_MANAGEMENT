using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using CORPORATE_DISBURSEMENT_UTILITY;
using DATA.Interface;
using DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using SERVICE.Interface;
using System.Net;
using System.Text;

namespace SERVICE.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _UserRepository;
        private readonly IHttpContextAccessor _iHttpContextAccesor;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public UserManager(IUserRepository UserRepository, IHttpContextAccessor iHttpContextAccesor, ILogger logger, IConfiguration configuration)
        {
            _UserRepository = UserRepository;
            _iHttpContextAccesor = iHttpContextAccesor;
            _logger = logger;
            _configuration = configuration;

        }
        /// <summary>
        /// check if user exists
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> IsFirstAdminExist(string userName, string password)
        {
            string secretKey = _configuration.GetValue<string>("ApplicationConfiguration:SecretHashKey");
            string decryptedPassword = string.Empty;
            //password = Util.Encrypt(password, true);
            //desco_app_db_admin_dbcontext db = new desco_app_db_admin_dbcontext();
            var userInfo = await _UserRepository.GetUserInfoByUserName(userName);
            if (userInfo.Count > 0)
            {
                decryptedPassword = AESEncryption.DecryptString(secretKey ?? string.Empty, userInfo.First().Password ?? string.Empty);
                if (decryptedPassword.Trim() == password.Trim())
                {
                    return true;
                }
                return false;
            }

            return false;
        }
        /// <summary>
        /// user delete method
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public async Task<int> DeleteUser(User User)
        {
            try
            {
                return await _UserRepository.DeleteUser(User);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserManager->DeleteUser. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// get user information by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUserInfoListByUserName(string userName)
        {
            try
            {
                return await _UserRepository.GetUserInfoByUserName(userName);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserManager->GetUserInfoListByUserName. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// load user information list
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserInfoViewModel>> GetUserInfos()
        {
            List<UserInfoViewModel> userInfoViewModels = new();
            try
            {
                var data = await _UserRepository.GetUserInfos();
                if (data.Count > 0)
                {
                    userInfoViewModels = data.Select(a => new UserInfoViewModel
                    {
                        CreateTime = a.CreateTime,
                        Email = a.Email,
                        Id = a.Id,
                        LoginType = a.LoginType,
                        Mobile = a.Mobile,
                        Name = a.Name,
                        Password = a.Password,
                        Status = a.Status,
                        StatusInput = Convert.ToBoolean(Convert.ToInt32(a.Status)),
                        Type = a.Type,
                        UpdateTime = a.UpdateTime,
                        Userid = a.Userid,

                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"UserManager->GetUserInfos. Error: {ex.ToString()}");
                throw;
            }
            return userInfoViewModels;
        }
        /// <summary>
        /// load current user info from httpcontext
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetCurrentUserInfo()
        {
            User User = new();
            try
            {
                var sessionInfo = _iHttpContextAccesor.HttpContext.Session.Keys.ToList();
                if (sessionInfo.Count > 0)
                {
                    User = (await _UserRepository.GetUserInfoByUserName(_iHttpContextAccesor.HttpContext.Session.GetString("UserName"))).FirstOrDefault();
                }


            }
            catch (Exception ex)
            {
                _logger.Error($"UserManager->GetCurrentUserInfo. Error: {ex.ToString()}");
                throw;
            }
            return User;
        }
        /// <summary>
        /// loads single user information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfoViewModel> GetUserInfoById(int id)
        {
            UserInfoViewModel userInfoViewModel = new UserInfoViewModel();
            try
            {
                string secretKey = _configuration.GetValue<string>("ApplicationConfiguration:SecretHashKey") ?? string.Empty;
                var data = await _UserRepository.GetUserInfoById(id);
                if (data != null)
                {
                    var currentUserInfo = await GetCurrentUserInfo();
                    userInfoViewModel = new UserInfoViewModel
                    {
                        CreateTime = data.CreateTime,
                        Email = data.Email,
                        Id = data.Id,
                        LoginType = data.LoginType,
                        Mobile = data.Mobile,
                        Name = data.Name,
                        Password = AESEncryption.DecryptString(secretKey, data.Password ?? string.Empty),
                        Status = data.Status,
                        StatusInput = Convert.ToBoolean(Convert.ToInt32(data.Status)),
                        Type = data.Type,
                        UpdateTime = data.UpdateTime,
                        Userid = data.Userid,
                        SaltedPassword = data.Password,
                        CreatedBy = currentUserInfo.Id,

                    };
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"UserManager->GetUserInfoById. Error: {ex.ToString()}");
                throw;
            }
            return userInfoViewModel;
        }
        /// <summary>
        /// creates user information (old)
        /// </summary>
        /// <param name="DummyUserRole"></param>
        /// <returns></returns>
        public async Task<int> SaveUserInfoByDummyUserRole(DummyUserRole DummyUserRole)
        {
            try
            {
                User userInfo = new User();
                userInfo.Name = DummyUserRole.userName;
                userInfo.Userid = "";
                //userInfo.Status = true;
                userInfo.CreateTime = DateTime.Now;
                return await _UserRepository.SaveUserInfoByDummyRoleName(userInfo);
            }
            catch (Exception ex)
            {
                _logger.Error($"UserManager->SaveUserInfoByDummyUserRole. Error: {ex.ToString()}");
                throw;
            }
        }
        /// <summary>
        /// creates user method
        /// </summary>
        /// <param name="userInfoViewModel"></param>
        /// <returns></returns>
        public async Task<int> AddUser(UserInfoViewModel userInfoViewModel)
        {
            try
            {
                var currentUserInfo = await GetCurrentUserInfo();
                int max_user_id = await _UserRepository.GetMaxUserId();
                string secretKey = _configuration.GetValue<string>("ApplicationConfiguration:SecretHashKey") ?? string.Empty;
                User userInfo = new()
                {
                    CreateTime = DateTime.Now,
                    Email = userInfoViewModel.Email,
                    LoginType = userInfoViewModel.LoginType,
                    Mobile = userInfoViewModel.Mobile,
                    Name = userInfoViewModel.Name,
                    Password = AESEncryption.EncryptString(secretKey, userInfoViewModel.Password ?? string.Empty),
                    Status = userInfoViewModel.StatusInput,
                    Type = userInfoViewModel.Type,
                    UpdateTime = DateTime.Now,
                    Userid = Convert.ToString(max_user_id + 1),
                    CreatedBy = currentUserInfo.Id,
                    IsPasswordReset = true,
                    UpdatedBy = currentUserInfo.Id,


                };
                return await _UserRepository.AddUser(userInfo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// update user information method
        /// </summary>
        /// <param name="userInfoViewModel"></param>
        /// <returns></returns>
        public async Task<int> EditUser(UserInfoViewModel userInfoViewModel)
        {
            try
            {
                var currentUserInfo = await GetCurrentUserInfo();
                string secretKey = _configuration.GetValue<string>("ApplicationConfiguration:SecretHashKey");
                string savedPassword = string.Empty;
                string incomingPassword = AESEncryption.EncryptString(secretKey, userInfoViewModel.Password);
                if (userInfoViewModel.SaltedPassword == userInfoViewModel.Password)
                {
                    savedPassword = userInfoViewModel.Password;
                }
                else
                {
                    savedPassword = incomingPassword;
                }
                User userInfo = new User
                {
                    Id = userInfoViewModel.Id,
                    CreateTime = DateTime.Now,
                    Email = userInfoViewModel.Email,
                    LoginType = userInfoViewModel.LoginType,
                    Mobile = userInfoViewModel.Mobile,
                    Name = userInfoViewModel.Name,
                    Password = savedPassword,
                    Status = userInfoViewModel.StatusInput,
                    Type = userInfoViewModel.Type,
                    UpdateTime = DateTime.Now,
                    Userid = userInfoViewModel.Userid,
                    CreatedBy = userInfoViewModel.CreatedBy,
                    UpdatedBy = currentUserInfo.UpdatedBy,
                    IsPasswordReset = userInfoViewModel.IsPasswordReset,

                };
                return await _UserRepository.EditUser(userInfo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// disable user information method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeactivateUser(int id)
        {
            int result = 0;
            try
            {
                var userInfoData = await _UserRepository.GetUserInfoById(id);
                if (userInfoData != null)
                {
                    var currentUserInfo = await GetCurrentUserInfo();
                    userInfoData.UpdatedBy = currentUserInfo.UpdatedBy;
                    userInfoData.UpdateTime = DateTime.Now;
                    userInfoData.Status = false;
                    result = await _UserRepository.EditUser(userInfoData);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async Task<User> GetUserInfoByEmail(string email)
        {
            User userInfoData = null;
            try
            {
                userInfoData = await _UserRepository.GetUserInfoByEmail(email);


            }
            catch (Exception)
            {

                throw;
            }
            return userInfoData;
        }

        /// <summary>
        /// resets user password to 1234
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> ResetUserPassword(int id)
        {
            int result = 0;
            try
            {
                var userData = await _UserRepository.GetUserInfoById(id);
                if (userData != null)
                {
                    var currentUserInfo = await GetCurrentUserInfo();
                    string secretKey = _configuration.GetValue<string>("ApplicationConfiguration:SecretHashKey");
                    userData.Password = AESEncryption.EncryptString(secretKey, "1234");
                    userData.UpdateTime = DateTime.Now;
                    userData.UpdatedBy = currentUserInfo.Id;
                    userData.IsPasswordReset = true;
                    result = await _UserRepository.EditUser(userData);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        /// <summary>
        /// password update when user is redirected to the update password page
        /// </summary>
        /// <param name="resetPasswordViewModel"></param>
        /// <returns></returns>
        public async Task<int> UpdateResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            int result = 0;
            try
            {
                var userData = await _UserRepository.GetUserInfoById(resetPasswordViewModel.UserId);
                if (userData != null)
                {
                    var currentUserInfo = await GetCurrentUserInfo();
                    string secretKey = _configuration.GetValue<string>("ApplicationConfiguration:SecretHashKey");
                    userData.Password = AESEncryption.EncryptString(secretKey, resetPasswordViewModel.Password ?? string.Empty);
                    userData.UpdateTime = DateTime.Now;
                    userData.UpdatedBy = userData.Id;
                    userData.IsPasswordReset = false;
                    result = await _UserRepository.EditUser(userData);
                }
            }
            catch (Exception ex)
            {

                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return result;
        }

        public async Task<List<DisplayTable>> GetDisplayTableUserList()
        {
            List<DisplayTable> result = new List<DisplayTable>();
            List<User> userInfos = await _UserRepository.GetUserInfos();
            if (userInfos.Any())
            {
                result = userInfos.Select(x => new DisplayTable
                {
                    Display = x.Name ?? string.Empty,
                    Key = x.Id.ToString()
                }).ToList();
            }
            return result;
        }

        public async Task<int> AddPatientInfo(Patient patient)
        {
           return await _UserRepository.AddPatientInfo(patient);
        }
    }
}
