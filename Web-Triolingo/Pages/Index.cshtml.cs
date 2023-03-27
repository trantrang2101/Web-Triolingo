using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.Statistics;
using Web_Triolingo.Interface.UserCourse;
using Web_Triolingo.Interface.UserRoles;
using Web_Triolingo.Interface.Users;

namespace Web_Triolingo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserService _userService;
		private readonly ICourseService _courseService;
		private readonly IStatisticService _service;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserCourse _mentorCourseService;
        [BindProperty]
        public List<Course> Courses { get; set; }
        [BindProperty]
        public Course GetCourse { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }
		public IndexModel(ILogger<IndexModel> logger, 
            IUserService userService, 
            IStatisticService service, 
            ICourseService courseService,
            IUserRoleService userRoleService,
            IUserCourse mentorCourseService)
        {
            _logger = logger;
            _userService = userService;
            _service = service;
            _courseService = courseService;
            _userRoleService = userRoleService;
            _mentorCourseService = mentorCourseService;
        }

        public void OnGet(int? id)
        {
            User user = null;
            if (HttpContext.Session.GetString("user") != null &&
                (user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"))) != null) {
                var userRole = _userRoleService.GetRoleOfUser(user.Id);
                IsAdmin = userRole.RoleType == 2?true:false;
                Courses = _courseService.GetAllCourse().Result;
                if (userRole.RoleType == 3)
                {
                    Courses = _mentorCourseService.getCourseByMentor(user.Id);
                }
                if(id.HasValue)
                {
                    GetCourse=_courseService.GetCourseById(id.Value).Result;
                }
                else
                {
					GetCourse = Courses.FirstOrDefault();
				}
				ViewData["recentUnit"] = _service.GetLesson(user.Id, GetCourse);
				GetUserMark(user.Id,GetCourse);
				GetCourseProgress(user.Id,GetCourse);
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

        void GetCourseProgress(int userId,Course course)
        {
            int progress = _service.GetCurrentProgress(userId, course);
            if (course == null)
            {
                ViewData["currentCourseName"] = "You haven't start any course!";
				return;
            }
            ViewData["currentCourseName"] = course.Name;
            ViewData["courseProgress"] = progress;
        }

        void GetUserMark(int userId, Course course)
        {
            var data = _service.GetMarks(userId, course);
            if (data != null && data.Count > 0)
            {
                ViewData["chartLabel"] = JsonConvert.SerializeObject(data.Keys).Replace('\"', '\'');
                ViewData["chartData"] = JsonConvert.SerializeObject(data.Values);
            }
		}
    }
}