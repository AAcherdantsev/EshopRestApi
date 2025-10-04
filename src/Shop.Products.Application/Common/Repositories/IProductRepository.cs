using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Application.Common.Repositories;

public interface IProductRepository
{
    Task GetAllProductsAsync();
    
    Task GetProductByIdAsync(int productId);
    
    Task CreateProductAsync(CreateProductRequest request);
    
    Task UpdateProductQuantityAsync(int productId, int newQuantity);
}