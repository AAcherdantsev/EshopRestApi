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
    /// The current page (-1 to get all data)
    /// </summary>
    public int PageNumber { get; init; } 

    /// <summary>
    /// The page size
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// The total number of elements across all pages.
    /// </summary>
    public long TotalCount { get; init; }

    /// <summary>
    /// The values of the current page.
    /// </summary>
    public IReadOnlyCollection<T> Values { get; init; } = [];
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class with the specified values, total count, page number, and page size.
    /// </summary>
    /// <param name="values">The items in the current page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The size of each page.</param>
    public PagedList(IEnumerable<T> values, long totalCount, int page, int pageSize)
    {
        Values = new List<T>(values).AsReadOnly();
        TotalCount = totalCount;
        PageNumber = page;
        PageSize = pageSize;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    private PagedList()
    {
    }
    
    /// <summary>
    /// Creates a <see cref="PagedList{T}"/> from the specified query, page number, and page size.
    /// </summary>
    /// <param name="query">The source query from which to retrieve the items.</param>
    /// <param name="page">The current page number. If less than 0, all items are returned.</param>
    /// <param name="pageSize">The size of each page.</param>
    /// <returns>A <see cref="PagedList{T}"/> containing the items for the specified page.</returns>
    public static PagedList<T> Create(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = query.Count();

        List<T> values;

        if (page >= 0)
        {
            values = query.Skip(page * pageSize).Take(pageSize).ToList();
        }
        else
        {
            values = query.ToList();
        }

        return new PagedList<T>(values, totalCount, page, pageSize);
    }
}