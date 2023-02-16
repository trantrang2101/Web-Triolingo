using Web_Triolingo.ModelDto;
using Web_Triolingo.Models;

namespace Web_Triolingo.Interface.Lessons
{
    public interface ILessonService
    {
        Task<List<LessonDto>> GetAllLesson();
        Task<LessonDto> GetLessonById(int id);
        Task<bool> AddLesson(LessonDto lesson);
        Task<bool> UpdateLesson(LessonDto lesson);
        Task<bool> DeleteLesson(int id);
        Task<List<LessonDto>> getAllLessonsByUnitId(int? unitId);
    }
}
