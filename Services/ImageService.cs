namespace ModernPortfolio.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }


    public async Task DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return;
        }
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl);
        if (System.IO.File.Exists(filePath))
        {
            await Task.Run(() => System.IO.File.Delete(filePath));
        }
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        var allowedExtentions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (!allowedExtentions.Contains(fileExtension))
        {
            throw new ArgumentException("Geçersiz dosya formatı! Sadece PNG, JPG, ve GIF formatları desteklenir.");
        }
        if (imageFile.Length > 5 * 1024 * 1024)
        {
            throw new ArgumentException("Dosya boyutu 5 MB'tan büyük olamaz.");
        }
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ui", "img", "portfolio");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        var filePath = Path.Combine(uploadsFolder, fileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        var imageUrl = $"ui/img/portfolio/{fileName}";
        return imageUrl;
    }

}
