using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.Users;
using Triolingo.Core.Entity;

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
        public List<Lesson> ListAllLesson { get; set; }
        public Lesson Lesson { get; set; }
        public void OnGet(string loginError, string regisError)
        {
            //Get session
            var objString = HttpContext.Session.GetString("user");
            if (objString != null)
            {
                var obj = JsonConvert.DeserializeObject<User>(objString);
                ViewData["Name"] = obj.FullName;
            }
            ViewData["LoginError"] = loginError;
            ViewData["RegisError"] = regisError;
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
        public void OnPostLessonById(int id)
        {
            try
            {
                Lesson = _lessonService.GetLessonById(id).Result;
                ListAllLesson = _lessonService.GetAllLesson().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostAdd(Lesson lesson)
        {
            try
            {
                var check = _lessonService.AddLesson(lesson).Result;
                if(check == true)
                {

                }
                ListAllLesson = _lessonService.GetAllLesson().Result;
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
                    ViewData["Error"] = "Email or Password is incorrect";
                    return RedirectToAction("Index", new { loginError = ViewData["Error"] });
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
                var user = _userService.Regis(userRegis).Result;
                if (user)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = "This email is already in use";
                    return RedirectToAction("Index", new { regisError = ViewData["Error"] });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
