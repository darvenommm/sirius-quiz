namespace Quiz.Models;

public class Test
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ICollection<Question> Questions { get; set; } = null!;
}
