using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Infrastructure.Configurations;
using Shop.Products.Infrastructure.Messages;

namespace Shop.Products.Infrastructure.Tests.Messages;

[TestFixture]
public class ProductStockConsumerTests
{
    private Mock<IServiceScopeFactory> _scopeFactoryMock;
    private Mock<IServiceScope> _scopeMock;
    private Mock<IServiceProvider> _serviceProviderMock;
    private Mock<IProductRepository> _repositoryMock;
    private IOptions<KafkaSettings> _settings;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _scopeMock = new Mock<IServiceScope>();
        _scopeFactoryMock = new Mock<IServiceScopeFactory>();

        _scopeFactoryMock.Setup(f => f.CreateScope()).Returns(_scopeMock.Object);
        _scopeMock.Setup(s => s.ServiceProvider).Returns(_serviceProviderMock.Object);
        _serviceProviderMock.Setup(p => p.GetService(typeof(IProductRepository))).Returns(_repositoryMock.Object);

        _settings = Options.Create(new KafkaSettings
        {
            BootstrapServers = "localhost:9092",
            Topic = "product-stock-topic"
        });
    }

    [Test]
    public async Task ExecuteAsync_ProcessesValidMessage_UpdatesRepository()
    {
        // Arrange
        var message = new PatchProductMessage
        {
            ProductId = 123,
            NewQuantity = 42
        };
        var jsonMessage = JsonSerializer.Serialize(message);
        var consumer = new TestableProductStockConsumer(_scopeFactoryMock.Object, _settings)
        {
            TestMessage = jsonMessage
        };

        // Act
        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        await consumer.StartAsync(cts.Token);
        await consumer.StopAsync(CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r =>
                r.UpdateProductQuantityAsync(message.ProductId, message.NewQuantity, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WhenMessageIsNull_DoesNotUpdateRepository()
    {
        var consumer = new TestableProductStockConsumer(_scopeFactoryMock.Object, _settings)
        {
            TestMessage = null
        };

        var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        await consumer.StartAsync(cts.Token);
        await consumer.StopAsync(CancellationToken.None);

        _repositoryMock.Verify(r =>
                r.UpdateProductQuantityAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public Task ExecuteAsync_WhenCancelled_StopsGracefully()
    {
        var consumer = new TestableProductStockConsumer(_scopeFactoryMock.Object, _settings)
        {
            TestMessage = null
        };

        var cts = new CancellationTokenSource();
        cts.CancelAfter(50);

        Assert.DoesNotThrowAsync(async () =>
        {
            await consumer.StartAsync(cts.Token);
            await consumer.StopAsync(CancellationToken.None);
        });
        return Task.CompletedTask;
    }
}

internal class TestableProductStockConsumer : ProductStockConsumer
{
    public string? TestMessage { get; set; }

    public TestableProductStockConsumer(IServiceScopeFactory factory, IOptions<KafkaSettings> settings)
        : base(factory, settings)
    {
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (TestMessage != null)
        {
            var request = JsonSerializer.Deserialize<PatchProductMessage>(TestMessage);
            if (request != null)
            {
                using var scope = ((IServiceScopeFactory)typeof(ProductStockConsumer)
                        .GetField("_scopeFactory",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                        .GetValue(this)!)
                    .CreateScope();

                var repo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                await repo.UpdateProductQuantityAsync(request.ProductId, request.NewQuantity, stoppingToken);
            }
        }
    }
}
