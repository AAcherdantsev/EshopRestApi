namespace Shop.Products.Application.Dto.Requests;

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