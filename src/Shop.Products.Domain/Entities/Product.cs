namespace Shop.Products.Domain.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    
    public required string ImageUrl { get; set; }
    
    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime LastUpdatedAt { get; set; }
    
    public override string ToString()
    {
        return $"Id:{Id}, Name:{Name}, Price:{Price}, Quantity:{Quantity}, ImageUrl:{ImageUrl}, " +
               $"Description:{(string.IsNullOrEmpty(Description) ? "-" : Description)}, " +
               $"Created:{CreatedAt:dd.MM.yyyy hh:mm:ss}, LastUpdatedAt:{LastUpdatedAt:dd.MM.yyyy hh:mm:ss}";
    }
}