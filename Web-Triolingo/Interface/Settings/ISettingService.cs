using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.Settings
{
    public interface ISettingService
    {
        Task<List<SettingDto>> GetAllSetting();
        Task<bool> DeactiveSetting(int settingId);
    }
}
