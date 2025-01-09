using Microsoft.EntityFrameworkCore;

using Quiz.Models;
using Quiz.Data;

namespace Quiz.Services;

public interface ITestService
{
    IEnumerable<Test> GetTests();
    Test GetTest(int testId);
    int GetCountCorrectAnswers(int testId, List<string> answers);
    IEnumerable<TestResult> GetTestResultsForUser(string userId);
    void CreateTest(Test test);
    void UpdateTest(int testId, Test test);
    void DeleteTest(int testId);
    void AddTestResult(int countCorrectAnswers, int testId, string userId);
}

public class TestService(AppDbContext context) : ITestService
{
    private readonly AppDbContext _context = context;

    public IEnumerable<Test> GetTests()
    {
        return [.. _context.Tests.Include(t => t.Questions)];
    }

    public Test GetTest(int testId)
    {
        return _context.Tests.Include(t => t.Questions).FirstOrDefault(t => t.Id == testId) ?? throw new KeyNotFoundException("Test not found");
    }

    public int GetCountCorrectAnswers(int testId, List<string> answers)
    {
        var questions = _context.Questions.Where(q => q.TestId == testId).ToList();

        if (questions.Count == 0)
        {
            throw new KeyNotFoundException("No questions found for the specified test.");
        }

        int countCorrectAnswers = 0;
        for (int i = 0; i < questions.Count && i < answers.Count; ++i)
        {
            if (questions.ElementAt(i).Answer == answers.ElementAt(i))
            {
                ++countCorrectAnswers;
            }
        }

        return countCorrectAnswers;
    }

    public IEnumerable<TestResult> GetTestResultsForUser(string userId)
    {
        return [.. _context.TestResults.Where(tr => tr.UserId == userId)];
    }

    public void CreateTest(Test test)
    {
        _context.Tests.Add(test);
        _context.SaveChanges();
    }

    public void UpdateTest(int testId, Test updatedTest)
    {
        var existingTest = _context.Tests.FirstOrDefault(t => t.Id == testId) ?? throw new KeyNotFoundException("Test not found.");

        existingTest.Title = updatedTest.Title;
        existingTest.Description = updatedTest.Description;
        existingTest.Questions = updatedTest.Questions;

        _context.SaveChanges();
    }

    public void DeleteTest(int testId)
    {
        var test = _context.Tests.FirstOrDefault(t => t.Id == testId) ?? throw new KeyNotFoundException("Test not found.");

        _context.Tests.Remove(test);
        _context.SaveChanges();
    }

    public void AddTestResult(int countCorrectAnswers, int testId, string userId)
    {
        var test = _context.Tests.FirstOrDefault(t => t.Id == testId) ?? throw new KeyNotFoundException("Test not found.");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId) ?? throw new KeyNotFoundException("User not found.");

        var testResult = new TestResult
        {
            TestId = testId,
            UserId = user.Id,
            CorrectAnswersCount = countCorrectAnswers
        };

        _context.TestResults.Add(testResult);
        _context.SaveChanges();
    }
}
