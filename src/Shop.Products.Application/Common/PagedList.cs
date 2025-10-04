namespace Shop.Products.Application.Common;

/// <summary>
/// Represents a paginated list of items.
/// </summary>
/// <typeparam name="T">The type of elements contained in the list.</typeparam>
public class PagedList<T>
{
    /// <summary>
    /// Gets an empty paged list.
    /// </summary>
    public static PagedList<T> Empty =>
        new() { PageNumber = 0, PageSize = 0, Values = [] };

    /// <summary>
    /// The current page
    /// </summary>
    public int PageNumber { get; init; } = 0;

    /// <summary>
    /// The page size
    /// </summary>
    public int? PageSize { get; init; } = 10;

    /// <summary>
    /// The total number of elements across all pages.
    /// </summary>
    public long TotalCount { get; init; }

    /// <summary>
    /// The values of the current page.
    /// </summary>
    public IReadOnlyCollection<T> Values { get; init; } = [];
}