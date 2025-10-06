using System.Text;
using System.Text.Json;
using Moq;
using Shop.Products.Api.Tests.IntegrationTests;
using Shop.Products.Application.Dto.Messages;

namespace Shop.Products.Api.Tests.Endpoints.V2;

[TestFixture]
public class PatchProductEndpointTests : IntegrationTestBase
{
    [Test]
    public async Task PatchProduct_SendsPatchMessage_WhenCalled()
    {
        // Arrange
        var productId = 1;
        var newQuantity = 10;
        var request = new { NewQuantity = newQuantity };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        ProducerMock.Setup(p => p.SendAsync(It.IsAny<PatchProductMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var response = await Client.PatchAsync($"/v2/products/{productId}", content);

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        ProducerMock.Verify(p => p.SendAsync(It.Is<PatchProductMessage>(msg =>
            msg.ProductId == productId && msg.NewQuantity == newQuantity)), Times.Once);
    }
}