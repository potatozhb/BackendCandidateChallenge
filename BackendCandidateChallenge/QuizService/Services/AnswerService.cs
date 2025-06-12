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
    public class AnswerService : IAnswerService
    {
        private readonly QuizDbContext quizDbContext;
        private readonly IQuizRepo quizRepo;
        private readonly IQuestionRepo questionRepo;
        private readonly IAnswerRepo answerRepo;

        public AnswerService(QuizDbContext quizDb, IQuizRepo quizRepo, IQuestionRepo questionRepo, IAnswerRepo answerRepo)
        {
            this.quizDbContext = quizDb;
            this.quizRepo = quizRepo;
            this.questionRepo = questionRepo;
            this.answerRepo = answerRepo;
        }

        public Answer AddAnswer(int questionId, AnswerCreateModel answer)
        {
            if (!this.questionRepo.Exist(questionId)) throw new NotFoundException("Not found this question");

            int maxid = this.answerRepo.GetAnswers()
                .OrderByDescending(q => q.Id)
                .Select(q => q.Id)
                .FirstOrDefault();

            var answerfull = new Answer
            {
                Id = maxid + 1,
                QuestionId = questionId,
                Text = answer.Text,
            };

            this.answerRepo.AddAnswer(answerfull);
            this.Save();

            return answerfull;
        }

        public bool Exist(int id)
        {
            return this.answerRepo.Exist(id);
        }

        public Answer GetAnswer(int id)
        {
            if (!this.answerRepo.Exist(id)) throw new NotFoundException("Not found this answer");

            var answer = this.answerRepo.GetAnswer(id);
            return answer;
        }

        public ICollection<Answer> GetAnswers(int questionId)
        {
            if (!this.questionRepo.Exist(questionId)) throw new NotFoundException("Not found this question");

            var answers = this.answerRepo.GetAnswers(questionId);
            return answers;
        }

        public Answer RemoveAnswer(int questionId, int id)
        {
            if (!this.questionRepo.Exist(questionId)) throw new NotFoundException("Not found this question");

            var answer = this.answerRepo.GetAnswer(id);
            this.answerRepo.RemoveAnswer(answer);
            this.Save();
            return answer;
        }

        public bool Save()
        {
            int n = this.quizDbContext.SaveChanges();
            return n > 0;
        }

        public Answer UpdateAnswer(int quizId, int questionId, int id, AnswerUpdateModel answer)
        {
            if (!this.quizRepo.Exist(quizId)) throw new NotFoundException("Not found this quiz");
            if (!this.questionRepo.Exist(questionId)) throw new NotFoundException("Not found this question");
            if (!this.answerRepo.Exist(id)) throw new NotFoundException("Not found this answer");

            var answerfull = this.answerRepo.GetAnswer(id);
            if (answerfull.QuestionId != questionId) throw new CustomException("this answer is not belong the question");

            answerfull.Text = answer.Text;

            this.answerRepo.UpdateAnswer(answerfull);
            this.Save();
            return answerfull;
        }
    }
}
