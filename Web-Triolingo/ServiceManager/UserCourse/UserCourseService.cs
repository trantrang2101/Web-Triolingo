using Microsoft.EntityFrameworkCore;
using Triolingo.Core.DataAccess;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.UserCourse;

namespace Web_Triolingo.ServiceManager.UserCourse
{
    public class UserCourseService : IUserCourse
    {
        private readonly TriolingoDbContext _context;

        public UserCourseService(TriolingoDbContext context)
        {
            _context = context;
        }

        public List<Course> getCourseByMentor(int mentorId)
        {
            return _context.StudentCourses.Include(x=>x.Course).Where(x => x.IsStudent == false && x.StudentId== mentorId).Select(x => x.Course).ToList();
        }

        public List<int> getUserIdInCourse(int courseId)
        {
            return _context.StudentCourses.Where(x => x.IsStudent == false && x.CourseId == courseId).Select(x => x.StudentId).ToList();
        }

        public void updateMentorAdd(List<User> mentors, List<bool> isMentor, int courseId)
        {
            for (var i = 0; i < mentors.Count; i++)
            {
                User mentor = mentors[i];
                StudentCourse student = _context.StudentCourses.FirstOrDefault(x => x.CourseId == courseId && mentor.Id == x.StudentId && x.IsStudent == false);
                if (isMentor[i])
                {
                    if (student == null)
                    {
                        student = new StudentCourse()
                        {
                            CourseId = courseId,
                            StudentId = mentor.Id,
                            IsStudent = false
                        };
                        _context.StudentCourses.Add(student);
                    }
                }
                else
                {
                    if (student != null)
                        _context.StudentCourses.Remove(student);
                }
                _context.SaveChanges();
            }
        }
    }
}
