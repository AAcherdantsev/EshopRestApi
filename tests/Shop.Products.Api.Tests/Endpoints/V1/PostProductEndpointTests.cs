using System.Text;
using System.Text.Json;
using FluentResults;
using Moq;
using Shop.Products.Api.Tests.IntegrationTests;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Tests.Endpoints.V1;

[TestFixture]
public class PostProductEndpointTests : IntegrationTestBase
{
    [Test]
    public async Task PostProduct_CreatesProduct_WhenRepositoryIsSuccessful()
    {
        // Arrange
        var domainProduct = new Product
        {
            Id = 1,
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
            Id = 1,
            Name = "Test Product",
            ImageUrl = "http://image",
            Price = 10,
            Description = "desc",
            Quantity = 5,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        var request = new CreateProductRequest
        {
            Name = "Test Product",
            ImageUrl = "http://image",
            Price = 10,
            Description = "desc",
            Quantity = 5
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        RepositoryMock.Setup(r => r.CreateProductAsync(It.IsAny<CreateProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(domainProduct));
        MapperMock.Setup(m => m.Map<ProductDto>(domainProduct)).Returns(productDto);

        // Act
        var response = await Client.PostAsync("/v1/products", content);

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        RepositoryMock.Verify(
            r => r.CreateProductAsync(It.IsAny<CreateProductRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        MapperMock.Verify(m => m.Map<ProductDto>(domainProduct), Times.Once);
    }
}