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

        public List<int> getUserIdInCourse(int courseId)
        {
            return _context.StudentCourses.Where(x => x.IsStudent == false && x.CourseId == courseId).Select(x => x.StudentId).ToList();
        }

        public void updateMentorAdd(Dictionary<User, bool> mentors, int courseId)
        {
            foreach(var mentor in mentors)
            {
                StudentCourse student = _context.StudentCourses.FirstOrDefault(x => x.CourseId == courseId&&mentor.Key.Id==x.StudentId&&x.IsStudent==false);
                if (mentor.Value)
                {
                    if (student == null)
                    {
                        student = new StudentCourse()
                        {
                            CourseId = courseId,
                            StudentId = mentor.Key.Id,
                            IsStudent = false
                        };
                        _context.StudentCourses.Add(student);
                    }
                }
                else
                {
                    _context.StudentCourses.Remove(student);
                }
                _context.SaveChanges();
            }
        }
    }
}
