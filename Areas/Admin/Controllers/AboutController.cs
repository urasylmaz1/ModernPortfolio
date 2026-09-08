using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Areas.Admin.Controllers;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModernPortfolio.Areas.Admin.Controllers;

public class AboutController : BaseAdminController
{
    private readonly IAboutService _aboutService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IImageService _imageService;
    public AboutController(IAboutService aboutService, IWebHostEnvironment webHostEnvironment, IImageService imageService)
    {
        _aboutService = aboutService;
        _webHostEnvironment = webHostEnvironment;
        _imageService = imageService;
    }

    public async Task<IActionResult> Index()
    {
        var about = await _aboutService.GetAboutAsync();
        if (about is null)
        {
            return View((AboutEditViewModel?) null);
        }
        var model = new AboutEditViewModel
        {
            Id = about.Id,
            Title = about.Title,
            Age = about.Age,
            City = about.City,
            Description = about.Description,
            Email = about.Email,
            GithubUrl = about.GithubUrl,
            LinkedinUrl = about.LinkedinUrl,
            PhoneNumber = about.PhoneNumber,
            CurrentImageUrl = "/" + about.ImageUrl
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Create(AboutCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var about = new About
        {
            Title = model.Title,
            Description = model.Description,
            Age = model.Age,
            City = model.City,
            GithubUrl = model.GithubUrl,
            Email = model.Email,
            LinkedinUrl = model.LinkedinUrl,
            PhoneNumber = model.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };
        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            try
            {
                var imageUrl = await _imageService.SaveImageAsync(model.ImageFile,"about");
                about.ImageUrl = imageUrl;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ImageFile", e.Message);
                return View(model);
            }
        }
        try
        {
            var aboutId = await _aboutService.CreateAboutAsync(about);
            if (aboutId > 0)
            {
                TempData["SuccessMessage"] = "Hakkında başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Hata: Hakkında eklenirken bir sorun oluştu!";
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AboutEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var about = await _aboutService.GetAboutAsync();
        if (about is null || about.Id != model.Id)
        {
            TempData["ErrorMessage"] = "Hakkında bilgisi bulunamadı.";
            return RedirectToAction(nameof(Index));
        }
        about.Title = model.Title;
        about.Description = model.Description;
        about.Age = model.Age;
        about.City = model.City;
        about.Email = model.Email;
        about.GithubUrl = model.GithubUrl;
        about.LinkedinUrl = model.LinkedinUrl;
        about.PhoneNumber = model.PhoneNumber;
        about.UpdatedAt = DateTime.UtcNow;

        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(about.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(about.ImageUrl);
                }
                var imageUrl = await _imageService.SaveImageAsync(model.ImageFile,"about");
                about.ImageUrl = imageUrl;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View(model);
            }
        }

        var result = await _aboutService.UpdateAboutAsync(about);
        if (result)
        {
            TempData["SuccessMessage"] = "Hakkında bilgisi başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = "Hakkında bilgisi güncellenirken bir sorun oluştu.";
        return View(model);
    }
}






