using Triolingo.Core.DataAccess;
using Web_Triolingo.Interface.Statistics;
using System.Linq;
using Triolingo.Core.Entity;
using System.Collections;

namespace Web_Triolingo.ServiceManager.Statistic
{
	public class StatisticService : IStatisticService
	{
		private readonly TriolingoDbContext _context;

		public StatisticService(TriolingoDbContext context)
		{
			_context = context;
		}

		public int GetCurrentProgress(int userId, out Course course)
		{
			var courses = from studentCourse in _context.StudentCourses
						  where studentCourse.StudentId == userId
						  select studentCourse;
			var lesson = from stuLesson in _context.StudentLessons
						 join stuCourse in courses
						   on stuLesson.StudentCourseId equals stuCourse.Id
						 select new
						 {
							 stuLesson.LessionId,
							 stuCourse.CourseId,
						 };
			course = null;
			if (lesson == null || !lesson.Any()) return 0;

			int lastCourseId = lesson.ToArray()[lesson.Count() - 1].CourseId;
			course = (from course_ in _context.Courses
					  where course_.Id == lastCourseId && course_.Status > 0
					  select course_).FirstOrDefault();
			if (course == null) return 0;

			int courseId = course.Id;
			int lessonCount = (from lesson_ in _context.Lessons.Where(l => l.Status > 0)
							   join unit in _context.Units.Where(u => u.Status > 0)
								 on lesson_.UnitId equals unit.Id
							   where unit.CourseId == courseId
							   select lesson_).Count();
			return lesson.Count() * 100 / lessonCount;
		}

		public IDictionary<string, double> GetMarks(int userId)
		{
			Dictionary<string, double> data = new Dictionary<string, double>();

			var courses = from studentCourse in _context.StudentCourses
						  where studentCourse.StudentId == userId
						  select studentCourse;
			var lessons = from stuLesson in _context.StudentLessons
						 join stuCourse in courses
						   on stuLesson.StudentCourseId equals stuCourse.Id
						 join lesson in _context.Lessons.Where(l => l.Status > 0)
							on stuLesson.LessionId equals lesson.Id
						 select new
						 {
							 stuLesson.Mark,
							 lesson
						 };

			int lessonCount = lessons.Count();
			foreach(var lesson in lessons.ToList())
			{
				int totalMark = GetTotalMarkOfLesson(lesson.lesson.Id);
				if (totalMark != 0)
				{
					if (data.Count == 0)
					{
						data.Add(string.Empty, 0);
					}
					data.Add(lesson.lesson.Name, Math.Clamp((int)lesson.Mark * 10 / totalMark, 0, 10));
				}
			}

			return data;
		}

		public IEnumerable<string> GetUnits(int userId, out int currentIndex)
		{
			currentIndex = -1;
			var courses = from studentCourse in _context.StudentCourses
						  where studentCourse.StudentId == userId
						  select studentCourse;
			var lessons = from stuLesson in _context.StudentLessons
						  join stuCourse in courses
							on stuLesson.StudentCourseId equals stuCourse.Id
						  join lesson in _context.Lessons.Where(l => l.Status > 0)
							 on stuLesson.LessionId equals lesson.Id
						  select lesson.UnitId;
			var units = (from unit in _context.Units.Where(u => u.Status > 0)
						where lessons.Contains(unit.Id)
						select unit.Name);
			currentIndex = units.Any() ? units.Count() - 1 : 0;
			var notStudyUnit = (from unit in _context.Units.Where(u => u.Status > 0)
							   where !lessons.Contains(unit.Id)
							   select unit.Name);
			return units.Concat(notStudyUnit).AsEnumerable();
		}

		int GetTotalMarkOfLesson(int lessonId)
		{
			return (from exercise in _context.Exercises.Where(e => e.Status > 0)
					join question in _context.Questions.Where(e => e.Status > 0)
						on exercise.Id equals question.ExerciseId
					where exercise.LessonId == lessonId
					select question.Mark).Sum();
		}
	}
}
