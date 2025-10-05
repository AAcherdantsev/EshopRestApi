using FluentResults;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Application.Common.Repositories;

public interface IProductRepository
{
    Task<Result<IEnumerable<Product>>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    
    Task<Result<PagedList<Product>>> GetAllProductsAsync(GetPagedProductListRequest request, CancellationToken cancellationToken = default);
    
    Task<Result<Product>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);
    
    Task<Result<Product>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    
    Task<Result<Product>> UpdateProductQuantityAsync(int productId, int newQuantity, CancellationToken cancellationToken = default);
}