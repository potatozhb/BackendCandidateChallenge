using QuizService.Data;
using QuizService.Model.Domain;
using QuizService.Repos.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QuizService.Repos
{
    public class AnswerRepo : IAnswerRepo
    {
        private readonly QuizDbContext quizDbContext;
        public AnswerRepo(QuizDbContext quizDb)
        {
            this.quizDbContext = quizDb;
        }

        public void AddAnswer(Answer answer)
        {
            this.quizDbContext.Answers.Add(answer);
        }

        public bool Exist(int id)
        {
            return this.quizDbContext.Answers.Any(x => x.Id == id);
        }

        public Answer GetAnswer(int id)
        {
            return this.quizDbContext.Answers.Where(a => a.Id == id).FirstOrDefault();
        }

        public ICollection<Answer> GetAnswers()
        {
            return this.quizDbContext.Answers.ToList();
        }

        public ICollection<Answer> GetAnswers(int questionId)
        {
            return this.quizDbContext.Answers.Where(a => a.QuestionId == questionId).ToList();
        }

        public void RemoveAnswer(Answer answer)
        {
            this.quizDbContext.Answers.Remove(answer);
        }

        public void UpdateAnswer(Answer answer)
        {
            this.quizDbContext.Update(answer);
        }
    }
}
