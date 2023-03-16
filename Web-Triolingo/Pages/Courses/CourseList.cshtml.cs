using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Courses;
using Triolingo.Core.Entity;
using Newtonsoft.Json;
using Web_Triolingo.Interface.Users;

namespace Web_Triolingo.Pages.Courses
{
    public class CourseListModel : PageModel
    {
        private readonly ILogger<CourseListModel> logger;
        private readonly ICourseService service;
        private readonly IUserService _userService;
        public List<Course> List { get; set; }
        [BindProperty]
        public Course course { get; set; }
        public CourseListModel(ILogger<CourseListModel> _logger, ICourseService _service, IUserService userService)
        {
            logger = _logger;
            service= _service;
            _userService= userService;
        }
        public void OnGet()
        {
            try
            {
                List = service.GetAllCourse().Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostEdit(int? id)
        {
            try
            {
                course = service.GetCourseById(id).Result;
                List = service.GetAllCourse().Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void onPostAdd()
        {
            try
            {
                course = new Course();
                course.Id = 0;
                List = service.GetAllCourse().Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostSave()
        {
            try
            {
                if (course == null || course.Id == null || course.Id == 0)
                {
                    int id = service.AddNewCourse(course).Result;
                    if (id!=0)
                    {
                        OnPostEdit(id);
                        return;
                    }
                }
                else
                {
                    if (service.EditCourse(course).Result == false)
                    {
                    }
                    OnPostEdit(course.Id);
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
            OnGet();
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
                logger.LogError(ex.ToString());
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
                logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
