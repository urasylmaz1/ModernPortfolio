using System.ComponentModel.DataAnnotations;
namespace ModernPortfolio.ViewModels;

public class SkillEditViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Yetenek adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Yetenek adı en fazla 100 karakter olabilir.")]
    [Display(Name ="Yetenek Adı")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Yüzdelik zorunludur.")]
    [Range(0,100,ErrorMessage ="Yüzde değeri 0-100 arasında olmalıdır.")]
    public int? Percentage { get; set; }
    [Display(Name ="Görüntüleme Sırası")]
    public int DisplayOrder { get; set; }
}
