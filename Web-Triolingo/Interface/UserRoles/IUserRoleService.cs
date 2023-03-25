using Microsoft.EntityFrameworkCore.Storage;
using Triolingo.Core.Entity;
using Web_Triolingo.ServiceManager.UserRoles;

namespace Web_Triolingo.Interface.UserRoles
{
    public interface IUserRoleService
    {
        Dictionary<int, string> GetAllRoles();
        UserRole GetRole(int id);
        UserRole GetRoleOfUser(int userId);
        bool UpdateRoleOfUser(UserRole role);
        bool AddRoleForUser(UserRole userRole);
        bool RemoveRole(int id);
        bool RemoveRoleFromUser(int userId);
        Setting GetRoleSetting(int settingId);
        bool DoesUserHaveRole(int userId, int roleSettingId);
        IEnumerable<UserRoleInfo> GetAllRoleOfUser(int userId);
        bool UpdateRoleOfUser(int userId, IEnumerable<UserRoleInfo> roles);
	}
}
