namespace Shop.Products.Application.Dto.Messages;

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