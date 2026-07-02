using System;
using ModernPortfolio.Models;
using ModernPortfolio.Repositories;

namespace ModernPortfolio.Services;

public class AboutService : IAboutService
{
    private readonly IAboutRepository _repository;

    public AboutService(IAboutRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<int> CreateAboutAsync(About about)
    {
        if(about is null)
        {
            throw new ArgumentNullException("About cannot be null!",nameof(about));
        }
        ValidateAbout(about);
        var existingAbout = await GetAboutAsync();
        if(existingAbout is not null)
        {
            throw new InvalidOperationException("About record already exists. Use UpdateAboutAsync to update existing record!");
        }
        about.CreatedAt = DateTime.UtcNow;
        var result = await _repository.CreateAsync(about);
        return result;
    }

    public async Task<About?> GetAboutAsync()
    {
        var about = await _repository.GetAllAsync();
        var result = about.FirstOrDefault();
        return result;
    }

    public async Task<bool> UpdateAboutAsync(About about)
    {
        if(about is null)
        {
            throw new ArgumentNullException("About cannot be null!",nameof(about));
        }
        if(about.Id <= 0)
        {
            throw new ArgumentException("About ID must be greater than zero!", nameof(about));
        }
        var existingAbout = await GetAboutAsync();
        if(existingAbout is null)
        {
            throw new InvalidOperationException($"About with id {about.Id} not found!");
        }
        ValidateAbout(about);
        about.CreatedAt = existingAbout.CreatedAt;
        about.UpdatedAt = DateTime.UtcNow;
        var result = await _repository.UpdateAsync(about);
        return result;
    }

    //Validation
    private void ValidateAbout(About about)
    {
        if (string.IsNullOrWhiteSpace(about.Title))
        {
            throw new ArgumentException("About title cannot be empty or whitespace!", nameof(about));
        }
        if (about.Title.Length > 100)
        {
            throw new ArgumentException("About title cannot exceed 100 characters!", nameof(about));
        }
        if (string.IsNullOrWhiteSpace(about.Description))
        {
            throw new ArgumentException("About description cannot be empty or whitespace!", nameof(about));
        }
        
    }
}
