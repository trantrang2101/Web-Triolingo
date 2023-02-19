using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Model;

namespace Web_Triolingo.ServiceManager.Lessons
{
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        public LessonService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<UserDto> FindExistLesson(string name)
        {
            var userLoged = await DataProvider.Ins.DB.Lessons.Where(x => x.Name == name && x.Status == 1).FirstOrDefaultAsync();
            var result = _mapper.Map<UserDto>(userLoged);
            return result;
        }

        public async Task<List<Lesson>> GetAllLesson()
        {
            using (var context = new TriolingoDBContext())
            {
                var lessons = await context.Lessons.Where(x => x.Status == 1).ToListAsync();
                var result = _mapper.Map<List<Lesson>>(lessons);

                return result;
            }
        }
        public async Task<Lesson> GetLessonById(int id)
        {
            using (var context = new TriolingoDBContext())
            {
                var lessons = await context.Lessons.Where(x => x.Status == 1 && x.Id == id).FirstOrDefaultAsync();
                var result = _mapper.Map<Lesson>(lessons);
                return result;
            }
        }

        public async Task<bool> AddLesson(LessonDto lesson)
        {
            var check = FindExistLesson(lesson.Name);
            if (check != null)
            {
                Lesson newUser = new Lesson()
                {
                    Name= lesson.Name,
                    Status = 1,
                    Note = lesson.Note,
                    Description = lesson.Description,
                    UnitId = lesson.UnitId,
                };
                using (var context = new TriolingoDBContext())
                {
                    await context.AddAsync(newUser);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateLesson(Lesson lesson)
        {
            using (var context = new TriolingoDBContext())
            {
                var lessons = await context.Lessons.Where(x => x.Status == 1 && x.Id == lesson.Id).FirstOrDefaultAsync();
                if (lessons != null)
                {
                    lessons.Name= lesson.Name;
                    lessons.Status= 1;
                    lessons.Note= lesson.Note;
                    lessons.Description= lesson.Description;
                    lessons.UnitId= lesson.UnitId;
                    context.Update(lessons);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> DeleteLesson(int id)
        {
            using (var context = new TriolingoDBContext())
            {
                var lessons = await context.Lessons.Where(x => x.Status == 1 && x.Id == id).FirstOrDefaultAsync();
                if (lessons != null)
                {
                    lessons.Status= 0;
                    context.Update(lessons);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<List<Lesson>> getAllLessonsByUnitId(int? unitId)
        {
            using (var context = new TriolingoDBContext())
            {
                var lessons = await context.Lessons.Where(x => x.Status == 1 && x.UnitId == unitId).ToListAsync();
                var result = _mapper.Map<List<Lesson>>(lessons);

                return result;
            }
        }
    }
}
