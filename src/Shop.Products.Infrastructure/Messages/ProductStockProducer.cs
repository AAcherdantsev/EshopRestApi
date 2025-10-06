using System.Runtime.CompilerServices;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Application.Messaging;
using Shop.Products.Infrastructure.Configurations;

[assembly: InternalsVisibleTo("Shop.Products.Infrastructure.Tests")]
namespace Shop.Products.Infrastructure.Messages;

/// <summary>
///     Produces product stock update messages to a Kafka topic.
/// </summary>
internal class ProductStockProducer : IProductStockProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaSettings _settings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProductStockProducer" /> class.
    /// </summary>
    /// <param name="settings">Kafka settings containing bootstrap servers and topic.</param>
    public ProductStockProducer(IOptions<KafkaSettings> settings)
    {
        _settings = settings.Value;
        var config = new ProducerConfig { BootstrapServers = _settings.BootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    /// <summary>
    ///     Sends a patch product message asynchronously to Kafka.
    /// </summary>
    /// <param name="request">The message containing product ID and new quantity.</param>
    public async Task SendAsync(PatchProductMessage request)
    {
        var message = JsonSerializer.Serialize(request);
        await _producer.ProduceAsync(_settings.Topic, new Message<Null, string> { Value = message });
    }
}