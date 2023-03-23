using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Statistics
{
	public interface IStatisticService
	{
		int GetCurrentProgress(int userId, Course course);
		IDictionary<string, double> GetMarks(int userId, Course course);
		IEnumerable<Unit> GetUnits(int userId, Course course);
		IDictionary<Unit, List<Lesson>> GetLesson(int userId, Course course);
	}
}
