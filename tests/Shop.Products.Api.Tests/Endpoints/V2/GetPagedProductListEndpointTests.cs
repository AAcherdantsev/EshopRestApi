using FluentResults;
using Moq;
using Shop.Products.Api.Tests.IntegrationTests;
using Shop.Products.Application.Common;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Tests.Endpoints.V2;

[TestFixture]
public class GetPagedProductListEndpointTests : IntegrationTestBase
{
    [Test]
    public async Task GetPagedProducts_ReturnsPagedProducts_WhenRepositoryIsSuccessful()
    {
        // Arrange
        var domainPagedList = new PagedList<Product>(
            new List<Product>
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
            }, 1, 10, 1);

        var pagedList = new PagedList<ProductDto>(
            new List<ProductDto>
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
            }, 1, 10, 1);

        RepositoryMock.Setup(r =>
                r.GetAllProductsAsync(It.IsAny<GetPagedProductListRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(domainPagedList));
        MapperMock.Setup(m => m.Map<PagedList<ProductDto>>(domainPagedList)).Returns(pagedList);

        // Act
        var response = await Client.GetAsync("/v2/products?pageNumber=1&pageSize=10");

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        RepositoryMock.Verify(
            r => r.GetAllProductsAsync(It.IsAny<GetPagedProductListRequest>(), It.IsAny<CancellationToken>()),
            Times.Once);
        MapperMock.Verify(m => m.Map<PagedList<ProductDto>>(domainPagedList), Times.Once);
    }
}