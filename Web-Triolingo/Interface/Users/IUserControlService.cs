using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Users
{
    public interface IUserControlService
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(int id);
        List<User> GetUsers(string email);
        Task<User> GetUser(string email);
        Task<bool> UpdateUser(User updateVal);
        Task<bool> SwitchStatus(int id);
        Task<bool> CreateUser(User newUser);
    }
}
