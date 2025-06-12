using QuizService.Model.Domain;
using QuizService.Model;
using System.Collections.Generic;

namespace QuizService.Services.Interfaces
{
    public interface IAnswerService
    {
        ICollection<Answer> GetAnswers(int questionId);
        Answer GetAnswer(int id);

        Answer AddAnswer(int questionId, AnswerCreateModel answer);
        Answer RemoveAnswer(int questionId, int id);
        Answer UpdateAnswer(int quizId, int questionId, int id, AnswerUpdateModel answer);

        bool Exist(int id);
        bool Save();
    }
}
