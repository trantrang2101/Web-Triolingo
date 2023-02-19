using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using Web_Triolingo.Interface.Courses;
using Triolingo.Core.Entity;
using Triolingo.Core.DataAccess;

namespace Web_Triolingo.ServiceManager.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly TriolingoDbContext _dbContext;
        public CourseService(IMapper mapper, TriolingoDbContext dBContext)
        {
            _mapper = mapper;
            _dbContext = dBContext;
        }
        public async Task<bool> AddNewCourse(Course Course)
        {
            Course cour = new Course();
            await _dbContext.Courses.AddAsync(cour);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditCourse(Course course)
        {
            var getCourse = await _dbContext.Courses.Where(x => x.Id == course.Id).FirstOrDefaultAsync();
            if (getCourse != null)
            {
                getCourse.Name=course.Name;
                getCourse.Description = course.Description;
                getCourse.Note= course.Note;
                getCourse.Status = course.Status;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Course>> GetAllCourse()
        {
            var courses = await _dbContext.Courses.ToListAsync();
            var result = _mapper.Map<List<Course>>(courses);
            return result;
        }

        public async Task<Course> GetCourseById(int? id)
        {
            var course = await _dbContext.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();
            var result = _mapper.Map<Course>(course);
            return result;
        }
    }
}
