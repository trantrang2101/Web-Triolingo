using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Users
{
    public interface IUserService
    {
        Task<User> Login(User user);
        Task<bool> Regis(User user);
    }
}
