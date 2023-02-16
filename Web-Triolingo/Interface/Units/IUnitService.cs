using Web_Triolingo.Model;

namespace Web_Triolingo.Interface.Units
{
    public interface IUnitService
    {
        Task<List<Unit>> GetUnitsByCourseId(int? courseId);
        Task<Unit> GetById(int? unitId);
    }
}
