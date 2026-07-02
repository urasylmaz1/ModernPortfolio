using System;
using ModernPortfolio.Models;
using ModernPortfolio.Repositories;

namespace ModernPortfolio.Services;

public class SkillService : ISkillService
{
     private readonly ISkillRepository _repository;

    public SkillService(ISkillRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    public async Task<int> CreateSkillAsync(Skill skill)
    {
        if(skill is null)
        {
            throw new ArgumentNullException("Skill cannot be null!",nameof(skill));
        }
        ValidateSkill(skill);
        skill.CreatedAt= DateTime.UtcNow;

        //Controlling Display Order
        if (skill.DisplayOrder == 0)
        {
            var allSkills = await _repository.GetAllAsync();
            skill.DisplayOrder=allSkills.Any() ? allSkills.Max(s=>s.DisplayOrder) + 1: 1;
        }
        var result = await _repository.CreateAsync(skill);
        return result;
    }

    public Task<bool> DeleteSkillAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
    {
        var skills = await _repository.GetAllAsync();
        var result = skills.OrderBy(s=>s.DisplayOrder).ThenByDescending(s=>s.CreatedAt);
        return result;
    }

    public async Task<Skill?> GetSkillByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("Id must be greater than zero!", nameof(id));
        }
        var result = await _repository.GetByIdAsync(id);
        return result;
    }

    public async Task<bool> UpdateSkillAsync(Skill skill)
    {
        if(skill is null)
        {
            throw new ArgumentNullException("Skill cannot be null!",nameof(skill));
        }
        if(skill.Id <= 0)
        {
            throw new ArgumentException("Skill ID must be greater than zero!", nameof(skill));
        }
        var existingSkill = await _repository.GetByIdAsync(skill.Id);

        if(existingSkill is null)
        {
            throw new ArgumentNullException($"Skill with id {skill.Id} not found!",nameof(skill));
        }
        ValidateSkill(skill);
        skill.CreatedAt = existingSkill.CreatedAt;
        var result = await _repository.UpdateAsync(skill);
        return result;
    }

    //Validation
    private void ValidateSkill(Skill skill)
    {
        if (string.IsNullOrWhiteSpace(skill.Name))
        {
            throw new ArgumentException("Skill name cannot be empty or whitespace!", nameof(skill));
        }
        if (skill.Name.Length > 100)
        {
            throw new ArgumentException("Skill name cannot exceed 100 characters!", nameof(skill));
        }
        if (skill.Percentage<0 || skill.Percentage > 100)
        {
            throw new ArgumentException("Skill percentage must be between 0 and 100!",nameof(skill));
        }
        if (skill.DisplayOrder < 0)
        {
            throw new ArgumentException("Skill display order cannot be negative!", nameof(skill));
        }
    }
}
