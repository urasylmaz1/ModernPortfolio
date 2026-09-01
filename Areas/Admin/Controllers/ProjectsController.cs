using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Areas.Admin.Controllers;

public class ProjectsController : BaseAdminController
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        var model = projects.Select(p => new ProjectListViewModel
        {
            Id = p.Id,
            Title = p.Title,
            ImageUrl = "/" + p.ImageUrl,
            Status = p.IsActive ? "Aktif" : "Pasif",
            CreatedDate = p.CreatedAt.ToShortDateString()
        }).ToList();
        return View(model);
    }
    public IActionResult Create()
    {
        return View(new ProjectCreateViewModel());
    }
}
