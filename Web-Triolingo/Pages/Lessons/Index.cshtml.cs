using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IndexModel(ILogger<IndexModel> logger, ILessonService lessonService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _lessonService = lessonService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public List<LessonDto> ListAllLesson { get; set; }
        public void OnGet()
        {
            //Get session
            var objString = HttpContext.Session.GetString("user");
            if (objString != null)
            {
                var obj = JsonConvert.DeserializeObject<UserDto>(objString);
                ViewData["Name"] = obj.FullName;
            }

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

        public ActionResult OnPostLogin(UserLoginDto userLogin)
        {
            try
            {
                var user = _userService.Login(userLogin).Result;
                if (user != null)
                {
                    //Set session
                    string jsonStr = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("user", jsonStr);
                    return RedirectToAction("Index");
                }
                return Content("Email or Password is incorrect");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public ActionResult OnPostRegis(UserLoginDto userLogin)
        {
            try
            {
                var user = _userService.Login(userLogin).Result;
                if (user != null)
                {
                    //Set session
                    string jsonStr = JsonConvert.SerializeObject(user);
                    HttpContext.Session.SetString("user", jsonStr);
                    return RedirectToAction("Index");
                }
                return Content("Email or Password is incorrect");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
