using Microsoft.EntityFrameworkCore;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Infrastructure.Persistence;

/// <summary>
///     Entity Framework Core database context for managing product entities.
/// </summary>
internal class DatabaseContext : DbContext
{
    /// <summary>
    ///     Creates a new instance of the <see cref="DatabaseContext" /> class with the specified options.
    /// </summary>
    /// <param name="options">The options.</param>
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="DatabaseContext" /> class.
    /// </summary>
    public DatabaseContext()
    {
    }

    /// <summary>
    ///     Gets or sets the products in the database.
    /// </summary>
    public DbSet<Product> Products { get; set; }
}