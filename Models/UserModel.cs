using Microsoft.AspNetCore.Identity;

namespace Quiz.Models;

public class User : IdentityUser
{
    public ICollection<TestResult> TestResults { get; set; } = null!;
}
