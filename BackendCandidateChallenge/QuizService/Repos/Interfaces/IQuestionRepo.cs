using QuizService.Model.Domain;
using System.Collections.Generic;

namespace QuizService.Repos.Interfaces
{
    public interface IQuestionRepo
    {
        ICollection<Question> GetQuestions();
        ICollection<Question> GetQuestions(int quizId);
        Question GetQuestion(int id);

        void AddQuestion(Question question);
        void RemoveQuestion(Question question);
        void UpdateQuestion(Question question);

        bool Exist(int id);
    }
}
