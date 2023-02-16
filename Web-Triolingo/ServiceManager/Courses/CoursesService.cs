using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using Web_Triolingo.Common;
using Web_Triolingo.DBContext;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Model;

namespace Web_Triolingo.ServiceManager.Courses
{
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        public CourseService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<bool> AddNewCourse(Course Course)
        {
            Course cour = Course;
            await DataProvider.Ins.DB.Courses.AddAsync(cour);
            await DataProvider.Ins.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditCourse(Course course)
        {
            var getCourse = await DataProvider.Ins.DB.Courses.Where(x => x.Id == course.Id).FirstOrDefaultAsync();
            if (getCourse != null)
            {
                getCourse.Name=course.Name;
                getCourse.Description = course.Description;
                getCourse.Note= course.Note;
                getCourse.Status = course.Status;
                await DataProvider.Ins.DB.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Course>> GetAllCourse()
        {
            var courses = await DataProvider.Ins.DB.Courses.ToListAsync();
            var result = _mapper.Map<List<Course>>(courses);
            return result;
        }

        public async Task<Course> GetCourseById(int? id)
        {
            var course = await DataProvider.Ins.DB.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();
            var result = _mapper.Map<Course>(course);
            return result;
        }
    }
}
