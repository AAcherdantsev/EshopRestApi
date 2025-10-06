using System.Reflection;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Moq;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Infrastructure.Configurations;
using Shop.Products.Infrastructure.Messages;

namespace Shop.Products.Infrastructure.Tests.Messages;

[TestFixture]
public class ProductStockProducerTests
{
    [SetUp]
    public void SetUp()
    {
        _settings = new KafkaSettings
        {
            BootstrapServers = "localhost:9092",
            Topic = "product-stock-topic"
        };

        _mockProducer = new Mock<IProducer<Null, string>>();
        CreateProducerWithMock(_mockProducer.Object, _settings);
    }

    private Mock<IProducer<Null, string>> _mockProducer;
    private KafkaSettings _settings;

    private static ProductStockProducer CreateProducerWithMock(IProducer<Null, string> mockProducer,
        KafkaSettings settings)
    {
        var producer = new ProductStockProducer(Options.Create(settings));
        var field = typeof(ProductStockProducer).GetField("_producer",
            BindingFlags.NonPublic | BindingFlags.Instance);
        field!.SetValue(producer, mockProducer);
        return producer;
    }

    [Test]
    public async Task Constructor_ShouldInitializeProducer()
    {
        // Arrange
        var mockProducer = new Mock<IProducer<Null, string>>();
        var producer = new ProductStockProducer(Options.Create(_settings));

        var field = typeof(ProductStockProducer)
            .GetField("_producer", BindingFlags.NonPublic | BindingFlags.Instance)!;
        field.SetValue(producer, mockProducer.Object);

        var message = new PatchProductMessage { ProductId = 1, NewQuantity = 10 };

        // Act
        await producer.SendAsync(message);

        mockProducer.Verify(p => p.ProduceAsync(
                _settings.Topic,
                It.IsAny<Message<Null, string>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}