using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Courses;
using Triolingo.Core.Entity;

namespace Web_Triolingo.Pages.Courses
{
    public class CourseListModel : PageModel
    {
        private readonly ILogger<CourseListModel> logger;
        private readonly ICourseService service;
        public List<Course> List { get; set; }
        [BindProperty]
        public Course course { get; set; }
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
    }
}
