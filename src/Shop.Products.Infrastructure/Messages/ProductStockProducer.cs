using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Application.Messaging;
using Shop.Products.Infrastructure.Configurations;

namespace Shop.Products.Infrastructure.Messages;

internal class ProductStockProducer : IProductStockProducer
{
    private readonly KafkaSettings _settings;
    private readonly IProducer<Null, string> _producer;

    public ProductStockProducer(IOptions<KafkaSettings> settings)
    {
        _settings = settings.Value;
        var config = new ProducerConfig { BootstrapServers = _settings.BootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendAsync(PatchProductMessage request)
    {
        var message = JsonSerializer.Serialize(request);
        await _producer.ProduceAsync(_settings.Topic, new Message<Null, string> { Value = message });
    }
}