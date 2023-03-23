using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web_Triolingo.Interface.Settings;
using Triolingo.Core.Entity;
using Newtonsoft.Json;
using log4net.Repository.Hierarchy;
using Web_Triolingo.Interface.Users;
using Microsoft.AspNetCore.SignalR;
using Web_Triolingo.Hubs;

namespace Web_Triolingo.Pages.Settings
{
    public class SettingListModel : PageModel
    {
        private readonly ILogger<SettingListModel> _logger;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRServer> _signalRHub;

        public SettingListModel(ILogger<SettingListModel> logger,
            ISettingService settingService,
            IUserService userService,
            IHubContext<SignalRServer> signalRHub)
        {
            _logger = logger;
            _settingService = settingService;
            _userService = userService;
            _signalRHub = signalRHub;
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
                    await _signalRHub.Clients.All.SendAsync("LoadSetting");
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
                {
                    await _signalRHub.Clients.All.SendAsync("LoadSetting");
                    return RedirectToAction("SettingList");
                }
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
                        await _signalRHub.Clients.All.SendAsync("LoadSetting");
                        return RedirectToPage("./SettingList");
                    }
                }
                else
                {
                    check = _settingService.ActiveSetting(id).Result;
                    if (check)
                    {
                        await _signalRHub.Clients.All.SendAsync("LoadSetting");
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

        public ActionResult OnPostLogin(User userLogin)
        {
            try
            {
                HttpContext.Session.Clear();
                var user = _userService.Login(userLogin).Result;
                if (user != null)
                {
                    //Set session
                    string jsonStr = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("user", jsonStr);
                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.Session.SetString("loginError", "Email or Password is incorrect");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public ActionResult OnPostRegis(User userRegis)
        {
            try
            {
                HttpContext.Session.Clear();
                var user = _userService.Regis(userRegis).Result;
                if (user)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.Session.SetString("regisError", "This email is already in use");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public ActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
