using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.Settings
{
    public class SettingService : ISettingService
    {
        public async Task<List<SettingDto>> GetAllSetting()
        {

            var settings = await DataProvider.Ins.DB.Settings.ToListAsync();
            List<SettingDto> result = new List<SettingDto>();
            settings.ForEach(se =>
            {
                result.Add(new SettingDto()
                {
                    Id = se.Id,
                    Name = se.Name,
                    Value = se.Value,
                    Note = se.Note,
                    Status = se.Status,
                    ParentName = GetParentNameByParentId(se.ParentId)
                });
            });
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
            var result = new SettingDto()
            {
                Id = Convert.ToInt32(id),
                Value = settings.Value,
                Name = settings.Name,
                ParentName = GetParentNameByParentId(settings.ParentId),
                Note = settings.Note,
                Status = settings.Status,
            };
            return result;
        }

        #region private method
        private string GetParentNameByParentId(int? parentId)
        {
            return DataProvider.Ins.DB.Settings.Where(x => x.Id == parentId).Select(x => x.Name).FirstOrDefault();
        }
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
