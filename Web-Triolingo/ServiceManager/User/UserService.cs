using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.User;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Models;

namespace Web_Triolingo.ServiceManager.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<UserDto> Login(UserLoginDto user)
        {
            var userLoged = await DataProvider.Ins.DB.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefaultAsync();
            var result = _mapper.Map<UserDto>(userLoged);
            return result;
        }
        public async Task<UserDto> FindExistEmail(string email)
        {
            var userLoged = await DataProvider.Ins.DB.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            var result = _mapper.Map<UserDto>(userLoged);
            return result;
        }
        public async Task<bool> Regis(UserRegisDto user)
        {
            var check = FindExistEmail(user.Email);
            if (check == null)
            {
                Web_Triolingo.Models.User newUser = new Web_Triolingo.Models.User()
                {
                    Email= user.Email,
                    Password= user.Password,
                    FullName= user.FullName,
                };
                await DataProvider.Ins.DB.Users.AddAsync(newUser);
                await DataProvider.Ins.DB.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
