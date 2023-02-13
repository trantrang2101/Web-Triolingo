using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Web_Triolingo.Common;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Model;
using Web_Triolingo.Pages.Settings;
using Web_Triolingo.ServiceManager.Courses;
using Web_Triolingo.ServiceManager.Settings;

namespace Web_Triolingo.Pages.Courses
{
    public class CourseListModel : PageModel
    {
        private readonly ILogger<CourseListModel> logger;
        private readonly ICourseService service;
        public List<Course> List { get; set; }
        [BindProperty]
        public Course GetCourse { get; set; }
        public CourseListModel(ILogger<CourseListModel> _logger, ICourseService _service)
        {
            logger = _logger;
            service= _service;
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
                GetCourse = service.GetCourseById(id).Result;
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
                GetCourse = new Course();
                GetCourse.Id = 0;
                List = service.GetAllCourse().Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public IActionResult OnPostSave()
        {
            try
            {
                if (GetCourse == null || GetCourse.Id == null || GetCourse.Id == 0)
                {
                    if (service.AddNewCourse(GetCourse).Result == false)
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    if (service.EditCourse(GetCourse).Result == false)
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction("CourseList");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
