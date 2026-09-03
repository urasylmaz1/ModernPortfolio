using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModernPortfolio.Areas.Admin.Controllers;

public class ProjectsController : BaseAdminController
{
    private readonly IProjectService _projectService;
    private readonly IImageService _imageService;

    public ProjectsController(IProjectService projectService, IImageService imageService)
    {
        _projectService = projectService;
        _imageService = imageService;

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
    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProjectCreateViewModel());
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProjectCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var project = new Project
        {
            Title = model.Title,
            Description = model.Description,
            GithubUrl = model.GithubUrl,
            ProjectUrl = model.ProjectUrl,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            try
            {
                var imageUrl = await _imageService.SaveImageAsync(model.ImageFile);
                project.ImageUrl = imageUrl;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ImageFile", e.Message);
                return View(model);
            }
        }
        try
        {
            var projectId = await _projectService.CreateProjectAsync(project);
            if (projectId > 0)
            {
                TempData["SuccessMessage"] = "Proje başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Hata: Proje eklenirken bir sorun oluştu!";
        }
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project is null)
        {
            TempData["ErrorMessage"] = "Proje bulunamadı!";
            return RedirectToAction(nameof(Index));
        }
        var model = new ProjectEditViewModel()
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ProjectUrl = project.ProjectUrl,
            GithubUrl = project.GithubUrl,
            CurrentImageUrl = "/"+ project.ImageUrl,
            IsActive = project.IsActive
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ProjectEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var project = await _projectService.GetProjectByIdAsync(model.Id);
        if (project is null)
        {
            TempData["ErrorMessage"] = "Proje bulunamadı!";
            return RedirectToAction(nameof(Index));
        }
        project.Title = model.Title;
        project.Description = model.Title;
        project.ProjectUrl = model.ProjectUrl;
        project.GithubUrl = model.GithubUrl;
        project.IsActive = model.IsActive;
        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(project.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(project.ImageUrl);
                }
                var imageUrl = await _imageService.SaveImageAsync(model.ImageFile);
                project.ImageUrl = imageUrl;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ImageFile", e.Message);
                return View(model);
            }
        }
        try
        {
            var result = await _projectService.UpdateProjectAsync(project);
            if (result)
            {
                TempData["SuccessMessage"] = "Proje başarıyla düzenlendi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Proje güncellenirken bir sorun oluştu!";
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Hata: {e.Message}";
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _projectService.DeleteProjectAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Proje başarıyla silindi";
            }
            else
            {
                TempData["ErrorMessage"] = "Proje silinirken hata oluştu!";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Hata: {e.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
}





