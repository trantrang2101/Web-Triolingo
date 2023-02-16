using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.DBContext;
using Web_Triolingo.Model;
using Web_Triolingo.Interface.QnA;

namespace Web_Triolingo.ServiceManager.QnA
{
    public class QuestionService : IQuestion
    {
        private readonly IMapper _mapper;
        public QuestionService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<bool> AddNewQuestion(Question Question)
        {
            Question cour = Question;
            await DataProvider.Ins.DB.Questions.AddAsync(cour);
            await DataProvider.Ins.DB.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditQuestion(Question course)
        {
            var getQuestion = await DataProvider.Ins.DB.Questions.Where(x => x.Id == course.Id).FirstOrDefaultAsync();
            if (getQuestion != null)
            {
                await DataProvider.Ins.DB.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            var courses = await DataProvider.Ins.DB.Questions.ToListAsync();
            var result = _mapper.Map<List<Question>>(courses);
            return result;
        }

        public async Task<Question> GetQuestionById(int? id)
        {
            var course = await DataProvider.Ins.DB.Questions.Where(x => x.Id == id).FirstOrDefaultAsync();
            var result = _mapper.Map<Question>(course);
            return result;
        }
    }
}
