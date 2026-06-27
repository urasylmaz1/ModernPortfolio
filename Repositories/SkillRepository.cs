using System;
using ModernPortfolio.Models;

namespace ModernPortfolio.Repositories;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(IConfiguration configuration) : base(configuration)
    {
    }
}
