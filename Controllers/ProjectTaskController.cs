using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInventorySystem.Data;

namespace SmartInventorySystem.Controllers;

public class ProjectTaskController: Controller
{
    
    public ProjectTaskController(ApplicationDbContext _context)
    {
        _context = context;
    }

    // Index action to show tasks for a specific project
    public IActionResult Index(int projectId)
    {
        var tasks = _context.ProjectTasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Project)
            .ToList();
            
        ViewBag.ProjectId = projectId;
        ViewBag.ProjectName = _context.Projects.Find(projectId)?.Name;
        
        return View(tasks);
    }

    // Details action
    public IActionResult Details(int id)
    {
        var task = _context.ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);
            
        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
    }

    // GET: Create
    public IActionResult Create(int projectId)
    {
        ViewBag.ProjectId = projectId;
        return View();
    }

    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title,Description,ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.ProjectTasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }
        
        ViewBag.ProjectId = task.ProjectId;
        return View(task);
    }

    // GET: Edit
    public IActionResult Edit(int id)
    {
        var task = _context.ProjectTasks.Find(id);
        
        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
    }

    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectTaskId,Title,Description,ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(task);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.ProjectTaskId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }
        return View(task);
    }

    // GET: Delete
    public IActionResult Delete(int id)
    {
        var task = _context.ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);
            
        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
    }

    // POST: Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var task = _context.ProjectTasks.Find(id);
        
        if (task != null)
        {
            int projectId = task.ProjectId;
            _context.ProjectTasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { projectId });
        }
        
        return NotFound();
    }

    public IActionResult Search(int projectId, string searchTerm)
    {
        ViewBag.ProjectId = projectId;
        ViewBag.ProjectName = _context.Projects.Find(projectId)?.Name;
        ViewBag.SearchTerm = searchTerm;
    
        if (string.IsNullOrEmpty(searchTerm))
        {
            return RedirectToAction(nameof(Index), new { projectId });
        }
    
        var tasks = _context.ProjectTasks
            .Where(t => t.ProjectId == projectId && 
                        (t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm)))
            .Include(t => t.Project)
            .ToList();
    
        ViewBag.SearchPerformed = true;
        return View("Index", tasks);
    }
    
    private bool TaskExists(int id)
    {
        return _context.ProjectTasks.Any(e => e.ProjectTaskId == id);
    }
    
}