using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Exercises;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.QnA;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.Units;
using Web_Triolingo.Interface.Users;

namespace Web_Triolingo.Pages.QnA
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IExercise exservice;
        private readonly IQuestion service;
        private readonly IAnswer answerService;
        private readonly ILessonService lessonService;
        private readonly IUserService _userService;
        private readonly ISettingService settingService;
        private readonly IUnitService unitService;
        private readonly ICourseService courseService;
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<Course> Courses { get; set; }
        public List<Unit> Units { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<SelectListItem> Settings { get; set; }
        public Course? Course { get; set; }
        public Unit? Unit { get; set; }
        public Lesson Lesson { get; set; }
        public List<Exercise> List { get; set; }
        public bool? isEdit { get; set; } = null;
        [BindProperty]
        public Exercise Exercise{ get; set; }
        [BindProperty]
        public Question Question { get; set; }
        [BindProperty]
        public Answer? Answer { get; set; }
        [BindProperty]
        [DataType(DataType.Upload)]
        public IFormFile FileUpload { get; set; }
        public IndexModel(ILogger<IndexModel> _logger,ISettingService setting, IUserService userService, IExercise exercise, IQuestion _service, ICourseService courseService, IUnitService unitService, ILessonService lessonService,IAnswer answer)
        {
            logger = _logger;
            settingService = setting;
            service = _service;
            answerService = answer;
            this.courseService = courseService;
            this.lessonService = lessonService;
            this.unitService = unitService;
            _userService = userService;
            exservice= exercise;
        }
        public void OnGet(int id = 0)
        {
            try
            {
                List<Setting> settings = settingService.GetSettingsNoParentId();
                if (settings != null && settings.Count > 0)
                {
                    Setting set = settings.Where(x => x.Value.ToLower() == "lesson").FirstOrDefault();
                    if (set != null)
                    {
                        List<Setting> list = settingService.GetSettingByParentId(set.Id).ToList();
                        Settings = list.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.Name
                                  }).ToList();
                    }
                }
                if (id != null && id > 0)
                {
                    Lesson = lessonService.GetLessonById(id).Result;
                }
                if (Lesson==null||id == null || id <= 0)
                {
                    List<Lesson> lessons = lessonService.GetAllLesson().Result;
                    Lesson = lessons.FirstOrDefault();
                }
                Unit = unitService.GetById(Lesson.UnitId);
                Course = courseService.GetCourseById(Unit.CourseId).Result;
                Courses = courseService.GetAllCourse().Result;
                Units = unitService.GetUnitsByCourseId(Course.Id);
                Lessons = lessonService.getAllLessonsByUnitId(Unit.Id).Result;
                List = new List<Exercise>();
                exservice.getAllExercisesByLessonId(Lesson.Id).Result.ToList().ForEach(x =>
                {
                    x.Setting = settingService.GetSettingById(x.TypeId).Result;
                    x.Questions = service.GetAllQuestions(x.Id).Result.ToList();
                    List.Add(x);
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostCourse(int id)
        {
            try
            {
                if(Course== null||id==null||id<=0)
                {
                    var list = courseService.GetAllCourse().Result;
                    id = list.FirstOrDefault().Id;
                }
                Unit = unitService.GetUnitsByCourseId(id).FirstOrDefault();
                Lesson = lessonService.getAllLessonsByUnitId(Unit.Id).Result.FirstOrDefault();
                OnGet(Lesson.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostUnit(int id)
        {
            try
            {
                if (Course == null || id == null || id <= 0)
                {
                    var list = unitService.GetAll();
                    id = list.FirstOrDefault().Id;
                }
                Lesson = lessonService.getAllLessonsByUnitId(id).Result.FirstOrDefault();
                OnGet(Lesson.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        private bool IsBase64String(string? base64)
        {
            if(base64== null)
            {
                return false;
            }
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
        public void OnPostEdit(int? questionId, int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    Exercise = exservice.GetExerciseById(id).Result;
                    Question = null;
                }
                if (questionId!=null&&questionId != 0)
                {
                    Question = service.GetQuestionById(questionId).Result;
                    Answers = answerService.GetAllAnswers(Question.Id).Result;
                    Exercise = exservice.GetExerciseById(Question.ExerciseId).Result;
                }
                if (Exercise.File!=null&& Exercise.FileName !=null)
                {
                    if (IsBase64String(Exercise.File))
                    {
                        using (var ms = new MemoryStream())
                        {
                            byte[] bytes = Convert.FromBase64String(Exercise.File);
                            FileUpload = new FormFile(ms, 0, bytes.Length, Exercise.FileName, Exercise.FileName);
                            Exercise.FileExtention = System.IO.Path.GetExtension(Exercise.FileName).Substring(1);
                        }
                    }
                    else
                    {
                        Exercise.FileUrl = Exercise.File;
                    }
                }
                OnGet(Exercise.LessonId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostDelete(int questionId = 0, int id = 0)
        {
            try
            {
                if (questionId != 0)
                {
                    Question = service.GetQuestionById(questionId).Result;
                    Answers = answerService.GetAllAnswers(Question.Id).Result;
                    Exercise = exservice.GetExerciseById(Question.ExerciseId).Result;
                    if (service.DeleteQuestion(questionId).Result == false)
                    {

                    }
                }
                if (id != 0)
                {
                    Exercise = exservice.GetExerciseById(id).Result;
                    Question = null;
                    if (exservice.DeleteExercise(id).Result == false)
                    {

                    }
                }
                OnGet(Exercise.LessonId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostActive(int questionId = 0, int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    Exercise = exservice.GetExerciseById(id).Result;
                    Question = null;
                    if (exservice.ActiveExercise(id).Result == false)
                    {

                    }
                }
                if (questionId != 0)
                {
                    Question = service.GetQuestionById(questionId).Result;
                    Answers = answerService.GetAllAnswers(Question.Id).Result;
                    Exercise = exservice.GetExerciseById(Question.ExerciseId).Result;
                    if (service.ActiveQuestion(questionId).Result == false)
                    {

                    }
                }
                OnGet(Exercise.LessonId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostAdd(int lessonId)
        {
            try
            {
                Exercise = new Exercise();
                Exercise.Id = 0;
                Exercise.LessonId = lessonId;
                Question = null;
                OnGet(lessonId) ;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostAddQuestion(int exerciseId)
        {
            try
            {
                Question = new Question();
                Question.Id = 0;
                Question.ExerciseId = exerciseId;
                Exercise = exservice.GetExerciseById(exerciseId).Result;
                Question.Exercise= Exercise;
                Answers = new List<Answer>();
                OnGet(Exercise.LessonId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostOpen(int? id, int questionId,int exerciseId)
        {
            try
            {
                isEdit = true;
                if (id == null)
                {
                    Answer = new Answer();
                    Answer.Id = 0;
                    Answer.QuestionId = questionId;
                    List<Answer> listAns = answerService.GetAllAnswers(questionId).Result;
                    switch (exservice.GetExerciseById(exerciseId).Result.TypeId){
                        case 6:
                            OnPostEdit(questionId);
                            isEdit = false;
                            break;
                        case 7:
                            if(listAns.Count() > 3)
                            {
                                isEdit = false;
                            }
                            break;
                        case 8:
                            if (listAns.Count() > 3)
                            {
                                isEdit = false;
                            }
                            break;
                        case 9:
                            if (listAns.Count() > 0)
                            {
                                isEdit = false;
                            }
                            break;
                    }
                }
                else
                {
                    Answer = answerService.GetAnswerById(id).Result;
                }
                OnPostEdit(questionId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostSave(int questionId=0)
        {
            try
            {
                if (FileUpload != null && FileUpload.Length > 0)
                {
                    using(var ms = new MemoryStream())
                    {
                        FileUpload.CopyTo(ms);
                        byte[] bytes = ms.ToArray();
                        Exercise.File=  Convert.ToBase64String(bytes);
                        Exercise.FileName = FileUpload.FileName;
                    }
                }else if(Exercise.File!=null&&Exercise.File.Length > 0)
                {
                    Exercise.File = Exercise.FileUrl;
                    Exercise.FileName = System.IO.Path.GetFileName(Exercise.File);
                }
                if (Exercise.Id == null || Exercise.Id == 0)
                {
                    Exercise.Id = exservice.AddExercise(Exercise).Result;
                    if (Exercise.Id == null || Exercise.Id == 0)
                    {
                    }
                }
                else
                {
                    if (exservice.UpdateExercise(Exercise).Result == false)
                    {
                    }
                }
                OnPostEdit(questionId, Exercise.LessonId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostSaveQuestion()
        {
            try
            {
                if (Question.Id == null || Question.Id == 0)
                {
                    Question.Id = service.AddNewQuestion(Question).Result;
                    if (Question.Id == null || Question.Id == 0)
                    {
                    }
                }
                else
                {
                    if (service.EditQuestion(Question).Result == false)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostDeleteAnswer(int id)
        {
            try
            {
                int questionId = answerService.GetAnswerById(id).Result.QuestionId;
                if (answerService.DeleteAnswer(id).Result == false)
                {

                }
                OnPostEdit(questionId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostActiveAnswer(int id)
        {
            try
            {
                int questionId = answerService.GetAnswerById(id).Result.QuestionId;
                if (answerService.ActiveAnswer(id).Result == false)
                {

                }
                OnPostEdit(questionId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
        public void OnPostSaveAnswer()
        {
            try
            {
                if (Answer == null || Answer.Id == null || Answer.Id == 0)
                {
                    if (answerService.AddNewAnswer(Answer).Result == false)
                    {
                    }
                }
                else
                {
                    if (answerService.EditAnswer(Answer).Result == false)
                    {
                    }
                }
                isEdit = false;
                OnPostEdit(Answer.QuestionId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
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
