using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.Settings
{
    public interface ISettingService
    {
        List<SettingDto> GetAllSetting();
        Task<bool> DeactiveSetting(int? settingId);
        Task<bool> ActiveSetting(int? settingId);
        Task<SettingDto> GetSettingById(int? id);
        List<SettingDto> GetSettingByParentId(int? settingId);
        List<SettingDto> GetSettingsNoParentId();
        Task<bool> AddNewSetting(SettingDto setting);
        Task<bool> EditSetting(SettingDto setting);
        bool IsDuplicateSetting(SettingDto item);
        List<SettingDto> OrderSettingsParent();
    }
}
