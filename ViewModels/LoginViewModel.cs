using System;
using System.ComponentModel.DataAnnotations;

namespace ModernPortfolio.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage="Kullanıcı adı boş bırakılamaz!")]
    [Display(Name ="Kullanıcı Adı")]
    public string UserName { get; set; } = string.Empty;
    [Required(ErrorMessage="Şifre boş bırakılamaz!")]
    [Display(Name ="Şifre")]
    public string Password { get; set; } = string.Empty;
    [Display(Name ="Beni Hatırla")]
    public bool RememberMe { get; set; }
}
