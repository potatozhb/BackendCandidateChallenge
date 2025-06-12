using QuizService.Data;
using QuizService.Model.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QuizService
{
    public class Seed
    {
        private readonly QuizDbContext dataContext;
        public Seed(QuizDbContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void AddData()
        {
            if (!this.dataContext.Questions.Any())
            {
                var quizzes = new List<Quiz>()
                    {
                        new Quiz() { Id = 1, Title = "My first quiz" },
                        new Quiz() { Id = 2, Title = "My second quiz" },
                    };

                var questions = new List<Question>()
                    {
                        new Question { Id = 1, Text = "My first question", QuizId = 1 },
                        new Question { Id = 2, Text = "My second question", QuizId = 1 },
                    };

                var answers = new List<Answer>()
                    {
                        new Answer() { Id = 1, QuestionId = 1, Text = "My first answer to first q" },
                        new Answer() { Id = 2, QuestionId = 1, Text = "My second answer to first q" },
                        new Answer() { Id = 3, QuestionId = 2, Text = "My first answer to second q" },
                        new Answer() { Id = 4, QuestionId = 2, Text = "My second answer to second q" },
                        new Answer() { Id = 5, QuestionId = 2, Text = "My third answer to second q" }
                    };

                this.dataContext.Quizzes.AddRange(quizzes);
                this.dataContext.SaveChanges();

                this.dataContext.Questions.AddRange(questions);
                this.dataContext.SaveChanges();

                this.dataContext.Answers.AddRange(answers);
                this.dataContext.SaveChanges();

                var q1 = this.dataContext.Questions.First(q => q.Id == 1);
                q1.CorrectAnswerId = 1;

                var q2 = this.dataContext.Questions.First(q => q.Id == 2);
                q2.CorrectAnswerId = 5;

                this.dataContext.SaveChanges();
            }

        }
    }
}
