using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Units
{
    public interface IUnitService
    {
        List<Unit> GetUnitsByCourseId(int? courseId);
        Unit GetById(int? unitId);
        List<Unit> GetAll();
        Task<bool> ActiceUnit(int unitId);
        Task<bool> DeactiveUnit(int unitId);
        Task<bool> UpdateUnit(Unit unit);
        Task<bool> AddUnit(Unit unit);
        Course GetCourseByUnitId(int unitId);
        bool IsDuplicateUnitAdd(Unit unit);
        bool IsDuplicateUnitEdit(Unit unit);
    }
}
