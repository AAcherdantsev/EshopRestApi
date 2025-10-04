namespace Shop.Products.Application.Dto.Products;

/// <summary>
/// Represents a product data transfer object.
/// </summary>
public class ProductDto
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the product.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the product image.
    /// </summary>
    public required string ImageUrl { get; init; }

    /// <summary>
    /// The price of the product.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// The description of the product.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// The available quantity of the product.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// The date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The date and time when the product was last updated.
    /// </summary>
    public DateTime LastUpdatedAt { get; init; }
}