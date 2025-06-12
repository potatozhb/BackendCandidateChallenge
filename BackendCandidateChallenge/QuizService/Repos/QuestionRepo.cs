using QuizService.Data;
using QuizService.Model.Domain;
using QuizService.Repos.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QuizService.Repos
{
    public class QuestionRepo : IQuestionRepo
    {
        private readonly QuizDbContext quizDbContext;

        public QuestionRepo(QuizDbContext quizDb)
        {
            this.quizDbContext = quizDb;
        }

        public void AddQuestion(Question question)
        {
            this.quizDbContext.Questions.Add(question);
        }

        public bool Exist(int id)
        {
            return this.quizDbContext.Questions.Any(x => x.Id == id);
        }

        public Question GetQuestion(int id)
        {
            return this.quizDbContext.Questions.Where(q => q.Id == id).FirstOrDefault();
        }

        public ICollection<Question> GetQuestions()
        {
            return this.quizDbContext.Questions.ToList();
        }

        public ICollection<Question> GetQuestions(int quizId)
        {
            return this.quizDbContext.Questions.Where(q => q.QuizId == quizId).ToList();
        }

        public void RemoveQuestion(Question question)
        {
            this.quizDbContext.Questions.Remove(question);
        }

        public void UpdateQuestion(Question question)
        {
            this.quizDbContext.Update(question);
        }
    }
}
