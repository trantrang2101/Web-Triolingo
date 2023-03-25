using Microsoft.EntityFrameworkCore;
using Triolingo.Core.DataAccess;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.UserRoles;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Web_Triolingo.ServiceManager.UserRoles
{
    public class UserRoleService : IUserRoleService
    {
        private const string ADMIN_SETTING_VALUE = "ROLE_ADMIN";
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

        public List<User> GetUsersByRole(String roleName)
        {
            Setting parentSetting = _dbContext.Settings.FirstOrDefault(setting => setting.Name.Contains(roleName) && setting.ParentSetting.Value == SETTING_VALUE);
            if(parentSetting != null)
            {
                List<User> users = _dbContext.UserRoles.Include(x => x.User).Where(x=>x.RoleType==parentSetting.Id).Select(x=>x.User).ToList();
                return users.Where(x=>x.Status>0).ToList();
            }
            return null;
        }

		public bool DoesUserHaveRole(int userId, int roleSettingId)
		{
			var query = from userRole in _dbContext.UserRoles
						where userRole.UserId == userId
                        select userRole;
            return query.Any(role => (role.Setting == null ? role.RoleType : role.Setting.Id) == roleSettingId);
		}

        public IEnumerable<UserRoleInfo> GetAllRoleOfUser(int userId)
        {
			Setting parentSetting = _dbContext.Settings.FirstOrDefault(setting => setting.ParentId == null && setting.Value == SETTING_VALUE);

            var roles = from setting in _dbContext.Settings
                        join userRole in _dbContext.UserRoles.Where(ur => ur.UserId == userId)
                          on setting.Id equals userRole.RoleType
                          into userRole_setting
                        from userRoleInfo in userRole_setting.DefaultIfEmpty()
                        where setting.ParentId == parentSetting.Id
                        select new UserRoleInfo
                        {
                            IsActivated = userRoleInfo != null,
                            SettingId = setting.Id,
                            RoleName = setting.Name,
                            RoleNote = userRoleInfo == null ? null : userRoleInfo.Note,
                        };

            return roles;
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

		public bool UpdateRoleOfUser(int userId, IEnumerable<UserRoleInfo> roles)
		{
            var currentRole = from userRole in _dbContext.UserRoles
                              where userRole.UserId == userId
                              select userRole;
			foreach(var role in roles)
            {
                UserRole roleEntity = currentRole.Include(r => r.Setting).FirstOrDefault(r => r.Setting.Id == role.SettingId);
                if (role.IsActivated && roleEntity == null)
                {
                    // add new row
                    var newRow = new UserRole
                    {
                        UserId = userId,
                        User = _dbContext.Users.Find(userId),
                        Setting = _dbContext.Settings.Find(role.SettingId),
                        RoleType = role.SettingId,
                    };
                    _dbContext.UserRoles.Add(newRow);
					_dbContext.SaveChanges();
				}
                else if (role.IsActivated)
                {
					// update
					if (roleEntity.Setting.Id != role.SettingId)
                    {
                        roleEntity.Setting = _dbContext.Settings.Find(role.SettingId);
                        roleEntity.RoleType = role.SettingId;
                    }
                    if (string.Compare(roleEntity.Note, role.RoleNote) != 0)
                    {
                        roleEntity.Note = role.RoleNote;
                    }
					_dbContext.UserRoles.Update(roleEntity);
					_dbContext.SaveChanges();
				}
                else if (roleEntity != null)
                {
					// delete
					_dbContext.UserRoles.Remove(roleEntity);
					_dbContext.SaveChanges();
				}
            }
            return true;
		}

        public Setting GetAdminSetting()
        {
            Setting parentSetting = _dbContext.Settings.FirstOrDefault(setting => setting.ParentId == null && setting.Value == SETTING_VALUE);
            return (from setting in _dbContext.Settings
                    where setting.ParentId == parentSetting.Id &&
                        setting.Value == ADMIN_SETTING_VALUE
                    select setting).FirstOrDefault();
        }
    }

	public class UserRoleInfo
	{
		public bool IsActivated { get; set; }
		public int SettingId { get; set; }
		public string RoleName { get; set; }
		public string RoleNote { get; set; }
	}
}
