namespace Shop.Products.Application.Dto.Requests;

/// <summary>
/// Represents a request to create a product with specified properties.
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// The name of the product to be created.
    /// This property is required and specifies the product's name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the image associated with the product.
    /// This property is required and specifies the product's image.
    /// </summary>
    public required string ImageUrl { get; init; }

    /// <summary>
    /// The price of the product to be created.
    /// </summary>
    public decimal Price { get; init; } = 0;

    /// <summary>
    /// The description of the product to be created.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// The quantity of the product to be created.
    /// </summary>
    public int Quantity { get; init; } = 0;
}