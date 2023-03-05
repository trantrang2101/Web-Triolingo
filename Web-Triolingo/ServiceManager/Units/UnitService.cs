using Microsoft.EntityFrameworkCore;
using Web_Triolingo.Interface.Units;
using Triolingo.Core.Entity;
using Triolingo.Core.DataAccess;
using Web_Triolingo.Interface.Courses;

namespace Web_Triolingo.ServiceManager.Units
{
    public class UnitService : IUnitService
    {
        private readonly TriolingoDbContext _dbContext;
        private readonly ICourseService _courseService;
        public UnitService(TriolingoDbContext dbContext,
            ICourseService courseService)
        {
            _dbContext = dbContext;
            _courseService = courseService;
        }
        public async Task<bool> AddUnit(Unit unit)
        {
            unit.Status = 1;
            await _dbContext.Units.AddAsync(unit);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ActiceUnit(int unitId)
        {
            var unit = GetById(unitId);
            if (unit != null)
            {
                unit.Status = 1;
                _dbContext.Update(unit);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeactiveUnit(int unitId)
        {
            var unit = GetById(unitId);
            if (unit != null)
            {
                unit.Status = 0;
                _dbContext.Update(unit);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public List<Unit> GetAll()
        {
            return _dbContext.Units.ToList();
        }

        public Unit GetById(int? unitId)
        {
            return _dbContext.Units.Where(x => x.Id == unitId).SingleOrDefault();
        }

        public List<Unit> GetUnitsByCourseId(int? courseId)
        {
            List<Unit> units = new List<Unit>();
            units = _dbContext.Units.Where(x => x.CourseId == courseId).ToList();
            return units;
        }

        public async Task<bool> UpdateUnit(Unit unit)
        {
            var item = await _dbContext.Units.SingleOrDefaultAsync(x => x.Id == unit.Id);
            if (item != null)
            {
                item.Order = unit.Order;
                item.Note = unit.Note;
                item.Description = unit.Description;
                item.Name = unit.Name;
                _dbContext.Units.Update(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Course GetCourseByUnitId(int unitId)
        {
            var unit = GetById(unitId);
            return _courseService.GetCourseById(unit.CourseId).Result;
        }
    }
}
