using QuizService.Data;
using QuizService.Exceptions;
using QuizService.Model;
using QuizService.Model.Domain;
using QuizService.Repos.Interfaces;
using QuizService.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace QuizService.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly QuizDbContext quizDbContext;
        private readonly IQuizRepo quizRepo;
        private readonly IQuestionRepo questionRepo;
        private readonly IAnswerRepo answerRepo;

        public QuestionService(QuizDbContext quizDb, IQuizRepo quizRepo, IQuestionRepo questionRepo, IAnswerRepo answerRepo)
        {
            this.quizDbContext = quizDb;
            this.quizRepo = quizRepo;
            this.questionRepo = questionRepo;
            this.answerRepo = answerRepo;
        }

        public Question AddQuestion(int quizId, QuestionCreateModel question)
        {
            if (!this.quizRepo.Exist(quizId)) throw new NotFoundException("Not found this quiz");

            int maxid = this.questionRepo.GetQuestions()
                .OrderByDescending(q => q.Id)
                .Select(q => q.Id)
                .FirstOrDefault();

            var questionfull = new Question
            {
                Id = maxid + 1,
                QuizId = quizId,
                Text = question.Text,
            };

            this.questionRepo.AddQuestion(questionfull);
            this.Save();

            return questionfull;
        }

        public bool Exist(int id)
        {
            return this.questionRepo.Exist(id);
        }

        public ICollection<Question> GetQuestions(int quizId)
        {
            if (!this.quizRepo.Exist(quizId)) throw new NotFoundException("Not found this quiz");

            var questions = this.questionRepo.GetQuestions(quizId);
            return questions;
        }

        public Question GetQuestion(int id)
        {
            if (!this.questionRepo.Exist(id)) throw new NotFoundException("Not found this question");

            var question = this.questionRepo.GetQuestion(id);
            return question;
        }

        public Question RemoveQuestion(int quizId, int id)
        {
            if (!this.quizRepo.Exist(quizId)) throw new NotFoundException("Not found this quiz");
            if (!this.questionRepo.Exist(id)) throw new NotFoundException("Not found this question");

            var question = this.questionRepo.GetQuestion(id);
            this.questionRepo.RemoveQuestion(question);
            this.Save();
            return question;
        }

        public bool Save()
        {
            int n = this.quizDbContext.SaveChanges();
            return n > 0;
        }

        public Question UpdateQuestion(int quizId, int questionId, QuestionUpdateModel question)
        {
            if (!this.quizRepo.Exist(quizId)) throw new NotFoundException("Not found this quiz");
            if (!this.questionRepo.Exist(questionId)) throw new NotFoundException("Not found this question");
            if (!this.answerRepo.Exist(question.CorrectAnswerId)) throw new NotFoundException("Not found this answer");

            var answer = this.answerRepo.GetAnswer(question.CorrectAnswerId);
            if (answer.QuestionId != questionId) throw new CustomException("this answer is not belong the question");

            var questionfull = this.questionRepo.GetQuestion(questionId);
            questionfull.CorrectAnswerId = question.CorrectAnswerId;
            questionfull.Text = question.Text;

            this.questionRepo.UpdateQuestion(questionfull);
            this.Save();
            return questionfull;
        }
    }
}
