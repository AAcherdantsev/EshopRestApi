namespace Shop.Products.Domain.Entities;

/// <summary>
/// Domain entity representing a product.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// The product name.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// URL of the product image.
    /// </summary>
    public required string ImageUrl { get; set; }
    
    /// <summary>
    /// Price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Quantity of the product in stock.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Date and time when the product was last updated.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Id:{Id}, Name:{Name}, Price:{Price}, Quantity:{Quantity}, ImageUrl:{ImageUrl}, " +
               $"Description:{(string.IsNullOrEmpty(Description) ? "-" : Description)}, " +
               $"Created:{CreatedAt:dd.MM.yyyy hh:mm:ss}, LastUpdatedAt:{LastUpdatedAt:dd.MM.yyyy hh:mm:ss}";
    }
}