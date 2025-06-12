using QuizService.Model.Domain;
using System.Collections.Generic;

namespace QuizService.Repos.Interfaces
{
    public interface IQuizRepo
    {
        ICollection<Quiz> GetQuizzes();
        Quiz GetQuiz(int id);

        void AddQuiz(Quiz quiz);
        void RemoveQuiz(Quiz quiz);
        void UpdateQuiz(Quiz quiz);

        bool Exist(int id);
    }
}
