using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModernPortfolio.ViewModels;

public class SettingsViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
    [Display(Name = "Kullanıcı Adı")]
    [StringLength(50, MinimumLength=3, ErrorMessage="Kullanıcı adının uzunluğu 3-50 karakter uzunluğunda olmalıdır.")]
    public string UserName { get; set; } = string.Empty;
    [Display(Name = "Mevcut Şifre")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;
    [Display(Name = "Yeni Şifre")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength=6, ErrorMessage="Şifre en az 6 karakter uzunluğunda olmalıdır.")]
    public string NewPassword { get; set; } = string.Empty;
    [Display(Name = "Yeni Şifre Tekrar")]
    [DataType(DataType.Password)]
    [Compare("NewPassword",ErrorMessage ="Şifreler eşleşmiyor!")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
