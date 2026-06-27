using System;
using ModernPortfolio.Models;

namespace ModernPortfolio.Repositories;

public class AboutRepository : GenericRepository<About>, IAboutRepository
{
    public AboutRepository(IConfiguration configuration) : base(configuration)
    {
    }
}
