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
    private readonly ISkillService _skillService;
    private readonly IContactService _contactService;

    public HomeController(IAboutService aboutService, ITestimonialService testimonailService, ISkillService skillService, IContactService contactService)
    {
        _aboutService = aboutService;
        _testimonailService = testimonailService;
        _skillService = skillService;
        _contactService = contactService;
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

    public async Task<IActionResult> About()
    {
        var about = await _aboutService.GetAboutAsync();
        var skills = await _skillService.GetAllSkillsAsync();
        if (about is null)
        {
            return View(new AboutViewModel());
        }
        var aboutViewModel = new AboutViewModel
        {
            Title= about.Title,
            Age= about.Age,
            City=about.City,
            Description=about.Description,
            Email= about.Email,
            GithubUrl= about.GithubUrl,
            ImageUrl= "/" + about.ImageUrl,
            LinkedinUrl=about.LinkedinUrl,
            PhoneNumber=about.PhoneNumber
        };
        var skillsViewModel = skills.Select(s=> new SkillViewModel
        {
            Name= s.Name,
            DisplayOrder= s.DisplayOrder,
            Percentage=s.Percentage
        }).ToList();

        var model = new AboutSkillsViewModel
        {
            About = aboutViewModel,
            Skills = skillsViewModel
        };
        return View(model);
    }
    public IActionResult Contact()
    {
        return View(new ContactViewModel { });
    }
    [HttpPost]
    public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(contactViewModel);
        }
        var contact = new Contact
        {
            Name= contactViewModel.Name!,
            Email= contactViewModel.Email!,
            Subject= contactViewModel.Subject,
            Message= contactViewModel.Message!,
        };
        await _contactService.CreateContactAsync(contact);
        TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. En kısa sürede dönüş yapacağız.";
        return RedirectToAction(nameof(Contact));
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
