using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.User;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.User
{
    public class UserControlService : IUserControlService
    {
        private readonly IMapper _mapper;
        public UserControlService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateUser(UserDto newUser)
        {
            Model.User newInstance = new Model.User()
            {
                Email = newUser.Email,
                FullName = newUser.FullName,
                Password = newUser.Password,
                AvatarUrl = newUser.AvatarUrl,
                Status = newUser.Status,
                Note = newUser.Note,
            };
            return await DataProvider.Ins.DB.SaveChangesAsync() > 0;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await DataProvider.Ins.DB.Users.ToListAsync();
            var mapped = _mapper.Map<List<UserDto>>(users);
            return mapped;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await DataProvider.Ins.DB.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            var mapped = _mapper.Map<UserDto>(user);
            return mapped;
        }

        public async Task<UserDto> GetUser(string email)
        {
            var user = await DataProvider.Ins.DB.Users.Where(user => user.Email.Equals(email)).FirstOrDefaultAsync();
            var mapped = _mapper.Map<UserDto>(user);
            return mapped;
        }

        public async Task<bool> RemoveUser(int id)
        {
            var user = await DataProvider.Ins.DB.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                DataProvider.Ins.DB.Remove(user);
                int removed = await DataProvider.Ins.DB.SaveChangesAsync();
                return removed > 0;
            }
            return true;
        }

        public async Task<bool> SwitchStatus(int id)
        {
            var user = await DataProvider.Ins.DB.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                // if Status 0 => 1
                // if Status 1 => 0
                user.Status = 1 - user.Status;
                DataProvider.Ins.DB.Update(user);
                int updated = await DataProvider.Ins.DB.SaveChangesAsync();
                return updated > 0;
            }
            return true;
        }

        public async Task<bool> UpdateUser(UserDto updateVal)
        {
            var user = await DataProvider.Ins.DB.Users.Where(user => user.Id == updateVal.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user = _mapper.Map<Model.User>(updateVal);
                DataProvider.Ins.DB.Update(user);
                int updated = await DataProvider.Ins.DB.SaveChangesAsync();
                return updated > 0;
            }
            return true;
        }
    }
}
