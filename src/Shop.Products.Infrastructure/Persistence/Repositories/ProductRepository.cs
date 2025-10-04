using Microsoft.Extensions.Logging;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DatabaseContext _context;
    private readonly ILogger<ProductRepository> _logger;
    
    public ProductRepository(DatabaseContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task GetAllProductsAsync()
    {
        throw new NotImplementedException();
    }

    public Task GetProductByIdAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task CreateProductAsync(CreateProductRequest request)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProductQuantityAsync(int productId, int newQuantity)
    {
        throw new NotImplementedException();
    }
}