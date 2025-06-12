using System.ComponentModel.DataAnnotations;

namespace QuizService.Model.Domain;

public class Question
{
    public int Id { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Text { get ; set; }

    public int? CorrectAnswerId { get; set; }
}