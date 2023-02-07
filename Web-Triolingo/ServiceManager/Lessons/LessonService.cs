using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.ModelDto;

namespace Web_Triolingo.ServiceManager.Lessons
{
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        public LessonService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<LessonDto>> GetAllLesson()
        {
            var lessons = await DataProvider.Ins.DB.Lessons.ToListAsync();
            var result = _mapper.Map<List<LessonDto>>(lessons);

            return result;
        }
    }
}
