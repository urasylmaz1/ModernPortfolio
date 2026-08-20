using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var IsValid = await _userService.ValidatePasswordAsync(loginViewModel.UserName, loginViewModel.Password);
            if (IsValid)
            {
                var user = await _userService.GetUserByUserNameAsync(loginViewModel.UserName);
                if(user is null)
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı!");
                    return View(loginViewModel);
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = loginViewModel.RememberMe
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Dashboard");
            }
            ModelState.AddModelError(string.Empty, "Kullanıcı ya da şifre hatalı!");
            return View(loginViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Since we are in the Admin area, we need to specify the area as empty to redirect to the Home controller in the root area.
            return RedirectToAction("Index","Home", new {area = string.Empty}); 
        }
    }
}
