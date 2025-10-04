namespace Shop.Products.Application.Dto.Requests;

/// <summary>
/// Represents a request to update a product.
/// </summary>
public class PatchProductRequest
{
    /// <summary>
    /// The new quantity for the product to be updated.
    /// </summary>
    public int NewQuantity { init; get;}
}