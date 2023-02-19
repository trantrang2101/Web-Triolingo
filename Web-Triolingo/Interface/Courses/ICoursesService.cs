using Web_Triolingo.Common;
using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Courses
{
    public interface ICourseService
    {
        Task<List<Course>> GetAllCourse();
        Task<Course> GetCourseById(int? id);
        Task<bool> AddNewCourse(Course Course);
        Task<bool> EditCourse(Course Course);
    }
}
