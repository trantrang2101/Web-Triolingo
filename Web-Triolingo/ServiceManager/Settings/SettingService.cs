using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.Settings
{
    public class SettingService
    {
        private readonly ILogger<SettingService> _logger;
        public SettingService(ILogger<SettingService> logger)
        {
            _logger = logger;
        }
        public static List<SettingDto> GetAllSetting()
        {
            try
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
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public static string GetParentNameByParentId(int? parentId)
        {
            return DataProvider.Ins.DB.Settings.Where(x => x.Id == parentId).Select(x => x.Name).FirstOrDefault();
        }
    }
}
