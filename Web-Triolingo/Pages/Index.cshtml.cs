using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.Statistics;
using Web_Triolingo.Interface.Users;

namespace Web_Triolingo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserService _userService;
		private readonly IStatisticService _service;

		public IndexModel(ILogger<IndexModel> logger, IUserService userService, IStatisticService service)
        {
            _logger = logger;
            _userService = userService;
            _service = service;
        }

        public void OnGet()
        {
            User user = null;
            if (HttpContext.Session.GetString("user") != null &&
                (user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"))) != null) {
                GetCourseProgress(user.Id);
                GetUserMark(user.Id);
                
				ViewData["recentUnit"] = _service.GetUnits(user.Id, out int unitIndex);
                ViewData["recentUnitIndex"] = unitIndex;
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

        void GetCourseProgress(int userId)
        {
            int progress = _service.GetCurrentProgress(userId, out Course course);
            if (course == null)
            {
                ViewData["currentCourseName"] = "You haven't start any course!";
				return;
            }
            ViewData["currentCourseName"] = course.Name;
            ViewData["courseProgress"] = progress;
        }

        void GetUserMark(int userId)
        {
            var data = _service.GetMarks(userId);
            if (data != null && data.Count > 0)
            {
                ViewData["chartLabel"] = JsonConvert.SerializeObject(data.Keys).Replace('\"', '\'');
                ViewData["chartData"] = JsonConvert.SerializeObject(data.Values);
            }
		}
    }
}