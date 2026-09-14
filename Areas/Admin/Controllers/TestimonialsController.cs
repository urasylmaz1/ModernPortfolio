using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Areas.Admin.Controllers;

public class TestimonialsController : BaseAdminController
{
    private readonly ITestimonialService _testimonialService;
    private readonly IImageService _imageService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TestimonialsController(ITestimonialService testimonialService, IImageService imageService, IWebHostEnvironment webHostEnvironment)
    {
        _testimonialService = testimonialService;
        _imageService = imageService;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: TestimonialsController
    public async Task<ActionResult> Index()
    {
        var testimonials = await _testimonialService.GetAllTestimonialsAsync();
        var models = testimonials.Select(t => new TestimonialListViewModel
        {
            Id = t.Id,
            ClientName = t.ClientName,
            ClientPosition = t.ClientPosition,
            Rating = t.Rating,
            ClientImageUrl = "/"+ t.ClientImageUrl,
            Status = t.IsActive ? "Aktif" : "Pasif",
            CreatedDate = t.CreatedAt.ToShortDateString()
        }).ToList();
        return View(models);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View(new TestimonialCreateViewModel());
    }
    [HttpPost]
    public async Task<IActionResult> Create(TestimonialCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var testimonial = new Testimonial
        {
            ClientName = model.ClientName,
            Comment = model.Comment,
            Rating = model.Rating,
            ClientPosition = model.ClientPosition!,
            IsActive = model.IsActive,
            CreatedAt = DateTime.UtcNow
        };
        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            try
            {
                var imageUrl = await _imageService.SaveImageAsync(model.ImageFile, "testimonials");
                testimonial.ClientImageUrl = imageUrl;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ImageFile", e.Message);
                return View(model);
            }
        }
        try
        {
            var testimonialId = await _testimonialService.CreateTestimonialAsync(testimonial);
            if (testimonialId > 0)
            {
                TempData["SuccessMessage"] = "Yorum başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Hata: Yorum eklenirken bir sorun oluştu!";
        }
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var testimonial = await _testimonialService.GetTestimonialByIdAsync(id);
        if (testimonial is null)
        {
            TempData["ErrorMessage"] = "Yorum bulunamadı!";
            return RedirectToAction(nameof(Index));
        }
        var model = new TestimonialEditViewModel()
        {
            Id = testimonial.Id,
            ClientName = testimonial.ClientName,
            ClientPosition = testimonial.ClientPosition,
            Comment = testimonial.Comment,
            Rating = testimonial.Rating,
            CurrentImageUrl = "/"+ testimonial.ClientImageUrl,
            IsActive = testimonial.IsActive
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(TestimonialEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var testimonial = await _testimonialService.GetTestimonialByIdAsync(model.Id);
        if (testimonial is null)
        {
            TempData["ErrorMessage"] = "Yorum bulunamadı!";
            return RedirectToAction(nameof(Index));
        }
        testimonial.ClientName = model.ClientName;
        testimonial.ClientPosition = model.ClientPosition!;
        testimonial.Comment = model.Comment;
        testimonial.Rating = model.Rating;
        testimonial.IsActive = model.IsActive;
        if (model.ImageFile is not null && model.ImageFile.Length > 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(testimonial.ClientImageUrl))
                {
                    await _imageService.DeleteImageAsync(testimonial.ClientImageUrl);
                }
                var imageUrl = await _imageService.SaveImageAsync(model.ImageFile);
                testimonial.ClientImageUrl = imageUrl;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ImageFile", e.Message);
                return View(model);
            }
        }
        try
        {
            var result = await _testimonialService.UpdateTestimonialAsync(testimonial);
            if (result)
            {
                TempData["SuccessMessage"] = "Yorum başarıyla düzenlendi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Yorum güncellenirken bir sorun oluştu!";
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
            var result = await _testimonialService.DeleteTestimonialAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Yorum başarıyla silindi";
            }
            else
            {
                TempData["ErrorMessage"] = "Yorum silinirken hata oluştu!";
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Hata: {e.Message}";
        }
        return RedirectToAction(nameof(Index));
    }
}