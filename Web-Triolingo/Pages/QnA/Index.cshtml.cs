using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.QnA;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.Units;
using Web_Triolingo.Model;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Pages.QnA
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IQuestion service;
        private readonly ILessonService lessonService;
        private readonly ISettingService settingService;
        private readonly IUnitService unitService;
        private readonly ICourseService courseService;
        public List<Question> List { get; set; }
        [BindProperty]
        public Question Question { get; set; }
        public List<Course> Courses { get; set; }
        public List<Unit> Units { get; set; } 
        public List<Lesson> Lessons { get; set; }
        public List<SettingDto> Settings { get; set; }
        public Course? Course;
        public Unit? Unit;
        public Lesson? Lesson;
        public IndexModel(ILogger<IndexModel> _logger, IQuestion _service, ICourseService courseService, IUnitService unitService, ILessonService lessonService)
        {
            logger = _logger;
            service = _service;
            this.courseService = courseService;
            this.lessonService = lessonService;
            this.unitService = unitService;
        }
        public void OnGet(int?id)
        {
            try
            {
                OnPostLesson(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostLesson(int? id)
        {
            try
            {
                if(id== null||id<=0) {
                    Lessons = lessonService.GetAllLesson().Result;
                    Lesson = Lessons.FirstOrDefault();
                }
                else
                {
                    Lesson = lessonService.GetLessonById(id).Result;
                }
                Unit = unitService.GetById(Lesson?.UnitId).Result;
                Course = courseService.GetCourseById(Unit.CourseId).Result;
                Units = Course.Units.ToList();
                Lessons = Unit.Lessons.ToList();
                List = Lesson.Questions.ToList();
            }catch(Exception e)
            {
                logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
