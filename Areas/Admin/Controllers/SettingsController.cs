using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Areas.Admin.Controllers;

public class SettingsController : BaseAdminController
{
    private readonly IUserService _userService;

    public SettingsController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        var settingsViewModel = new SettingsViewModel
        {
            UserName = user.UserName,
            CurrentPassword = string.Empty,
            NewPassword = string.Empty,
            ConfirmPassword = string.Empty
        };
        return View(settingsViewModel);
    }
}
