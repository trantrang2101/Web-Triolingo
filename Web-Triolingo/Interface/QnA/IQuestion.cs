using Web_Triolingo.Model;

namespace Web_Triolingo.Interface.QnA
{
    public interface IQuestion
    {
        Task<List<Question>> GetAllQuestion(int? lessonId);
        Task<Question> GetQuestionById(int? id);
        Task<bool> AddNewQuestion(Question Question);
        Task<bool> EditQuestion(Question Question);
    }
}
