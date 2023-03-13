using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web_Triolingo.Interface.Settings;
using Triolingo.Core.Entity;
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
        public List<Setting> ListAllSettings { get; set; }
        public List<Setting> ParentSetting { get; set; }
        [BindProperty]
        public Setting SettingAdd { get; set; }
        public void OnGet(string? addFail, string? updateFail)
        {
            try
            {
                if (TempData["AddSetting"] != null)
                    SettingAdd = JsonConvert.DeserializeObject<Setting>(TempData["AddSetting"].ToString());
                ViewData["AddFailed"] = addFail;
                ViewData["UpdateFaild"] = updateFail;
                ListAllSettings = _settingService.OrderSettingsParent();
                ParentSetting = _settingService.GetSettingsNoParentId();
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
                if (!_settingService.IsValidSettingAdd(SettingAdd))
                {
                    ViewData["ErrorAdd"] = "Cài đặt của bạn bị trùng lặp! Tên hoặc giá trị đã tồn tại!";
                    TempData["AddSetting"] = JsonConvert.SerializeObject(SettingAdd);
                    return RedirectToAction("SettingList", new { addFail = ViewData["ErrorAdd"] });

                }
                if (await _settingService.AddNewSetting(SettingAdd))
                {
                    ListAllSettings = _settingService.GetSettingsNoParentId();
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
        public void OnPostUpdateSetting(int? id)
        {
            try
            {
                ListAllSettings = _settingService.OrderSettingsParent();
                ParentSetting = _settingService.GetSettingsNoParentId();
                SettingAdd = _settingService.GetSettingById(id).Result;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
        public async Task<IActionResult> OnPostEdit()
        {
            try
            {
                if (!_settingService.IsValidSettingUpdate(SettingAdd))
                {
                    ViewData["ErrorAdd"] = "Cài đặt của bạn bị trùng lặp! Tên hoặc giá trị đã tồn tại!";
                    TempData["AddSetting"] = JsonConvert.SerializeObject(SettingAdd);
                    return RedirectToAction("SettingList", new { updateFail = ViewData["ErrorAdd"] });
                }
                if (_settingService.EditSetting(SettingAdd).Result)
                    return RedirectToAction("SettingList");
                return NotFound();
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
