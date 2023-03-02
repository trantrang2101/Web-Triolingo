using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Units;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;

namespace Web_Triolingo.Pages.Units
{
    public class ListAllModel : PageModel
    {
        private readonly IUnitService _unitService;
        private readonly ILogger<ListAllModel> _logger;
        private readonly ICourseService _courseService;
        public ListAllModel(IUnitService unitService,
            ILogger<ListAllModel> logger,
            ICourseService courseService)
        {
            _unitService = unitService;
            _logger = logger;
            _courseService = courseService;
        }
        [BindProperty]
        public List<Unit> AllUnitsById { get; set; }
        [BindProperty]
        public Unit UnitAdd { get; set; }
        public string CourseName { get; set; }
        public int Id { get; set; }
        public void OnGet(int? id)
        {
            try
            {
                //id = id == null ? Id : id;
                AllUnitsById = _unitService.GetUnitsByCourseId(id);
                CourseName = _courseService.GetCourseById(id).Result.Name;
                UnitAdd = new Unit();
                UnitAdd.CourseId = Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<IActionResult> OnPostStatusAsync(int id)
        {
            try
            {
                var unit = _unitService.GetById(id);
                var course = _unitService.GetCourseByUnitId(id);
                Id = course.Id;
                AllUnitsById = _unitService.GetUnitsByCourseId(course.Id);
                CourseName = course.Name;
                if (unit.Status == 1)
                {
                    await _unitService.DeactiveUnit(id);
                }
                else
                {
                    await _unitService.ActiceUnit(id);
                }
                return RedirectToPage("ListAll", new { id = Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<IActionResult> OnPostAddAsync()
        {
            try
            {
                var course = _courseService.GetCourseById(UnitAdd.CourseId).Result;
                Id = UnitAdd.CourseId;
                AllUnitsById = _unitService.GetUnitsByCourseId(UnitAdd.CourseId);
                CourseName = course.Name;
                if (await _unitService.AddUnit(UnitAdd))
                {
                    return RedirectToPage("ListAll", new { id = Id });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
