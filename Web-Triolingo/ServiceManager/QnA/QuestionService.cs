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
        public async Task<bool> AddNewQuestion(Question Question)
        {
            Question cour = Question;
            await _dbContext.Questions.AddAsync(cour);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditQuestion(Question course)
        {
            var getQuestion = await _dbContext.Questions.Where(x => x.Id == course.Id).FirstOrDefaultAsync();
            if (getQuestion != null)
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            var courses = await _dbContext.Questions.ToListAsync();
            return courses;
        }

        public async Task<Question> GetQuestionById(int? id)
        {
            var course = await _dbContext.Questions.Where(x => x.Id == id).FirstOrDefaultAsync();
            return course;
        }
    }
}
