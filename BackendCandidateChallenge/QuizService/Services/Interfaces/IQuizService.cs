using QuizService.Model;
using QuizService.Model.Domain;
using System.Collections.Generic;

namespace QuizService.Services.Interfaces
{
    public interface IQuizService
    {
        ICollection<QuizResponseModel> GetQuizzes();
        QuizResponseModel GetQuiz(int id);

        Quiz AddQuiz(QuizCreateModel quiz);
        Quiz RemoveQuiz(int id);
        Quiz UpdateQuiz(int id, QuizUpdateModel quiz);

        bool Exist(int id);
        bool Save();
    }
}
