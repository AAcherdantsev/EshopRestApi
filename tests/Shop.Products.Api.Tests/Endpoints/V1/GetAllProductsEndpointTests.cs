using FluentResults;
using Moq;
using Shop.Products.Api.Tests.IntegrationTests;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Tests.Endpoints.V1;

[TestFixture]
public class GetAllProductsEndpointTests : IntegrationTestBase
{
    [Test]
    public async Task GetProducts_ReturnsProducts_WhenRepositoryIsSuccessful()
    {
        // Arrange
        var domainProducts = new List<Product>
        {
            new()
            {
                Id = 1,
                Name = "Test Product",
                ImageUrl = "http://image",
                Price = 10,
                Description = "desc",
                Quantity = 5,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            }
        };

        var productDtos = new List<ProductDto>
        {
            new()
            {
                Id = 1,
                Name = "Test Product",
                ImageUrl = "http://image",
                Price = 10,
                Description = "desc",
                Quantity = 5,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            }
        };

        RepositoryMock.Setup(r => r.GetAllProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(domainProducts.AsEnumerable()));
        MapperMock.Setup(m => m.Map<List<ProductDto>>(domainProducts)).Returns(productDtos);

        // Act
        var response = await Client.GetAsync("/v1/products");
        await response.Content.ReadAsStringAsync();

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        RepositoryMock.Verify(r => r.GetAllProductsAsync(It.IsAny<CancellationToken>()), Times.Once);
        MapperMock.Verify(m => m.Map<List<ProductDto>>(domainProducts), Times.Once);
    }
}