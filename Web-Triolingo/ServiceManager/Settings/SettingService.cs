using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Model;

namespace Web_Triolingo.ServiceManager.Settings
{
    public class SettingService : ISettingService
    {
        private readonly IMapper _mapper;
        public SettingService(IMapper mapper)
        {
            _mapper = mapper;
        }


        public List<SettingDto> GetAllSetting()
        {
            var settings = DataProvider.Ins.DB.Settings.ToList();
            var result = _mapper.Map<List<SettingDto>>(settings);
            return result;
        }

        public List<SettingDto> GetSettingByParentId(int? settingId)
        {
            var settings = DataProvider.Ins.DB.Settings.Where(x => x.ParentId == settingId).ToList();
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
        public async Task<bool> AddNewSetting(SettingDto setting)
        {
            Setting set = new Setting()
            {
                Name = setting.Name,
                Status = 1,
                Note = setting.Note,
                Value = setting.Value,
                ParentId = setting.ParentId,
            };
            await DataProvider.Ins.DB.Settings.AddAsync(set);
            await DataProvider.Ins.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditSetting(SettingDto setting)
        {
            var oldEntity = await DataProvider.Ins.DB.Settings.Where(x => x.Id == setting.Id).FirstOrDefaultAsync();
            if (oldEntity != null)
            {
                oldEntity.Name = setting.Name;
                oldEntity.Note = setting.Note;
                oldEntity.Value = setting.Value;
                oldEntity.ParentId = setting.ParentId;
                DataProvider.Ins.DB.Settings.Update(oldEntity);
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

        public List<SettingDto> GetSettingsNoParentId()
        {
            var settings = DataProvider.Ins.DB.Settings.Where(x => x.ParentId == null).ToList();
            var result = _mapper.Map<List<SettingDto>>(settings);
            return result;
        }

        public bool IsDuplicateSetting(SettingDto item)
        {
            var val = DataProvider.Ins.DB.Settings.Where(x => x.Name == item.Name
            || x.Value == item.Value).FirstOrDefault();
            if (val == null)
            {
                return false;
            }
            return true;
        }

    }
}
