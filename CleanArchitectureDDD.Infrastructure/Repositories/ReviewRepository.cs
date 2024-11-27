using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Reviews;
using CleanArchitectureDDD.Infrastructure.Contexts;

namespace CleanArchitectureDDD.Infrastructure.Repositories;

internal sealed class ReviewRepository: Repository<Review, ReviewId>
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}