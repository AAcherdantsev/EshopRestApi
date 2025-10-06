using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Domain.Entities;
using Shop.Products.Infrastructure.Errors;
using Shop.Products.Infrastructure.Persistence;
using Shop.Products.Infrastructure.Persistence.Repositories;

namespace Shop.Products.Infrastructure.Tests.Persistence.Repositories;

/// <summary>
///     Unit tests for the <see cref="ProductRepository" /> class.
/// </summary>
[TestFixture]
public class ProductRepositoryTests
{
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);
        _loggerMock = new Mock<ILogger<ProductRepository>>();
        _repository = new ProductRepository(_context, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }

    private DatabaseContext _context;
    private Mock<ILogger<ProductRepository>> _loggerMock;
    private ProductRepository _repository;

    [Test]
    public async Task GetAllProductsAsync_WithProducts_ReturnsAllProductsOrderedByCreatedAt()
    {
        // Arrange
        var products = CreateTestProducts();
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllProductsAsync();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Count(), Is.EqualTo(3));

        var productList = result.Value.ToList();
        Assert.That(productList[0].Name, Is.EqualTo("Product 1"));
        Assert.That(productList[1].Name, Is.EqualTo("Product 2"));
        Assert.That(productList[2].Name, Is.EqualTo("Product 3"));

        var dates = productList.Select(p => p.CreatedAt).ToList();
        Assert.That(dates, Is.EqualTo(dates.OrderBy(d => d)));
    }

    [Test]
    public async Task GetAllProductsAsync_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetAllProductsAsync();

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task CreateProductAsync_WithCancellationToken_CancelsOperation()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            Description = "Test Description",
            ImageUrl = "https://example.com/image.jpg",
            Price = 99.99m,
            Quantity = 10
        };
        var cancelledToken = new CancellationToken(true);

        // Act & Assert
        var result = await _repository.CreateProductAsync(request, cancelledToken);
        Assert.IsNotNull(result);
    }

    [Test]
    public async Task GetAllProductsAsync_WithPagination_ReturnsPagedResults()
    {
        // Arrange
        var products = CreateTestProducts();
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var request = new GetPagedProductListRequest { PageNumber = 0, PageSize = 2 };

        // Act
        var result = await _repository.GetAllProductsAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.PageNumber, Is.EqualTo(0));
        Assert.That(result.Value.PageSize, Is.EqualTo(2));
        Assert.That(result.Value.TotalCount, Is.EqualTo(3));
        Assert.That(result.Value.Values.Count, Is.EqualTo(2));

        var firstPageProducts = result.Value.Values.ToList();
        Assert.That(firstPageProducts[0].Name, Is.EqualTo("Product 1"));
        Assert.That(firstPageProducts[1].Name, Is.EqualTo("Product 2"));
    }

    [Test]
    public async Task GetAllProductsAsync_WithPaginationSecondPage_ReturnsCorrectPage()
    {
        // Arrange
        var products = CreateTestProducts();
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var request = new GetPagedProductListRequest { PageNumber = 1, PageSize = 2 };

        // Act
        var result = await _repository.GetAllProductsAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.PageNumber, Is.EqualTo(1));
        Assert.That(result.Value.PageSize, Is.EqualTo(2));
        Assert.That(result.Value.TotalCount, Is.EqualTo(3));
        Assert.That(result.Value.Values.Count, Is.EqualTo(1));

        var secondPageProducts = result.Value.Values.ToList();
        Assert.That(secondPageProducts[0].Name, Is.EqualTo("Product 3"));
    }

    [Test]
    public async Task GetAllProductsAsync_WithPaginationEmptyPage_ReturnsEmptyPage()
    {
        // Arrange
        var products = CreateTestProducts();
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var request = new GetPagedProductListRequest { PageNumber = 5, PageSize = 2 };

        // Act
        var result = await _repository.GetAllProductsAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.PageNumber, Is.EqualTo(5));
        Assert.That(result.Value.PageSize, Is.EqualTo(2));
        Assert.That(result.Value.TotalCount, Is.EqualTo(3));
        Assert.That(result.Value.Values.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetProductByIdAsync_WithExistingProduct_ReturnsProduct()
    {
        // Arrange
        var product = CreateTestProduct("Test Product", 99.99m, 10);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetProductByIdAsync(product.Id);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Id, Is.EqualTo(product.Id));
        Assert.That(result.Value.Name, Is.EqualTo("Test Product"));
        Assert.That(result.Value.Price, Is.EqualTo(99.99m));
        Assert.That(result.Value.Quantity, Is.EqualTo(10));
    }

    [Test]
    public async Task GetProductByIdAsync_WithNonExistentProduct_ReturnsNotFoundError()
    {
        // Act
        var result = await _repository.GetProductByIdAsync(999);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors.First(), Is.TypeOf<NotFoundError>());
        Assert.That(result.Errors.First().Message, Is.EqualTo("Product not found."));
    }

    [Test]
    public async Task GetProductByIdAsync_WithCancellationToken_CancelsOperation()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var result = await _repository.GetProductByIdAsync(1, cts.Token);

        Assert.That(result.IsFailed, Is.True);
    }

    [Test]
    public async Task CreateProductAsync_WithValidRequest_CreatesAndReturnsProduct()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "New Product",
            ImageUrl = "https://example.com/image.jpg",
            Price = 49.99m,
            Description = "A great product",
            Quantity = 5
        };

        // Act
        var result = await _repository.CreateProductAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Name, Is.EqualTo("New Product"));
        Assert.That(result.Value.ImageUrl, Is.EqualTo("https://example.com/image.jpg"));
        Assert.That(result.Value.Price, Is.EqualTo(49.99m));
        Assert.That(result.Value.Description, Is.EqualTo("A great product"));
        Assert.That(result.Value.Quantity, Is.EqualTo(5));
        Assert.That(result.Value.Id, Is.GreaterThan(0));

        var savedProduct = await _context.Products.FindAsync(result.Value.Id);
        Assert.That(savedProduct, Is.Not.Null);
        Assert.That(savedProduct.Name, Is.EqualTo("New Product"));
    }

    [Test]
    public async Task CreateProductAsync_WithMinimalRequest_CreatesProductWithDefaults()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Minimal Product",
            ImageUrl = "https://example.com/minimal.jpg"
        };

        // Act
        var result = await _repository.CreateProductAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Name, Is.EqualTo("Minimal Product"));
        Assert.That(result.Value.ImageUrl, Is.EqualTo("https://example.com/minimal.jpg"));
        Assert.That(result.Value.Price, Is.EqualTo(0));
        Assert.That(result.Value.Description, Is.EqualTo(string.Empty));
        Assert.That(result.Value.Quantity, Is.EqualTo(0));
    }

    [Test]
    public async Task CreateProductAsync_WithDatabaseException_ReturnsFailureResult()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Name = "Test Product",
            ImageUrl = "https://example.com/test.jpg"
        };

        _context.Dispose();

        // Act
        var result = await _repository.CreateProductAsync(request);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors.First().Message, Does.StartWith("Error creating product. Exception:"));
    }

    [Test]
    public async Task UpdateProductQuantityAsync_WithExistingProduct_UpdatesQuantityAndReturnsProduct()
    {
        // Arrange
        var product = CreateTestProduct("Test Product", 99.99m, 10);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var originalLastUpdatedAt = product.LastUpdatedAt;

        // Act
        var result = await _repository.UpdateProductQuantityAsync(product.Id, 25);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Id, Is.EqualTo(product.Id));
        Assert.That(result.Value.Quantity, Is.EqualTo(25));
        Assert.That(result.Value.LastUpdatedAt, Is.GreaterThan(originalLastUpdatedAt));

        // Verify in database
        var updatedProduct = await _context.Products.FindAsync(product.Id);
        Assert.That(updatedProduct, Is.Not.Null);
        Assert.That(updatedProduct!.Quantity, Is.EqualTo(25));
        Assert.That(updatedProduct.LastUpdatedAt, Is.GreaterThan(originalLastUpdatedAt));
    }

    [Test]
    public async Task UpdateProductQuantityAsync_WithNonExistentProduct_ReturnsNotFoundError()
    {
        // Act
        var result = await _repository.UpdateProductQuantityAsync(999, 10);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors.First(), Is.TypeOf<NotFoundError>());
        Assert.That(result.Errors.First().Message, Is.EqualTo("Product not found."));
    }

    [Test]
    public async Task UpdateProductQuantityAsync_WithZeroQuantity_UpdatesToZero()
    {
        // Arrange
        var product = CreateTestProduct("Test Product", 99.99m, 10);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.UpdateProductQuantityAsync(product.Id, 0);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Quantity, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateProductQuantityAsync_WithNegativeQuantity_UpdatesToNegative()
    {
        // Arrange
        var product = CreateTestProduct("Test Product", 99.99m, 10);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.UpdateProductQuantityAsync(product.Id, -5);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Quantity, Is.EqualTo(-5));
    }

    [Test]
    public async Task UpdateProductQuantityAsync_WithCancellationToken_CancelsOperation()
    {
        // Arrange
        var product = CreateTestProduct("Test Product", 99.99m, 10);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var cancelledToken = new CancellationToken(true);

        // Act & Assert
        var result = await _repository.UpdateProductQuantityAsync(product.Id, 50, cancelledToken);
        Assert.IsNotNull(result);
    }

    private static Product CreateTestProduct(string name, decimal price, int quantity)
    {
        return new Product
        {
            Name = name,
            ImageUrl = "https://example.com/image.jpg",
            Price = price,
            Description = "Test description",
            Quantity = quantity,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };
    }

    private static List<Product> CreateTestProducts()
    {
        var baseTime = DateTime.UtcNow.AddHours(-3);

        return new List<Product>
        {
            new()
            {
                Name = "Product 1",
                ImageUrl = "https://example.com/product1.jpg",
                Price = 10.99m,
                Description = "First product",
                Quantity = 5,
                CreatedAt = baseTime,
                LastUpdatedAt = baseTime
            },
            new()
            {
                Name = "Product 2",
                ImageUrl = "https://example.com/product2.jpg",
                Price = 20.99m,
                Description = "Second product",
                Quantity = 3,
                CreatedAt = baseTime.AddMinutes(1),
                LastUpdatedAt = baseTime.AddMinutes(1)
            },
            new()
            {
                Name = "Product 3",
                ImageUrl = "https://example.com/product3.jpg",
                Price = 30.99m,
                Description = "Third product",
                Quantity = 8,
                CreatedAt = baseTime.AddMinutes(2),
                LastUpdatedAt = baseTime.AddMinutes(2)
            }
        };
    }
}