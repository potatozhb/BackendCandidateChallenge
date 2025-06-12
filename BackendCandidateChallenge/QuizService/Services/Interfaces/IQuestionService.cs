using QuizService.Model;
using QuizService.Model.Domain;
using System.Collections.Generic;

namespace QuizService.Services.Interfaces
{
    public interface IQuestionService
    {
        ICollection<Question> GetQuestions(int quizId);
        Question GetQuestion(int id);

        Question AddQuestion(int quizId, QuestionCreateModel question);
        Question RemoveQuestion(int quizId, int id);
        Question UpdateQuestion(int quizId, int questionId, QuestionUpdateModel question);

        bool Exist(int id);
        bool Save();
    }
}
