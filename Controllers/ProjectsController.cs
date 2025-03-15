using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_ICE.Controllers;
public class ProjectsController : Controller
{ ///<summary>
    /// Index action will retrieve a listing of projects (database)
    ///</summary>
    ///<return></return>
    public IActionResult Index()
    {
        var projects = new List<Project>
        {
            //
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project 1" }
            //feel free to define more projects
        };
        return View(projects);
    }
    [HttpGet]
    public IActionResult Create()
    {
        
        return View();
    }
    [HttpPost]
    public IActionResult Create(Project project)
    {
        //Persist new Project to the database
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Details(int id)
    {
        var project = new Project { ProjectId = id, Name = "Project Details", Description = "First Project Details" };
        return View(project);
    }

    [HttpGet, ActionName("Edit")]
    public IActionResult Edit(int id)
    {
        var project = new Project { ProjectId = id, Name = "Project Edit", Description = "First Project Edit" };
        return View(project);
    }
    
    //CRUD Create - Read - Update - Delete

    /*
    [HttpGet, ActionName("Delete")]
    public IActionResult Delete(int ProjectID)
    {
        if (Project != null)
        {
            
            <tr>
                <th>@Project.ProjectId</th>
                <th>@Project.Name</th>
                <th>@Project.Description</th>
                <th></th>
                </tr>
                
            return RedirectToAction("Index");
        }
    }
    */
}