﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Courses;
using Triolingo.Core.Entity;
using Newtonsoft.Json;
using Web_Triolingo.Interface.Users;
using ClosedXML.Excel;

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
            service = _service;
            _userService = userService;
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
        public IActionResult OnPostExport()
        {
            try
            {
                var listCourse = service.GetAllCourse();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Courses"); // Tạo sheet "Courses"

                    // Đưa dữ liệu vào sheet "Courses"
                    worksheet.Cell(1, 1).Value = "Name";
                    worksheet.Cell(1, 2).Value = "Description";
                    worksheet.Cell(1, 3).Value = "Note";
                    worksheet.Cell(1, 4).Value = "RateAverage";
                    worksheet.Cell(1, 5).Value = "Status";
                    worksheet.Column(2).Width = 150;
                    //var descriptionCells = worksheet.Column(2).Cells();
                    //var style = descriptionCells.Style;
                    //style.Alignment.WrapText = true;
                    //descriptionCells.Style = style;
                    var row = 2;
                    foreach (var course in listCourse.Result)
                    {
                        string sta = "";
                        if (course.Status == 1) sta = "Hoạt động";
                        else sta = "Đã khóa";
                        worksheet.Cell(row, 1).Value = course.Name;
                        worksheet.Cell(row, 2).Value = course.Description;
                        worksheet.Cell(row, 3).Value = course.Note;
                        worksheet.Cell(row, 4).Value = course.RateAverage;
                        worksheet.Cell(row, 5).Value = sta;
                        row++;
                    }
                    var stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Courses.xlsx");
                }
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
                    if (id != 0)
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
        public ActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
