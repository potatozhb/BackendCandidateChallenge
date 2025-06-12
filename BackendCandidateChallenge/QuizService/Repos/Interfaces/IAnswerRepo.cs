using QuizService.Model.Domain;
using System.Collections.Generic;

namespace QuizService.Repos.Interfaces
{
    public interface IAnswerRepo
    {
        ICollection<Answer> GetAnswers();
        ICollection<Answer> GetAnswers(int questionId);
        Answer GetAnswer(int id);

        void AddAnswer(Answer answer);
        void RemoveAnswer(Answer answer);
        void UpdateAnswer(Answer answer);

        bool Exist(int id);
    }
}
