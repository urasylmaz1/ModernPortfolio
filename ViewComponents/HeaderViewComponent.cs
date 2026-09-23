using ModernPortfolio.Services;
using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.ViewModels;
namespace ModernPortfolio.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly IAboutService _aboutService;

    public HeaderViewComponent(IAboutService aboutService)
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