using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;
using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Settings
{
    public interface ISettingService
    {
        List<Setting> GetAllSetting();
        Task<bool> DeactiveSetting(int? settingId);
        Task<bool> ActiveSetting(int? settingId);
        Task<Setting> GetSettingById(int? id);
        List<Setting> GetSettingByParentId(int? settingId);
        List<Setting> GetSettingsNoParentId();
        Task<bool> AddNewSetting(Setting setting);
        Task<bool> EditSetting(Setting setting);
        bool IsDuplicateSetting(Setting item);
        List<Setting> OrderSettingsParent();
    }
}
