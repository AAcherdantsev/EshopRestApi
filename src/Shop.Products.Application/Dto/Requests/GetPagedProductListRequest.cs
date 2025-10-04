namespace Shop.Products.Application.Dto.Requests;

/// <summary>
/// Represents a request for retrieving a paginated list of products.
/// </summary>
public class GetPagedProductListRequest
{
    /// <summary>
    /// The current page
    /// </summary>
    public int PageNumber { get; init; } = 0;

    /// <summary>
    /// The page size
    /// </summary>
    public int PageSize { get; init; } = 10;
}