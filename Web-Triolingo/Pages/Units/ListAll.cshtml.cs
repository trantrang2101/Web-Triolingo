using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Units;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;
using Newtonsoft.Json;
using Web_Triolingo.Interface.Users;
using ClosedXML.Excel;

namespace Web_Triolingo.Pages.Units
{
    public class ListAllModel : PageModel
    {
        private readonly IUnitService _unitService;
        private readonly ILogger<ListAllModel> _logger;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;

        public ListAllModel(IUnitService unitService,
            ILogger<ListAllModel> logger,
            ICourseService courseService,
            IUserService userService)
        {
            _unitService = unitService;
            _logger = logger;
            _courseService = courseService;
            _userService = userService;
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
                if (UnitAdd != null && UnitAdd.CourseId != null) id = UnitAdd.CourseId;
                if (TempData["UnitAdd"] != null) UnitAdd = JsonConvert.DeserializeObject<Unit>(TempData["UnitAdd"].ToString());
                ViewData["ErrorUpdate"] = updateFailed;
                ViewData["ErrorAdd"] = addFailed;
                AllUnitsById = _unitService.GetUnitsByCourseId(id);
                if (id != null)
                {
                    CourseName = _courseService.GetCourseById(id).Result.Name;
                    CourseId = Convert.ToInt32(id);
                }
                TempData["CourseId"] = id;
                TempData["CourseName"] = CourseName;
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
        public IActionResult OnPostExport()
        {
            try
            {
                int courId = Convert.ToInt32(TempData["CourseId"]);
                var listUnits = _unitService.GetUnitsByCourseId(courId);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Units"); // Tạo sheet "Courses"

                    // Đưa dữ liệu vào sheet "Courses"
                    worksheet.Cell(1, 1).Value = "Name";
                    worksheet.Cell(1, 2).Value = "Description";
                    worksheet.Cell(1, 3).Value = "Note";
                    worksheet.Cell(1, 4).Value = "Cousrse";
                    worksheet.Cell(1, 5).Value = "Status";
                    worksheet.Cell(1, 6).Value = "Order";
                    worksheet.Column(1).Width = 50;
                    worksheet.Column(2).Width = 100;
                    //var descriptionCells = worksheet.Column(2).Cells();
                    //var style = descriptionCells.Style;
                    //style.Alignment.WrapText = true;
                    //descriptionCells.Style = style;
                    var row = 2;
                    foreach (var unit in listUnits)
                    {
                        string sta = "";
                        if (unit.Status == 1) sta = "Hoạt động";
                        else sta = "Đã khóa";
                        worksheet.Cell(row, 1).Value = unit.Name;
                        worksheet.Cell(row, 2).Value = unit.Description;
                        worksheet.Cell(row, 3).Value = unit.Note;
                        worksheet.Cell(row, 4).Value = TempData["CourseName"].ToString();
                        worksheet.Cell(row, 5).Value = sta;
                        worksheet.Cell(row, 6).Value = unit.Order;
                        row++;
                    }
                    var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Units.xlsx");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
        public async Task<IActionResult> OnPostEditAsync()
        {
            try
            {
                if (!_unitService.IsDuplicateUnitEdit(UnitAdd))
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
    }
}
