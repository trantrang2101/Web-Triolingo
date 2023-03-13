using Microsoft.EntityFrameworkCore;
using Triolingo.Core.DataAccess;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Users;

namespace Web_Triolingo.ServiceManager.Users
{
    public class UserControlService : IUserControlService
    {
        private readonly TriolingoDbContext _dbContext;
        public UserControlService(TriolingoDbContext _context)
        {
            _dbContext = _context;
        }

        public async Task<bool> CreateUser(User newUser)
        {
            _dbContext.Users.Add(newUser);
			return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<User>> GetUsers() => await _dbContext.Users.ToListAsync();

        public async Task<User> GetUser(int id) => await _dbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<User> GetUser(string email) => await _dbContext.Users.Where(user => user.Email.Equals(email)).FirstOrDefaultAsync();

        public async Task<bool> RemoveUser(int id)
        {
            var user = await _dbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                int removed = await _dbContext.SaveChangesAsync();
                return removed > 0;
            }
            return true;
        }

        public async Task<bool> SwitchStatus(int id)
        {
            var user = await _dbContext.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                // if Status 0 => 1
                // if Status 1 => 0
                user.Status = 1 - user.Status;
                _dbContext.Users.Update(user);
                int updated = await _dbContext.SaveChangesAsync();
                return updated > 0;
            }
            return true;
        }

        public async Task<bool> UpdateUser(User updateVal)
        {
            var user = await _dbContext.Users.Where(user => user.Id == updateVal.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                if (!user.Email.Equals(updateVal.Email))
                {
                    user.Email = updateVal.Email;
                }
                if (!user.FullName.Equals(updateVal.FullName))
                {
                    user.FullName = updateVal.FullName;
                }
                if (!user.Password.Equals(updateVal.Password))
                {
                    user.Password = updateVal.Password;
                }
                if (user.Status != updateVal.Status)
                {
                    user.Status = updateVal.Status;
                }
                if (!string.Equals(user.Note, updateVal.Note))
                {
                    user.Note = updateVal.Note;
                }
				if (!string.Equals(user.AvatarUrl, updateVal.AvatarUrl))
				{
					user.AvatarUrl = updateVal.AvatarUrl;
				}
                _dbContext.Users.Update(user);
                int updated = await _dbContext.SaveChangesAsync();
                return updated > 0;
            }
            return true;
        }

		public List<User> GetUsers(string email) => _dbContext.Users.Where(user => user.Email.Equals(email)).ToList();
	}
}
