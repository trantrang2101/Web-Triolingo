using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Pages.Settings;

namespace Web_Triolingo.Pages.Lessons
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILessonService _lessonService;
        public IndexModel(ILogger<IndexModel> logger, ILessonService lessonService)
        {
            _logger = logger;
            _lessonService = lessonService;
        }
        public List<LessonDto> ListAllLesson { get; set; }
        public void OnGet()
        {
            try
            {
                ListAllLesson = _lessonService.GetAllLesson().Result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
