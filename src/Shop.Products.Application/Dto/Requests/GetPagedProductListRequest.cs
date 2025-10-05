using System.ComponentModel;

namespace Shop.Products.Application.Dto.Requests;

/// <summary>
/// Represents a request for retrieving a paginated list of products.
/// </summary>
public class GetPagedProductListRequest
{
    /// <summary>
    /// The current page
    /// </summary>
    [DefaultValue(0)]
    public int PageNumber { get; init; }

    /// <summary>
    /// The page size
    /// </summary>
    [DefaultValue(10)]
    public int PageSize { get; init; }
}