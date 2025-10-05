using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Infrastructure.Configurations;

namespace Shop.Products.Infrastructure.Messages;

/// <summary>
///     Background service that consumes product stock update messages from Kafka
///     and updates product quantities in the repository.
/// </summary>
internal class ProductStockConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaSettings _settings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProductStockConsumer" /> class.
    /// </summary>
    /// <param name="scopeFactory">The service scope factory used to create scopes for repository access.</param>
    /// <param name="settings">Kafka settings containing bootstrap servers and topic name.</param>
    public ProductStockConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaSettings> settings)
    {
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
    }

    /// <summary>
    ///     Executes the background service to consume messages and update product quantities.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token used to stop the service.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = "product-stock-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_settings.Topic);

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var consumeTask = Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
                var result = await consumeTask.ConfigureAwait(false);

                if (result == null || result.IsPartitionEOF) continue;

                var request = JsonSerializer.Deserialize<PatchProductMessage>(result.Message.Value);

                if (request != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    await repository.UpdateProductQuantityAsync(request.ProductId, request.NewQuantity, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
    }
}