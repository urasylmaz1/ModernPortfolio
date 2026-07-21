using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Controllers;

public class HomeController : Controller
{
    private readonly IAboutService _aboutService;
    private readonly ITestimonialService _testimonailService;

    public HomeController(IAboutService aboutService, ITestimonialService testimonailService)
    {
        _aboutService = aboutService;
        _testimonailService = testimonailService;
    }

    public async Task<ActionResult> Index()
    {
        var about = await _aboutService.GetAboutAsync();
        var testimonails = await _testimonailService.GetActiveTestimonialsAsync();
        var homeViewModel = new HomeViewModel
        {
            About = about,
            Testimonials = testimonails
        };
        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
