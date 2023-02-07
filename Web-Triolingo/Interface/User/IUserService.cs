using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.User
{
    public interface IUserService
    {
        Task<UserDto> Login(UserLoginDto user);
    }
}
