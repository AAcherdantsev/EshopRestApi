using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Products.Domain.Entities;
using Shop.Products.Infrastructure.Persistence;

namespace Shop.Products.Infrastructure.Services;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, bool addProductExamples = true)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await context.Database.MigrateAsync();
        
        if (addProductExamples)
        {
            await AddProductExamplesAsync(context);
        }
    }
    
    private static async Task AddProductExamplesAsync(DatabaseContext context)
    {
        if (context.Products.Any()) return;
        var currentTime = DateTime.UtcNow;
            
        context.Products.AddRange(
            new Product 
            { 
                Name = "iPhone 17 Pro",
                ImageUrl = "/images/iphone17pro.jpg",
                Price = 28_900,
                Description = "The best Apple smartphone.",
                Quantity = 25,
                CreatedAt = currentTime,
                LastUpdatedAt = currentTime
            },
            new Product 
            { 
                Name = "Samsung Galaxy S26",
                ImageUrl = "/images/galaxys26.jpg",
                Price = 29_000,
                Description = "Powerful Android smartphone with AI features",
                Quantity = 30,
                CreatedAt = currentTime,
                LastUpdatedAt = currentTime
            },
            new Product 
            { 
                Name = "MacBook Air M3",
                ImageUrl = "/images/macbookair.jpg",
                Price = 59_900,
                Description = "Lightweight and powerful laptop with Apple M3 chip",
                Quantity = 15,
                CreatedAt = currentTime,
                LastUpdatedAt = currentTime
            },
            new Product 
            { 
                Name = "Sony WH-1000XM5",
                ImageUrl = "/images/sony-headphones.jpg",
                Price = 6_900,
                Description = "Wireless noise-canceling headphones",
                Quantity = 40,
                CreatedAt = currentTime,
                LastUpdatedAt = currentTime
            },
            new Product 
            { 
                Name = "iPad Pro 12.9",
                ImageUrl = "/images/ipadpro.jpg",
                Price = 23_900,
                Description = "Professional Apple tablet with Liquid Retina XDR display",
                Quantity = 20,
                CreatedAt = currentTime,
                LastUpdatedAt = currentTime
            },
            new Product 
            { 
                Name = "Sony PlayStation 5 pro",
                ImageUrl = "/images/ps5pro.jpg",
                Price = 12_999,
                Description = "Next-generation game console",
                Quantity = 10,
                CreatedAt = currentTime,
                LastUpdatedAt = currentTime
            }
        );

        await context.SaveChangesAsync();
    }
}