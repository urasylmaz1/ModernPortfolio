using System;
using ModernPortfolio.Models;

namespace ModernPortfolio.Repositories;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
}
