using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.Lessons
{
    public class LessonService : ILessonService
    {
        public async Task<List<LessonDto>> GetAllLesson()
        {
            var lessons = await DataProvider.Ins.DB.Lessons.ToListAsync();
            List<LessonDto> result = new List<LessonDto>();
            lessons.ForEach(se =>
            {
                result.Add(new LessonDto()
                {
                    Id = se.Id,
                    Name = se.Name,
                    UnitId = se.UnitId,
                    TypeId = se.TypeId,
                    Description = se.Description,
                    Note = se.Note,
                    Status = se.Status
                });
            });
            return result;
        }
    }
}
