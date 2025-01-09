using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Quiz.Services;
using Quiz.Models;
using Quiz.ViewModels;

namespace Quiz.Controllers;

public class TestsController(ITestService testService) : Controller
{
    private readonly ITestService _testService = testService;

    [HttpGet]
    public IActionResult Index()
    {
        var tests = _testService.GetTests();

        return View(tests);
    }

    [HttpGet]
    public IActionResult Start(int id)
    {
        var test = _testService.GetTest(id);

        return View(test);
    }

    [HttpPost]
    public IActionResult SubmitTest(int testId, List<string> selectedAnswers)
    {
        var userId = User.Identity?.IsAuthenticated == true
            ? User.FindFirstValue(ClaimTypes.NameIdentifier)
            : null;

        Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        foreach (var a in selectedAnswers)
        {
            Console.WriteLine(a);
        }

        var test = _testService.GetTest(testId);
        var countCorrectAnswer = _testService.GetCountCorrectAnswers(testId, selectedAnswers);
        var submitTestViewModel = new SubmitTestViewModel {
            CorrectCount = countCorrectAnswer,
            AllCount = test.Questions.Count
        };

        if (userId != null)
        {
            _testService.AddTestResult(countCorrectAnswer, test.Id, userId);
        }

        return View(submitTestViewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(Test test)
    {
        var errors = new List<string>();
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                errors.Add($"Error: {error.ErrorMessage}");
            }
        }
        ViewBag.Errors = errors;

        if (ModelState.IsValid)
        {
            _testService.CreateTest(test);

            return RedirectToAction("Index");
        }

        return View(test);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var test = _testService.GetTest(id);

        return View(test);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int testId, Test test)
    {
        if (ModelState.IsValid)
        {
            _testService.UpdateTest(testId, test);

            return RedirectToAction("Index");
        }

        return View(test);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _testService.DeleteTest(id);

        return RedirectToAction("Index");
    }
}
