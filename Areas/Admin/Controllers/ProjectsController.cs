using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModernPortfolio.Areas.Admin.Controllers;

public class ProjectsController : BaseAdminController
{
    private readonly IProjectService _projectService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectsController(IProjectService projectService, IWebHostEnvironment webHostEnvironment)
    {
        _projectService = projectService;
        _webHostEnvironment = webHostEnvironment;
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
                var imageUrl = await SaveImageAsync(model.ImageFile);
                project.ImageUrl=imageUrl;
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
    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (!allowedExtentions.Contains(fileExtension))
        {
            throw new ArgumentException("Geçersiz dosya formatı! Sadece PNG, JPG, ve GIF formatları desteklenir.");
        }
        if (imageFile.Length > 5 * 1024 * 1024)
        {
            throw new ArgumentException("Dosya boyutu 5 MB'tan büyük olamaz.");
        }
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ui", "img", "portfolio");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        var filePath = Path.Combine(uploadsFolder, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        var imageUrl = $"ui/img/portfolio/{fileName}";
        return imageUrl;
    }
}





