using System;
using ModernPortfolio.Models;

namespace ModernPortfolio.Repositories;

public interface IContactRepository : IGenericRepository<Contact>
{
    Task<IEnumerable<Contact>> GetUnreadMessagesAsync();
}
