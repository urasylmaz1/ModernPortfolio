using System;
using ModernPortfolio.Models;

namespace ModernPortfolio.Services;

public interface IAboutService
{
    Task<About?> GetAboutAsync();
    Task<int> CreateAboutAsync(About about);
    Task<bool> UpdateAboutAsync(About about);
}
