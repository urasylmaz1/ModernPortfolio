using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly IAboutService _aboutService;

    public FooterViewComponent(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var about = await _aboutService.GetAboutAsync();
        var model = new AboutViewModel
        {
            Title = about!.Title,
            City = about.City,
            GithubUrl = about.GithubUrl,
            LinkedinUrl = about.LinkedinUrl,
            PhoneNumber = about.PhoneNumber,
            Email = about.Email,
        };
        return View(model);
    }
}
