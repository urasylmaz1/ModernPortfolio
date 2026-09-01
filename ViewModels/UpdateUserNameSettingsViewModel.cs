using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModernPortfolio.ViewModels;

public class UpdateUserNameSettingsViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
    [Display(Name = "Kullanıcı Adı")]
    [StringLength(50, MinimumLength=3, ErrorMessage="Kullanıcı adının uzunluğu 3-50 karakter uzunluğunda olmalıdır.")]
    public string UserName { get; set; } = string.Empty;
}
