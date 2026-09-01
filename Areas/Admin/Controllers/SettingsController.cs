using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;
using static Dapper.SqlMapper;

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
    [HttpPost]
    public async Task<IActionResult> UpdateUserName(UpdateUserNameSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        user.UserName = model.UserName;
        try
        {
            await _userService.UpdateUserAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            TempData["SuccessMessage"] = "Kullanıcı adı başarıyla güncellendi.";
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return View("Index",model);
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> UpdateUserPassword(UpdatePasswordSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        if (model.NewPassword != model.ConfirmPassword)
        {
            ModelState.AddModelError("CurrentPassword", "Yeni şifre ile onay şifresi eşleşmiyor.");
            return View("Index", model);
        }
        try
        {
            await _userService.UpdatePasswordAsync(user.Id, model.NewPassword);
            TempData["SuccessMessage"] = "Şifre başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View("Index",model);
        }
        return RedirectToAction("Index");
    }

}
