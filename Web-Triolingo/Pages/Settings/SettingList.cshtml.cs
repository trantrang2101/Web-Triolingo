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
        public async Task OnGetAsync()
        {
            //using (var context = new TriolingoDBContext())
            //{
            //    ListAllSettings = context.Settings.ToList();
            //}
            try
            {
                ListAllSettings = await _settingService.GetSettingsNoParentId();
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
        public IActionResult OnPost(int? id)
        {
            try
            {
                var setting = _settingService.GetSettingById(id).Result;
                bool check = true;
                if (setting.Status == 1)
                {
                    check = _settingService.DeactiveSetting(id).Result;
                    if (check)
                    {
                        return RedirectToPage("./SettingList");
                    }
                }
                else
                {
                    check = _settingService.ActiveSetting(id).Result;
                    if (check)
                    {
                        return RedirectToPage("./SettingList");
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }
    }
}
