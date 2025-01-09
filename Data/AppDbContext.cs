using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Quiz.Models;

namespace Quiz.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<TestResult> TestResults { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TestResult>()
            .HasOne(tr => tr.User)
            .WithMany(u => u.TestResults)
            .HasForeignKey(tr => tr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
