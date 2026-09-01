using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ModernPortfolio.ViewModels;

public class ProjectCreateViewModel
{
    [Required(ErrorMessage = "Başlık zorunludur!")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık 5-200 karakter arasında olmalıdır!")]
    [Display(Name ="Başlık")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Açıklama zorunludur!")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Açıklama 10-1000 karakter arasında olmalıdır!")]
    [Display(Name ="Açıklama")]
    public string Description { get; set; }= string.Empty;
    [Display(Name = "Proje URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz!")]
    public string? ProjectUrl { get; set; }
    [Display(Name = "Github URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz!")]
    public string? GithubUrl { get; set; }
    [Display(Name = "Görsel")]
    [DataType(DataType.Upload)]
    public IFormFile? ImageFile { get; set; }
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}
