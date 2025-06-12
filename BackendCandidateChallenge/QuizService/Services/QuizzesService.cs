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
    public class QuizzesService : IQuizService
    {
        private readonly QuizDbContext quizDbContext;
        private readonly IQuizRepo quizRepo;
        private readonly IQuestionRepo questionRepo;
        private readonly IAnswerRepo answerRepo;

        public QuizzesService(QuizDbContext quizDb, IQuizRepo quizRepo, IQuestionRepo questionRepo, IAnswerRepo answerRepo)
        {
            this.quizDbContext = quizDb;
            this.quizRepo = quizRepo;
            this.questionRepo = questionRepo;
            this.answerRepo = answerRepo;
        }

        public Quiz AddQuiz(QuizCreateModel quiz)
        {
            int maxid = this.quizRepo.GetQuizzes()
                .OrderByDescending(q => q.Id)
                .Select(q => q.Id)
                .FirstOrDefault();

            var quizfull = new Quiz()
            {
                Id = maxid + 1,
                Title = quiz.Title,
            };

            this.quizRepo.AddQuiz(quizfull);
            this.Save();

            return quizfull;
        }

        public bool Exist(int id)
        {
            return this.quizRepo.Exist(id);
        }

        public QuizResponseModel GetQuiz(int id)
        {
            if (!Exist(id)) throw new NotFoundException("Not found this quiz");

            var quiz = this.quizRepo.GetQuiz(id);

            QuizResponseModel model = new QuizResponseModel();
            model.Id = quiz.Id;
            model.Title = quiz.Title;
            var questions = this.questionRepo.GetQuestions(quiz.Id);
            var qitems = new List<QuizResponseModel.QuestionItem>();
            
            foreach (var question in questions)
            {
                var qitem = new QuizResponseModel.QuestionItem();
                qitem.Id = question.Id;
                qitem.Text = question.Text;
                qitem.CorrectAnswerId = question.CorrectAnswerId == null ? 0 : (int)question.CorrectAnswerId;

                var answers = this.answerRepo.GetAnswers(question.Id);
                var aitems = new List<QuizResponseModel.AnswerItem>();
                foreach (var ans in answers)
                {
                    aitems.Add(new QuizResponseModel.AnswerItem()
                    { Id = ans.Id, Text = ans.Text });
                }
                qitem.Answers = aitems;
                qitems.Add(qitem);
            }
            model.Questions = qitems;
            var links = new Dictionary<string, string>();
            links["self"] = $"/api/quizzes/{id}";
            links["questions"] = $"/api/quizzes/{id}/questions";
            model.Links = links;

            return model;
        }

        public ICollection<QuizResponseModel> GetQuizzes()
        {
            var quizzes = this.quizRepo.GetQuizzes();
            var response = new List<QuizResponseModel>();
            foreach(var quiz in quizzes)
            {
                QuizResponseModel model = new QuizResponseModel();
                model.Id = quiz.Id;
                model.Title = quiz.Title;
                var questions = this.questionRepo.GetQuestions(quiz.Id);
                var qitems = new List<QuizResponseModel.QuestionItem>();
                foreach(var question in questions)
                {
                    var qitem = new QuizResponseModel.QuestionItem();
                    qitem.Id = question.Id;
                    qitem.Text = question.Text;
                    qitem.CorrectAnswerId = question.CorrectAnswerId == null ? 0 : (int)question.CorrectAnswerId;

                    var answers = this.answerRepo.GetAnswers(question.Id);
                    var aitems = new List<QuizResponseModel.AnswerItem>();
                    foreach(var ans in answers)
                    {
                        aitems.Add(new QuizResponseModel.AnswerItem()
                            { Id = ans.Id, Text = ans.Text });
                    }
                    qitem.Answers = aitems;
                    qitems.Add(qitem);
                }
                model.Questions = qitems;
                var links = new Dictionary<string, string>();
                links["self"] = $"/api/quizzes/{quiz.Id}";
                links["questions"] = $"/api/quizzes/{quiz.Id}/questions";
                model.Links = links;

                response.Add(model);
            }
            return response;
        }

        public Quiz RemoveQuiz(int id)
        {
            if (!Exist(id)) throw new NotFoundException("Not found this quiz");

            var quiz = this.quizRepo.GetQuiz(id);
            this.quizRepo.RemoveQuiz(quiz);
            this.Save();
            return quiz;
        }


        public bool Save()
        {
            int n = this.quizDbContext.SaveChanges();
            return n > 0;
        }

        public Quiz UpdateQuiz(int id, QuizUpdateModel quiz)
        {
            var quizfull = new Quiz()
            {
                Id = id,
                Title = quiz.Title,
            };

            this.quizRepo.UpdateQuiz(quizfull);
            this.Save();

            return quizfull;
        }
    }
}
