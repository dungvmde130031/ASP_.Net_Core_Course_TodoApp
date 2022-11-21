using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Business;
using TodoApp.Business.Model;
using TodoApp.Business.Models;
using TodoApp.WebMvc.Models;

namespace TodoApp.WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ITaskBusiness _taskBusiness;

    public HomeController(ILogger<HomeController> logger, ITaskBusiness taskBusiness)
    {
        _logger = logger;
        _taskBusiness = taskBusiness;
    }

    // Render list of tasks
    public IActionResult Index()
    {
        var tasks = _taskBusiness.GetTasks();

        return View(tasks);
    }

    // Search
    [HttpPost]
    public IActionResult TaskFilter(TaskSearchModel model)
    {
        var taskFilter = _taskBusiness.TaskFilter(model);

        return View("Index", taskFilter);
    }

    // Add task
    [HttpGet]
    public IActionResult AddTask()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddTask(TaskModel taskModel)
    {
        _taskBusiness.AddTask(taskModel);

        return RedirectToAction("Index");
    }

    // Update task
    [HttpGet]
    public IActionResult UpdateTask(Guid id)
    {
        var getCurrentTaskId = _taskBusiness.GetTask(id);

        return View(getCurrentTaskId);
    }

    [HttpPost]
    public IActionResult UpdateTask(TaskModel taskModel)
    {
        _taskBusiness.UpdateTask(taskModel);

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult DeleteTask(Guid id)
    {
        _taskBusiness.DeleteTask(id);

        return RedirectToAction("Index");
    }

    
    public IActionResult CompleteTask(Guid id)
    {
        _taskBusiness.CompleteTask(id);

        return RedirectToAction("Index");
    }

    
    public IActionResult ReopenTask(Guid id)
    {
        _taskBusiness.ReopenTask(id);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

