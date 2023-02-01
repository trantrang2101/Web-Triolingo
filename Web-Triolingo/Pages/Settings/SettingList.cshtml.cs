using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Web_Triolingo.DBContext;
using Web_Triolingo.Models;

namespace Web_Triolingo.Pages.Settings
{
    public class SettingListModel : PageModel
    {
        private readonly ILogger<SettingListModel> _logger;
        public SettingListModel(ILogger<SettingListModel> logger)
        {
            _logger = logger;
        }
        public List<Setting> ListAllSettings { get; set; }
        public void OnGet()
        {
            //using (var context = new TriolingoDBContext())
            //{
            //    ListAllSettings = context.Settings.ToList();
            //}
            ListAllSettings = DataProvider.Ins.DB.Settings.ToList();
        }
    }
}
