using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Models;
using Web_Triolingo.ServiceManager.Settings;

namespace Web_Triolingo.Pages.Settings
{
    public class SettingListModel : PageModel
    {
        private readonly ILogger<SettingListModel> _logger;
        private readonly ISettingService _settingService;
        public SettingListModel(ILogger<SettingListModel> logger, ISettingService settingService)
        {
            _logger = logger;
            _settingService = settingService;
        }
        public List<SettingDto> ListAllSettings { get; set; }
        public void OnGet()
        {
            //using (var context = new TriolingoDBContext())
            //{
            //    ListAllSettings = context.Settings.ToList();
            //}
            try
            {
                ListAllSettings = _settingService.GetAllSetting().Result;
                //using (var context = new TriolingoDBContext())
                //{
                //    var result = context.Settings.ToList();
                //    result.ForEach(setting =>
                //    {
                //        ListAllSettings.Add(new SettingDto()
                //        {

                //        });
                //    });
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
