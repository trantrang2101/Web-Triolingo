using Web_Triolingo.ModelDto;

namespace Web_Triolingo.Interface.Lessons
{
    public interface ILessonService
    {
        Task<List<LessonDto>> GetAllLesson();
    }
}
