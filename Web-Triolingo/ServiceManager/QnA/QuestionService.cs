using Microsoft.EntityFrameworkCore;
using Triolingo.Core.DataAccess;
using Triolingo.Core.Entity;
using Web_Triolingo.Interface.QnA;

namespace Web_Triolingo.ServiceManager.QnA
{
    public class QuestionService : IQuestion
    {
        private readonly TriolingoDbContext _dbContext;
        public QuestionService(TriolingoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> AddNewQuestion(Question Question)
        {
            await _dbContext.Questions.AddAsync(Question);
            await _dbContext.SaveChangesAsync();
            return Question.Id;
        }

        public async Task<bool> DeleteQuestion(int Question)
        {
            var question = await _dbContext.Questions.Where(x => x.Id == Question).FirstOrDefaultAsync();
            if (question != null)
            {
                List<Answer> answerList = await _dbContext.Answers.Where(x => x.QuestionId == Question).ToListAsync();
                foreach (var answer in answerList)
                {
                    _dbContext.Answers.Remove(answer);
                }
                _dbContext.Questions.Remove(question);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EditQuestion(Question Question)
        {
            var question = await _dbContext.Questions.Where(x => x.Id == Question.Id).FirstOrDefaultAsync();
            if (question != null)
            {
                question.Question1 = Question.Question1;
                question.Mark = Question.Mark;
                _dbContext.Questions.Update(question);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Question>> GetAllQuestions(int exerciseId)
        {
            var lessons = await _dbContext.Questions.Where(x => x.ExerciseId== exerciseId).ToListAsync();
            return lessons;
        }

        public async Task<Question> GetQuestionById(int? id)
        {
            return await _dbContext.Questions.Where(x => x.Id== id).FirstOrDefaultAsync();
        }
    }
}
