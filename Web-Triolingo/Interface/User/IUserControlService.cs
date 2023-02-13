using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.User
{
    public interface IUserControlService
    {
        Task<List<UserDto>> GetUsers();
        Task<UserDto> GetUser(int id);
        Task<UserDto> GetUser(string email);
        Task<bool> UpdateUser(UserDto updateVal);
        Task<bool> SwitchStatus(int id);
        Task<bool> CreateUser(UserDto newUser);
    }
}
