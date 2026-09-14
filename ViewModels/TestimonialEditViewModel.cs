using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ModernPortfolio.ViewModels;

public class TestimonialEditViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Müşteri adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Müşteri adı en fazla 100 karakter olabilir.")]
    [Display(Name ="Müşteri Adı")]
    public string ClientName { get; set; } = string.Empty;
    [StringLength(100, ErrorMessage = "Müşteri pozisyonu en fazla 100 karakter olabilir.")]
    [Display(Name ="Müşteri Pozisyonu")]
    public string? ClientPosition { get; set; }
    [Required(ErrorMessage = "Yorum zorunludur.")]
    [StringLength(1000, ErrorMessage = "Yorum en fazla 1000 karakter olabilir.")]
    [Display(Name ="Yorum")]
    public string Comment { get; set; } = string.Empty;
    [Required(ErrorMessage = "Değerlendirme zorunludur.")]
    [Range(1,5,ErrorMessage ="Değerlendirme 1-5 arasında olmalıdır.")]
    [Display(Name ="Değerlendirme")]
    public int Rating { get; set; }
    [Display(Name = "Yeni Müşteri Görseli")]
    [DataType(DataType.Upload)]
    public IFormFile? ImageFile { get; set; }
    [Display(Name = "Mevcut Müşteri Görseli")]
    public string? CurrentImageUrl { get; set; }
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}
