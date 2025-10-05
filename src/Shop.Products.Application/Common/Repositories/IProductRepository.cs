using FluentResults;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Application.Common.Repositories;

/// <summary>
/// Repository interface for managing products.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Result<IEnumerable<Product>>> GetAllProductsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of products based on the request.
    /// </summary>
    /// <param name="request">The pagination request parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Result<PagedList<Product>>> GetAllProductsAsync(GetPagedProductListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="productId">The ID of the product.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Result<Product>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="request">The request containing product details.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Result<Product>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the quantity of an existing product.
    /// </summary>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="newQuantity">The new quantity value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Result<Product>> UpdateProductQuantityAsync(int productId, int newQuantity, CancellationToken cancellationToken = default);
}