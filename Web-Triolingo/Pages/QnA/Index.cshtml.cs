using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Exercises;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.QnA;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.Units;
namespace Web_Triolingo.Pages.QnA
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IExercise exservice;
        private readonly IQuestion service;
        private readonly IAnswer answerService;
        private readonly ILessonService lessonService;
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
        public bool isEdit { get; set; }
        [BindProperty]
        public Exercise Exercise{ get; set; }
        [BindProperty]
        public Question Question { get; set; }
        [BindProperty]
        public Answer? Answer { get; set; }
        public IndexModel(ILogger<IndexModel> _logger,ISettingService setting, IExercise exercise, IQuestion _service, ICourseService courseService, IUnitService unitService, ILessonService lessonService,IAnswer answer)
        {
            logger = _logger;
            settingService = setting;
            service = _service;
            answerService = answer;
            this.courseService = courseService;
            this.lessonService = lessonService;
            this.unitService = unitService;
            exservice= exercise;
        }
        public void OnGet(int id)
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
        public void OnPostLesson(int id)
        {
            //try
            //{
            //    if(id== null||id<=0) {
            //        var list = lessonService.GetAllLesson().Result;
            //        Lesson = list.FirstOrDefault();
            //    }
            //    else
            //    {
            //        Lesson = lessonService.GetLessonById(id).Result;
            //    }
            //    Unit = unitService.GetById(Lesson?.UnitId);
            //    Courses = courseService.GetAllCourse().Result;
            //    Units = unitService.GetUnitsByCourseId(Course.Id);
            //    Lessons = lessonService.getAllLessonsByUnitId(Unit.Id).Result;
            //    List = service.GetAllQuestions(Lesson.Id).Result;
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex.ToString());
            //    throw;
            //}
        }
        public void OnPostEdit(int? questionId, int id = 0)
        {
            try
            {
                if (questionId != null)
                {
                    Question = service.GetQuestionById(questionId).Result;
                    Answers = answerService.GetAllAnswers(Question.Id).Result;
                    Exercise = exservice.GetExerciseById(Question.ExerciseId).Result;
                }
                if(id != 0) 
                {
                    Exercise = exservice.GetExerciseById(id).Result;
                    Question = null;
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
        public void OnPostOpen(int? id, int questionId)
        {
            try
            {
                if (id == null)
                {
                    Answer = new Answer();
                    Answer.Id = 0;
                }
                else
                {
                    Answer = answerService.GetAnswerById(id).Result;
                }
                Answer.QuestionId = questionId;
                isEdit = true;
                OnPostEdit(questionId);
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
                OnPostEdit(null,Exercise.LessonId);
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
                OnPostEdit(Question.Id);
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
    }
}
