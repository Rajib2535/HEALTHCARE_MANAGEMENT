using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Models;

namespace SERVICE.Interface
{
    public interface IUserManager
    {
        Task<List<UserInfoViewModel>> GetUserInfos();
        Task<User> GetCurrentUserInfo();
        Task<User> GetUserInfoByEmail(string email);
        Task<List<User>> GetUserInfoListByUserName(string userName);
        Task<int> SaveUserInfoByDummyUserRole(DummyUserRole UserInfo);
        Task<int> DeleteUser(User UserInfo);
        Task<UserInfoViewModel> GetUserInfoById(int id);
        Task<int> AddUser(UserInfoViewModel userInfoViewModel);
        Task<int> EditUser(UserInfoViewModel userInfoViewModel);
        Task<bool> IsFirstAdminExist(string userName, string password);
        Task<int> DeactivateUser(int id);
        Task<int> ResetUserPassword(int id);
        Task<int> UpdateResetPassword(ResetPasswordViewModel resetPasswordViewModel);
        Task<List<DisplayTable>> GetDisplayTableUserList();
        Task<int> AddPatientInfo(Patient patient);

    }
}
