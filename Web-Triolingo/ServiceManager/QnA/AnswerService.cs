using Microsoft.EntityFrameworkCore;
using Triolingo.Core.DataAccess;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.QnA;

namespace Web_Triolingo.ServiceManager.QnA
{
    public class AnswerService : IAnswer
    {
        private readonly TriolingoDbContext _dbContext;
        public AnswerService(TriolingoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddNewAnswer(Answer Answer)
        {
            await _dbContext.AddAsync(Answer);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAnswer(int Question)
        {
            Answer answer = await _dbContext.Answers.Where(x=>x.Id== Question).FirstOrDefaultAsync();
            if(answer != null)
            {
                _dbContext.Answers.Remove(answer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EditAnswer(Answer Answer)
        {
            Answer answer = await _dbContext.Answers.Where(x => x.Id == Answer.Id).FirstOrDefaultAsync();
            if (answer != null)
            {
                answer.Answer1 = Answer.Answer1;
                answer.IsCorrect = Answer.IsCorrect;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Answer>> GetAllAnswers(int questionId)
        {
            return await _dbContext.Answers.Where(x => x.QuestionId== questionId).ToListAsync();
        }

        public async Task<Answer> GetAnswerById(int? id)
        {
            return await _dbContext.Answers.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
