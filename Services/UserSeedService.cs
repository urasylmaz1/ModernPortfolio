using System;
using ModernPortfolio.Models;
using ModernPortfolio.Repositories;

namespace ModernPortfolio.Services;

public class UserSeedService : IUserSeedService
{
    private readonly IUserRepository _userRepository;

    public UserSeedService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task SeedDeafaultUserAsync()
    {
        var allUsers = await _userRepository.GetAllAsync();
        if (!allUsers.Any())
        {
            var defaultUser = new User
            {
                UserName = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123.,"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
            await _userRepository.CreateAsync(defaultUser);
        }
    }
}
