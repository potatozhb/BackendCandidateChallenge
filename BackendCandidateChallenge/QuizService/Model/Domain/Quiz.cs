using System.ComponentModel.DataAnnotations;

namespace QuizService.Model.Domain;

public class Quiz
{
    //best use UUID
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get ; set; }
}