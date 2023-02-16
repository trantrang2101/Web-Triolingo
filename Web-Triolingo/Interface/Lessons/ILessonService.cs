using Web_Triolingo.Model;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.Lessons
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllLesson();
        Task<Lesson> GetLessonById(int? id);
        Task<List<LessonDto>> GetAllLessonDTO();
    }
}
