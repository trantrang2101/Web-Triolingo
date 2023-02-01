using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.Settings
{
    public class SettingService
    {
        public static List<SettingDto> GetAllSetting()
        {

            var settings = DataProvider.Ins.DB.Settings.ToList();
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
        public static string GetParentNameByParentId(int? parentId)
        {
            return DataProvider.Ins.DB.Settings.Where(x => x.Id == parentId).Select(x => x.Name).FirstOrDefault();
        }
    }
}
