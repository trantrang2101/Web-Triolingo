using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.User;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Pages.Settings;

namespace Web_Triolingo.Pages.Lessons
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILessonService _lessonService;
        private readonly IUserService _userService;
        public IndexModel(ILogger<IndexModel> logger, ILessonService lessonService, IUserService userService)
        {
            _logger = logger;
            _lessonService = lessonService;
            _userService = userService;
        }
        public List<LessonDto> ListAllLesson { get; set; }
        public void OnGet()
        {
            try
            {
                ListAllLesson = _lessonService.GetAllLesson().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public IActionResult OnPost(UserLoginDto userLogin)
        {
            try
            {
                var user = _userService.Login(userLogin).Result;
                if (user != null)
                {
                    return RedirectToPage("./SettingList");
                }
                return RedirectToPage("../Settings/SettingList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}