﻿using Web_Triolingo.Model;

namespace Web_Triolingo.Interface.Units
{
    public interface IUnitService
    {
        List<Unit> GetUnitsByCourseId(int? courseId);
        Unit GetById(int? unitId);
        List<Unit> GetAll();
        Task<bool> UpdateUnit(Unit unit);
        Task<bool> AddUnit(Unit unit);
    }
}
