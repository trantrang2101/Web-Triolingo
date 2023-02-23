using Triolingo.Core.Entity;

namespace Web_Triolingo.Interface.QnA
{
    public interface IAnswer
    {
        Task<List<Answer>> GetAllAnswers(int questionId);
        Task<Answer> GetAnswerById(int? id);
        Task<bool> AddNewAnswer(Answer Answer);
        Task<bool> EditAnswer(Answer Answer);
        Task<bool> DeleteAnswer(int Question);
    }
}
