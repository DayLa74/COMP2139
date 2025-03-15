using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Models;

namespace COMP2139_ICE.Controllers;

public class HomeController : Controller
{
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult Search(string searchType, string searchTerm)
    {
        ViewBag.SearchTerm = searchTerm;
        ViewBag.SearchType = searchType;
    
        if (string.IsNullOrEmpty(searchTerm))
        {
            return View(new SearchViewModel());
        }
    
        var viewModel = new SearchViewModel();
    
        if (searchType == "Project" || string.IsNullOrEmpty(searchType))
        {
            viewModel.Projects = _context.Projects
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToList();
        }
    
        if (searchType == "Task" || string.IsNullOrEmpty(searchType))
        {
            viewModel.Tasks = _context.ProjectTasks
                .Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))
                .Include(t => t.Project)
                .ToList();
        }
    
        return View(viewModel);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}