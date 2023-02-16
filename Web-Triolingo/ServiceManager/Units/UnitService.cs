using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Units;
using Web_Triolingo.Model;

namespace Web_Triolingo.ServiceManager.Units
{
    public class UnitService : IUnitService
    {
        public async Task<Unit> GetById(int? unitId)
        {
            return await DataProvider.Ins.DB.Units.Where(x => x.Id == unitId).FirstOrDefaultAsync();
        }

        public async Task<List<Unit>> GetUnitsByCourseId(int? courseId)
        {
            List<Unit> units = new List<Unit>();
            using (var context = new TriolingoDBContext())
            {
                units = await context.Units.Where(x => x.CourseId == courseId).ToListAsync();
            }
            return units;
        }
    }
}
