namespace ModernPortfolio.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile imageFile, string folderName="folderName");
    Task DeleteImageAsync(string imageUrl);
}
