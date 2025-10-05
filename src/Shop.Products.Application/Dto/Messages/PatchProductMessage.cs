namespace Shop.Products.Application.Dto.Messages;

/// <summary>
/// Represents a message used to update the quantity of a product.
/// </summary>
public record PatchProductMessage
{
    /// <summary>
    /// The product identifier.
    /// </summary>
    public int ProductId { get; init; }
    
    /// <summary>
    /// The new quantity for the product to be updated.
    /// </summary>
    public int NewQuantity { get; init; }
}