using Triolingo.Core.Entity;
using Web_Triolingo.Interface.QnA;

namespace Web_Triolingo.ServiceManager.QnA
{
    public class AnswerService : IAnswer
    {
        public Task<bool> AddNewAnswer(Answer Answer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAnswer(int Question)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditAnswer(Answer Answer)
        {
            throw new NotImplementedException();
        }

        public Task<List<Answer>> GetAllAnswers(int questionId)
        {
            throw new NotImplementedException();
        }

        public Task<Answer> GetAnswerById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
