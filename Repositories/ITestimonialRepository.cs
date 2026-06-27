using System;
using ModernPortfolio.Models;

namespace ModernPortfolio.Repositories;

public interface ITestimonialRepository: IGenericRepository<Testimonial>
{
    Task<IEnumerable<Testimonial>> GetActiveTestimonalsAsync();
}
