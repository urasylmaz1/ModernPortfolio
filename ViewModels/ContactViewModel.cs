using System;
using System.ComponentModel.DataAnnotations;

namespace ModernPortfolio.ViewModels;

public class ContactViewModel
{
    [Required(ErrorMessage ="Ad soyad gereklidir!")]
    [Display(Name= "Ad Soyad")]
    [StringLength(100, ErrorMessage ="Ad soyad en fazla 100 karakter olabilir!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage ="Email adresi gereklidir!")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz!")]
    [Display(Name= "Email")]
    public string Email { get; set; }= string.Empty;

    [Required(ErrorMessage ="Başlık gereklidir!")]
    [Display(Name= "Başlık")]
    [StringLength(200, ErrorMessage ="Başlık en fazla 200 karakter olabilir!")]
    public string? Subject { get; set; }

    [Required(ErrorMessage ="Mesaj gereklidir!")]
    [Display(Name= "Mesajınız")]
    [StringLength(2000, ErrorMessage ="Mesaj en fazla 2000 karakter olabilir!")]
    public string Message { get; set; }= string.Empty;
}
