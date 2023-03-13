using Triolingo.Core.DataAccess;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.UserRoles;

namespace Web_Triolingo.ServiceManager.UserRoles
{
    public class UserRoleService : IUserRoleService
    {
        private const string SETTING_VALUE = "ROLE";
        private readonly TriolingoDbContext _dbContext;

        public UserRoleService(TriolingoDbContext _context)
        {
            _dbContext = _context;
        }

        public bool AddRoleForUser(UserRole userRole)
        {
            _dbContext.UserRoles.Add(userRole);
            return _dbContext.SaveChanges() > 0;
        }

        public Dictionary<int, string> GetAllRoles()
        {
            Setting parentSetting = _dbContext.Settings.FirstOrDefault(setting => setting.ParentId == null && setting.Value == SETTING_VALUE);
            Dictionary<int, string> roles = new Dictionary<int, string>();
            if (parentSetting != null)
            {
                var roles_setting = _dbContext.Settings.Where(setting => setting.ParentId == parentSetting.Id);
                foreach (var role in roles_setting)
                {
                    roles.Add(role.Id, role.Name);
                }
            }
			return roles;
        }

        public UserRole GetRole(int id)
        {
            return _dbContext.UserRoles.FirstOrDefault(role => role.Id == id);
        }

        public UserRole GetRoleOfUser(int userId)
        {
            return _dbContext.UserRoles.FirstOrDefault(role => role.UserId == userId);
        }

		public Setting GetRoleSetting(int settingId)
		{
			return _dbContext.Settings.FirstOrDefault(setting => setting.Id == settingId);
		}

		public bool RemoveRole(int id)
        {
            UserRole role = GetRole(id);
            if (role != null)
            {
                _dbContext.UserRoles.Remove(role);
                return _dbContext.SaveChanges() > 0;
            }
            return true;
        }

        public bool RemoveRoleFromUser(int userId)
        {
            var roles = _dbContext.UserRoles.Where(role => role.UserId == userId);
            if (roles.Any())
            {
                _dbContext.UserRoles.RemoveRange(roles);
                return _dbContext.SaveChanges() > 0;
            }
            return true;
        }

        public bool UpdateRoleOfUser(UserRole role)
        {
            _dbContext.UserRoles.Update(role);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
