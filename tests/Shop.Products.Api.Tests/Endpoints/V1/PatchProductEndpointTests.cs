using System.Text;
using System.Text.Json;
using FluentResults;
using Moq;
using Shop.Products.Api.Tests.IntegrationTests;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Tests.Endpoints.V1;

[TestFixture]
public class PatchProductEndpointTests : IntegrationTestBase
{
    [Test]
    public async Task PatchProduct_UpdatesProduct_WhenRepositoryIsSuccessful()
    {
        // Arrange
        var productId = 1;
        var newQuantity = 10;
        var domainProduct = new Product
        {
            Id = productId,
            Name = "Test Product",
            ImageUrl = "http://image",
            Price = 10,
            Description = "desc",
            Quantity = newQuantity,
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
            Quantity = newQuantity,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        var request = new { NewQuantity = newQuantity };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        RepositoryMock.Setup(r => r.UpdateProductQuantityAsync(productId, newQuantity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(domainProduct));
        MapperMock.Setup(m => m.Map<ProductDto>(domainProduct)).Returns(productDto);

        // Act
        var response = await Client.PatchAsync($"/v1/products/{productId}", content);

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        RepositoryMock.Verify(r => r.UpdateProductQuantityAsync(productId, newQuantity, It.IsAny<CancellationToken>()),
            Times.Once);
        MapperMock.Verify(m => m.Map<ProductDto>(domainProduct), Times.Once);
    }
}