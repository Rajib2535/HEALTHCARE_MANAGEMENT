using DATA.Models;

namespace DATA.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUserInfos();
        Task<List<User>> GetUserInfoByUserName(string userName);
        Task<int> SaveUserInfoByDummyRoleName(User UserInfo);
        Task<int> DeleteUser(User UserInfo);
        Task<User> GetUserInfoById(int id);
        Task<User> GetUserInfoByEmail(string email);
        Task<int> AddUser(User userInfo);
        Task<int> EditUser(User userInfo);
        Task<int> GetMaxUserId();
        Task<string> getRoleofUser(string v);
        Task<int> AddPatientInfo(Patient patient);
    }
}
