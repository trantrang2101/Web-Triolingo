﻿using Triolingo.Core.Entity;
using Web_Triolingo.Interface.QnA;

namespace Web_Triolingo.ServiceManager.QnA
{
    public class QuestionService : IQuestion
    {
        public Task<int> AddNewQuestion(Question Question)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteQuestion(int Question)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditQuestion(Question Question)
        {
            throw new NotImplementedException();
        }

        public Task<List<Question>> GetAllQuestions(int exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetQuestionById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
