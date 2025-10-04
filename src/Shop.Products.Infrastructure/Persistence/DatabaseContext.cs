using Microsoft.EntityFrameworkCore;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) 
        : base(options) { }

    public DatabaseContext() { }
}