using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shop.Products.Application.Common;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Domain.Entities;
using Shop.Products.Infrastructure.Errors;

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

    public async Task<Result<IEnumerable<Product>>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all products...");
        
        var products = await _context.Products
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return products;
    }

    public Task<Result<PagedList<Product>>> GetAllProductsAsync(GetPagedProductListRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all products with pagination...");
        
        var query = _context.Products
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking();
        
        return Task.FromResult<Result<PagedList<Product>>>(PagedList<Product>.Create(query, request.PageNumber, request.PageSize));
    }

    public async Task<Result<Product>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting product with ID {productId}...", productId);
        
        var product = await _context.Products.FindAsync(productId);

        if (product != null)
        {
            return Result.Ok(product);
        }
        
        _logger.LogWarning("Product with ID {productId} not found", productId);
        return Result.Fail(new NotFoundError("Product not found."));
    }

    public async Task<Result<Product>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product()
        {
            Name = request.Name,
            Price = request.Price,
            Quantity = request.Quantity,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            Description = request.Description,
        };
        
        _logger.LogInformation("Creating product {product}...", product);

        try
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product {product} created successfully", product);
            return Result.Ok(product);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating product {product}", product);
            return Result.Fail($"Error creating product. Exception: {e.Message}");
        }
    }

    public async Task<Result<Product>> UpdateProductQuantityAsync(int productId, int newQuantity, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {productId} not found. Unable to update the product.", productId);
            return Result.Fail(new NotFoundError("Product not found."));
        }
        
        product.Quantity = newQuantity;
        product.LastUpdatedAt = DateTime.UtcNow;
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product {product} updated successfully", product);
            return Result.Ok(product);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating product {product}", product);
            return Result.Fail("Error updating product.");
        }
    }
}