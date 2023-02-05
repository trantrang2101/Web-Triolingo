using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.Settings
{
    public class SettingService : ISettingService
    {
        private readonly IMapper _mapper;
        public SettingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<SettingDto>> GetAllSetting()
        {

            var settings = await DataProvider.Ins.DB.Settings.ToListAsync();
            var result = _mapper.Map<List<SettingDto>>(settings);
            return result;
        }

        public async Task<List<SettingDto>> GetSettingByParentId(int settingId)
        {
            var settings = await DataProvider.Ins.DB.Settings.Where(x => x.ParentId == settingId).ToListAsync();
            var result = _mapper.Map<List<SettingDto>>(settings);
            return result;
        }

        public async Task<bool> DeactiveSetting(int? settingId)
        {
            var settingg = await DataProvider.Ins.DB.Settings.Where(x => x.Id == settingId).FirstOrDefaultAsync();
            if (settingg != null)
            {
                settingg.Status = 0;
                await DataProvider.Ins.DB.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> ActiveSetting(int? settingId)
        {
            var settingg = await DataProvider.Ins.DB.Settings.Where(x => x.Id == settingId).FirstOrDefaultAsync();
            if (settingg != null)
            {
                settingg.Status = 1;
                await DataProvider.Ins.DB.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<SettingDto> GetSettingById(int? id)
        {
            var settings = await DataProvider.Ins.DB.Settings.Where(x => x.Id == id).FirstOrDefaultAsync();
            var result = _mapper.Map<SettingDto>(settings);
            return result;
        }

        #region private method

        private bool IsDuplicateSetting(SettingDto item)
        {
            var val = DataProvider.Ins.DB.Settings.Where(x => x.Id == item.Id
            || x.Name == item.Name
            || x.Value == item.Value).FirstOrDefault();
            if (val == null)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
