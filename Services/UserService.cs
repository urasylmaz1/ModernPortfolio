using System;
using ModernPortfolio.Models;
using ModernPortfolio.Repositories;

namespace ModernPortfolio.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("User Id must be greater than zero!", nameof(id));
        }
        return await _repository.GetByIdAsync(id);
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username cannot be null or empty!", nameof(userName));
        }
        return await _repository.GetUserByUserNameAsync(userName);
    }

    public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User Id must be greater than zero!", nameof(userId));
        }
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("New password cannot be null or empty!", nameof(newPassword));
        }
        if (newPassword.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long!", nameof(newPassword));
        }
        var user = await _repository.GetByIdAsync(userId);
        if(user is null)
        {
            throw new InvalidOperationException($"User with Id {userId} not found!");
        }
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        return await _repository.UpdateAsync(user);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        if(user is null)
        {
            throw new ArgumentNullException($"User cannnot be null!");
        }
        if (user.Id <= 0)
        {
            throw new ArgumentException("User Id must be greater than zero!", nameof(user));
        }
        if (string.IsNullOrWhiteSpace(user.UserName))
        {
            throw new ArgumentException("Username cannot be null or empty!", nameof(user));
        }
        user.UpdatedAt = DateTime.UtcNow;
        return await _repository.UpdateAsync(user);

    }

    public async Task<bool> ValidatePasswordAsync(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username cannot be null or empty!", nameof(userName));
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty!", nameof(password));
        }
        var user = await _repository.GetUserByUserNameAsync(userName);
        if(user is null)
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

    }
}
