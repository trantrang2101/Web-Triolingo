using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.Statistics
{
	public interface IStatisticService
	{
		int GetCurrentProgress(int userId, out Course course);
		IDictionary<string, double> GetMarks(int userId);
		IEnumerable<string> GetUnits(int userId, out int currentIndex);
	}
}
