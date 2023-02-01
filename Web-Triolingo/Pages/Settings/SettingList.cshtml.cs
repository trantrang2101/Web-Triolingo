using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web_Triolingo.DBContext;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Models;
using Web_Triolingo.ServiceManager.Settings;

namespace Web_Triolingo.Pages.Settings
{
    public class SettingListModel : PageModel
    {
        private readonly ILogger<SettingListModel> _logger;
        public SettingListModel(ILogger<SettingListModel> logger)
        {
            _logger = logger;
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
                ListAllSettings = SettingService.GetAllSetting();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
