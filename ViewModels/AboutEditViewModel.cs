using System.ComponentModel.DataAnnotations;
namespace ModernPortfolio.ViewModels;

public class AboutEditViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    [Display(Name ="Başlık")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Açıklama zorunludur.")]
    [StringLength(5000, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
    [Display(Name ="Açıklama")]
    public string Description { get; set; } = string.Empty;
    [Display(Name = "Mevcut Görsel")]
    public string? CurrentImageUrl { get; set; }
    [Display(Name = "Yeni Görsel")]
    [DataType(DataType.Upload)]
    public IFormFile? ImageFile { get; set; }
    [Required(ErrorMessage = "Email zorunludur.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Display(Name = "Github URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz!")]
    public string? GithubUrl { get; set; }
    [Display(Name = "LinkedIn URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz!")]
    public string? LinkedinUrl { get; set; }
    [Display(Name = "Telefon numarası")]
    public string? PhoneNumber { get; set; }
    [Display(Name = "Şehir")]
    public string? City { get; set; }
    [Display(Name = "Yaş")]
    public int Age { get; set; }
}
