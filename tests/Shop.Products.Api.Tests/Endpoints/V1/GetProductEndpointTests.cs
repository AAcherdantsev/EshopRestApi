using FluentResults;
using Moq;
using Shop.Products.Api.Tests.IntegrationTests;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Tests.Endpoints.V1;

[TestFixture]
public class GetProductEndpointTests : IntegrationTestBase
{
    [Test]
    public async Task GetProduct_ReturnsProduct_WhenRepositoryIsSuccessful()
    {
        // Arrange
        var productId = 1;
        var domainProduct = new Product
        {
            Id = productId,
            Name = "Test Product",
            ImageUrl = "http://image",
            Price = 10,
            Description = "desc",
            Quantity = 5,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        var productDto = new ProductDto
        {
            Id = productId,
            Name = "Test Product",
            ImageUrl = "http://image",
            Price = 10,
            Description = "desc",
            Quantity = 5,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        RepositoryMock.Setup(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(domainProduct));
        MapperMock.Setup(m => m.Map<ProductDto>(domainProduct)).Returns(productDto);

        // Act
        var response = await Client.GetAsync($"/v1/products/{productId}");

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        RepositoryMock.Verify(r => r.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        MapperMock.Verify(m => m.Map<ProductDto>(domainProduct), Times.Once);
    }
}