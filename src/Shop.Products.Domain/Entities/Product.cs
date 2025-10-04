namespace Shop.Products.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; init; }
    
    public required string ImageUrl { get; init; }
    
    public decimal Price { get; init; }

    public string Description { get; init; } = string.Empty;
    
    public int Quantity { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime LastUpdatedAt { get; init; }
    
    public override string ToString()
    {
        return $"Id:{Id}, Name:{Name}, Price:{Price}, Quantity:{Quantity}, ImageUrl:{ImageUrl}, " +
               $"Description:{(string.IsNullOrEmpty(Description) ? "-" : Description)}, " +
               $"Created:{CreatedAt:dd.MM.yyyy hh:mm:ss}, LastUpdatedAt:{LastUpdatedAt:dd.MM.yyyy hh:mm:ss}";
    }
}