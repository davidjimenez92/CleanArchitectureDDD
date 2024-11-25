using CleanArchitectureDDD.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDDD.Infrastructure.Contexts;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
}