using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Quiz.Models;

public class Question
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Answer { get; set; }
    public int TestId { get; set; }

    [ValidateNever]
    public Test Test { get; set; } = null!;
}
