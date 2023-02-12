using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.Settings
{
    public interface ISettingService
    {
        Task<List<SettingDto>> GetAllSetting();
        Task<bool> DeactiveSetting(int? settingId);
        Task<bool> ActiveSetting(int? settingId);
        Task<SettingDto> GetSettingById(int? id);
        Task<List<SettingDto>> GetSettingByParentId(int? settingId);
        Task<List<SettingDto>> GetSettingsNoParentId();
        Task<bool> AddNewSetting(SettingDto setting);
    }
}
