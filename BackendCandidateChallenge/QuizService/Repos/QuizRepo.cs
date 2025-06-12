using QuizService.Data;
using QuizService.Model;
using QuizService.Model.Domain;
using QuizService.Repos.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QuizService.Repos
{
    public class QuizRepo : IQuizRepo
    {
        private readonly QuizDbContext quizDbContext;
        public QuizRepo(QuizDbContext quizDb)
        {
            this.quizDbContext = quizDb;
        }

        public void AddQuiz(Quiz quiz)
        {
            this.quizDbContext.Quizzes.Add(quiz);
        }

        public bool Exist(int id)
        {
            return this.quizDbContext.Quizzes.Any(x => x.Id == id);
        }

        public Quiz GetQuiz(int id)
        {
            return this.quizDbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();
        }

        public ICollection<Quiz> GetQuizzes()
        {
            return this.quizDbContext.Quizzes.ToList();
        }

        public void RemoveQuiz(Quiz quiz)
        {
            this.quizDbContext.Quizzes.Remove(quiz);
        }

        public void UpdateQuiz(Quiz quiz)
        {
            this.quizDbContext.Update(quiz);
        }
    }
}
