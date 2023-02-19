using Web_Triolingo.ModelDto;
using Web_Triolingo.Model;

namespace Web_Triolingo.Interface.Lessons
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLesson();
        Task<Lesson> GetLessonById(int id);
        Task<bool> AddLesson(LessonDto lesson);
        Task<bool> UpdateLesson(Lesson lesson);
        Task<bool> DeleteLesson(int id);
        Task<List<Lesson>> getAllLessonsByUnitId(int? unitId);
    }
}
