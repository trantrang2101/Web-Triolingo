using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Model;
using Web_Triolingo.ServiceManager.Settings;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Newtonsoft.Json;

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
        public List<SettingDto> AllSettingsByParent { get; set; }
        [BindProperty]
        public SettingDto SettingAdd { get; set; }
        public void OnGet()
        {
            try
            {
                ListAllSettings = _settingService.GetSettingsNoParentId().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public JsonResult OnPostGetChild(int? id)
        {
            try
            {
                AllSettingsByParent = _settingService.GetSettingByParentId(id).Result;
                ListAllSettings = _settingService.GetSettingsNoParentId().Result;
                //return RedirectToAction("SettingList");
                return new JsonResult(AllSettingsByParent, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public async Task<IActionResult> OnPostAdd()
        {
            try
            {
                if (await _settingService.AddNewSetting(SettingAdd))
                {
                    ListAllSettings = _settingService.GetSettingsNoParentId().Result;
                    return RedirectToAction("SettingList");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<IActionResult> OnPostEdit()
        {
            try
            {
                var oldEntity = await _settingService.GetSettingById(SettingAdd.Id);
                if (oldEntity != null)
                    ViewData["OldEntity"] = oldEntity;
                
                return RedirectToAction("SettingList");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                var setting = await _settingService.GetSettingById(id);
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
