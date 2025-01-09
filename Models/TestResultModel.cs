namespace Quiz.Models;

public class TestResult
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public Test Test { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public int CorrectAnswersCount { get; set; }
}
