using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Units;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;
using Newtonsoft.Json;

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
        public int CourseId { get; set; }
        public void OnGet(int? id, string? addFailed, string? updateFailed)
        {
            try
            {
                if (TempData["UnitAdd"] != null) UnitAdd = JsonConvert.DeserializeObject<Unit>(TempData["UnitAdd"].ToString());
                ViewData["ErrorUpdate"] = updateFailed;
                ViewData["ErrorAdd"] = addFailed;
                AllUnitsById = _unitService.GetUnitsByCourseId(id);
                CourseName = _courseService.GetCourseById(id).Result.Name;
                CourseId = Convert.ToInt32(id);
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
                var course = _unitService.GetCourseByUnitId(unit.CourseId);
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
                return RedirectToPage("ListAll", new { id = unit.CourseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            try
            {
                if(!_unitService.IsDuplicateUnitEdit(UnitAdd))
                {
                    ViewData["UpdateFail"] = "Học phần của bạn bị trùng lặp! Tên hoặc số thứ tự đã tồn tại!";
                    TempData["UnitAdd"] = JsonConvert.SerializeObject(UnitAdd);
                    return RedirectToPage("ListAll", new { id = UnitAdd.CourseId, updateFailed = ViewData["UpdateFail"] });
                }
                if (await _unitService.UpdateUnit(UnitAdd)) return RedirectToPage("ListAll", new { id = UnitAdd.CourseId });
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> OnPostAddAsync(int id)
        {
            try
            {
                var course = _courseService.GetCourseById(id).Result;
                AllUnitsById = _unitService.GetUnitsByCourseId(UnitAdd.CourseId);
                CourseName = course.Name;
                UnitAdd.CourseId = id;
                if (!_unitService.IsDuplicateUnitAdd(UnitAdd))
                {
                    ViewData["AddFail"] = "Học phần của bạn bị trùng lặp! Tên hoặc số thứ tự đã tồn tại!";
                    TempData["UnitAdd"] = JsonConvert.SerializeObject(UnitAdd);
                    return RedirectToPage("ListAll", new { id = UnitAdd.CourseId, addFailed = ViewData["AddFail"] });
                }
                if (await _unitService.AddUnit(UnitAdd))
                {
                    return RedirectToPage("ListAll", new { id = UnitAdd.CourseId });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public void OnPostEditUnit(int? id)
        {
            try
            {
                UnitAdd = _unitService.GetById(id);
                AllUnitsById = _unitService.GetUnitsByCourseId(UnitAdd.CourseId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}
