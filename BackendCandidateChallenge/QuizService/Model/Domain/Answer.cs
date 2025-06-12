using System.ComponentModel.DataAnnotations;

namespace QuizService.Model.Domain;

public class Answer
{
    public int Id { get; set; }

    [Required]
    public int QuestionId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Text { get ; set; }
}