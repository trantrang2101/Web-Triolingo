using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Units;
using Web_Triolingo.Model;

namespace Web_Triolingo.ServiceManager.Units
{
    public class UnitService : IUnitService
    {
        public async Task<bool> AddUnit(Unit unit)
        {
            using (var context = new TriolingoDBContext())
            {
                await context.Units.AddAsync(unit);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public List<Unit> GetAll()
        {
            using (var context = new TriolingoDBContext())
            {
                return context.Units.ToList();
            }
        }

        public Unit GetById(int? unitId)
        {
            return DataProvider.Ins.DB.Units.Where(x => x.Id == unitId).FirstOrDefault();
        }

        public List<Unit> GetUnitsByCourseId(int? courseId)
        {
            List<Unit> units = new List<Unit>();
            using (var context = new TriolingoDBContext())
            {
                units = context.Units.Where(x => x.CourseId == courseId).ToList();
            }
            return units;
        }

        public async Task<bool> UpdateUnit(Unit unit)
        {
            var item = await DataProvider.Ins.DB.Units.SingleOrDefaultAsync(x => x.Id == unit.Id);
            if (item != null)
            {
                item.Order = unit.Order;
                item.Note = unit.Note;
                item.CourseId = unit.CourseId;
                item.Description = unit.Description;
                item.Name = unit.Name;
                 DataProvider.Ins.DB.Units.Update(item);
                await DataProvider.Ins.DB.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
