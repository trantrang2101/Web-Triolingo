using Microsoft.EntityFrameworkCore;
using Web_Triolingo.Interface.Units;
using Triolingo.Core.Entity;
using Triolingo.Core.DataAccess;

namespace Web_Triolingo.ServiceManager.Units
{
    public class UnitService : IUnitService
    {
        private readonly TriolingoDbContext _dbContext;
        public UnitService(TriolingoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddUnit(Unit unit)
        {
            await _dbContext.Units.AddAsync(unit);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public List<Unit> GetAll()
        {
            return _dbContext.Units.ToList();
        }

        public Unit GetById(int? unitId)
        {
            return _dbContext.Units.Where(x => x.Id == unitId).FirstOrDefault();
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
                item.CourseId = unit.CourseId;
                item.Description = unit.Description;
                item.Name = unit.Name;
                _dbContext.Units.Update(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
