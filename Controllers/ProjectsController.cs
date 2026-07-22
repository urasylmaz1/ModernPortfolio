using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<ActionResult> Index()
        {
            var projects = await _projectService.GetActiveProjectsAsync();
            var model = projects.Select(p=> new ProjectViewModel
            {
                Id = p.Id,
                Title= p.Title,
                Description= p.Description,
                ImageUrl=p.ImageUrl,
                ProjectUrl=p.ProjectUrl,
                GithubUrl=p.GithubUrl,
                CreatedAt=p.CreatedAt,
                IsActive=p.IsActive
            }).ToList();
            return View(model);
        }

    }
}
