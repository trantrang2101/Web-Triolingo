using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.QnA;
using Web_Triolingo.Model;

namespace Web_Triolingo.Pages.QnA
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IQuestion service;
        private readonly ICourseService courseService;
        public List<Question> List { get; set; }
        [BindProperty]
        public Question Question { get; set; }
        public List<Course> Courses { get; set; }
        public List<Unit> Units { get; set; } 
        public List<Lesson> Lessons { get; set; }
        public Course? Course;
        public Unit? Unit;
        public Lesson? Lesson;
        public IndexModel(ILogger<IndexModel> _logger, IQuestion _service)
        {
            logger = _logger;
            service = _service;
        }
        public void OnGet(int?id)
        {
            try
            {
                Courses = courseService.GetAllCourse().Result;
                if (id!=null && id > 0)
                {
                    List = service.GetAllQuestion(id).Result;
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
