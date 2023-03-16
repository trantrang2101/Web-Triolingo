using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.Users;
using Triolingo.Core.Entity;
using Web_Triolingo.ServiceManager.Units;
using Web_Triolingo.Interface.Units;

namespace Web_Triolingo.Pages.Lessons
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILessonService _lessonService;
        private readonly IUserService _userService;
        private readonly IUnitService _unitService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IndexModel(ILogger<IndexModel> logger, IUnitService unitService, ILessonService lessonService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _lessonService = lessonService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _unitService= unitService;
        }
        public List<Lesson> ListAllLesson { get; set; }
        public Lesson Lesson { get; set; }
        public Unit Unit { get; set; }
        public List<Unit> ListAllUnit { get; set; }
        public void OnGet()
        {
            ViewData["AddAble"] = true;
            //Get session
            var objString = HttpContext.Session.GetString("user");
            if (objString != null)
            {
                var obj = JsonConvert.DeserializeObject<User>(objString);
                ViewData["Name"] = obj.FullName;
            }
            try
            {
                ListAllLesson = _lessonService.GetAllLesson().Result;
                ListAllUnit = _unitService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPost(Lesson lesson)
        {
            if (Request.Form["asp-page-handler"] == "Add")
            {
                OnPostAdd(lesson);
            }
            else if (Request.Form["asp-page-handler"] == "Delete")
            {
                OnPostDelete(lesson);
            }
            else if (Request.Form["asp-page-handler"] == "Update")
            {
                OnPostUpdate(lesson);
            }
        }
        public void OnPostLessonById(int id)
        {
            try
            {
                ViewData["AddAble"] = false;
                Lesson = _lessonService.GetLessonById(id).Result;
                ListAllLesson = _lessonService.GetAllLesson().Result;
                Unit = _unitService.GetById(Lesson.UnitId);
                ListAllUnit = _unitService.GetAll();
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
                ViewData["AddAble"] = false;
                var check = _lessonService.AddLesson(lesson).Result;
                ListAllUnit = _unitService.GetAll();
                ListAllLesson = _lessonService.GetAllLesson().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostUpdate(Lesson lesson)
        {
            try
            {
                ViewData["AddAble"] = false;
                var check = _lessonService.UpdateLesson(lesson).Result;
                ListAllLesson = _lessonService.GetAllLesson().Result;
                ListAllUnit = _unitService.GetAll();
                Lesson = _lessonService.GetLessonById(lesson.Id).Result;
                Unit = _unitService.GetById(Lesson.UnitId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostDelete(Lesson lesson)
        {
            try
            {
                ViewData["AddAble"] = false;
                var check = _lessonService.DeleteLesson(lesson.Id).Result;
                ListAllLesson = _lessonService.GetAllLesson().Result;
                ListAllUnit = _unitService.GetAll();
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
