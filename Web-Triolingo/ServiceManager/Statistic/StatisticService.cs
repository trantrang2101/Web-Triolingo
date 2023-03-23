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

		public int GetCurrentProgress(int userId, Course course)
		{
			var courses = from studentCourse in _context.StudentCourses
						  where studentCourse.StudentId == userId && studentCourse.CourseId == course.Id
						  select studentCourse;
			var lesson = from stuLesson in _context.StudentLessons
						 join stuCourse in courses
						   on stuLesson.StudentCourseId equals stuCourse.Id
						 select new
						 {
							 stuLesson.LessionId,
							 stuCourse.CourseId,
						 };
			if (lesson == null || !lesson.Any()) return 0;

			int courseId = course.Id;
			int lessonCount = (from lesson_ in _context.Lessons.Where(l => l.Status > 0)
							   join unit in _context.Units.Where(u => u.Status > 0)
								 on lesson_.UnitId equals unit.Id
							   where unit.CourseId == courseId
							   select lesson_).Count();
			return lesson.Count() * 100 / lessonCount;
		}

		public IDictionary<Unit, List<Lesson>> GetLesson(int userId, Course course)
		{
			Dictionary<Unit, List<Lesson>> data = new Dictionary<Unit, List<Lesson>>();
			var lessons = from stuLesson in _context.StudentLessons
						  join lesson in _context.Lessons.Where(l => l.Status > 0)
							 on stuLesson.LessionId equals lesson.Id
						  where stuLesson.StudentCourseId == course.Id
						  select lesson.Id;
			var units = (from unit in _context.Units.Where(u => u.Status > 0)
						 where lessons.Contains(unit.Id)
						 select unit);
			foreach(var unit in units)
			{
				unit.Status=1;
			}
			var notStudyUnit = (from unit in _context.Units.Where(u => u.Status > 0 && u.CourseId == course.Id)
								where !lessons.Contains(unit.Id)
								select unit);
			foreach (var unit in notStudyUnit)
			{
				unit.Status = 0;
			}
			var listUnit = units.Concat(notStudyUnit).ToList();
			foreach(var unit in listUnit)
			{
				List<Lesson> list = _context.Lessons.Where(x => x.Status > 0 && x.UnitId == unit.Id).ToList();
				foreach(var lesson in list)
				{
					if (lessons.Contains(lesson.Id))
					{
						lesson.Status = 1;
					}
					else
					{
						lesson.Status = 0;
					}
				}
				data.Add(unit,list);
			}
			return data;

		}

		public IDictionary<string, double> GetMarks(int userId,Course course)
		{
			Dictionary<string, double> data = new Dictionary<string, double>();

			var courses = from studentCourse in _context.StudentCourses
						  where studentCourse.StudentId == userId && studentCourse.CourseId==course.Id
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
					data.Add(lesson.lesson.Name, Math.Clamp((int)lesson.Mark * 100 / totalMark, 0, 100));
				}
			}

			return data;
		}

		public IEnumerable<Unit> GetUnits(int userId,Course course)
		{
			var lessons = from stuLesson in _context.StudentLessons
						  join lesson in _context.Lessons.Where(l => l.Status > 0)
							 on stuLesson.LessionId equals lesson.Id
						where stuLesson.StudentCourseId == course.Id
						  select lesson.UnitId;
			var units = (from unit in _context.Units.Where(u => u.Status > 0)
						where lessons.Contains(unit.Id)
						select unit);
			var notStudyUnit = (from unit in _context.Units.Where(u => u.Status > 0 && u.CourseId == course.Id)
							   where !lessons.Contains(unit.Id)
							   select unit);
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
