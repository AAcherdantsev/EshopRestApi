using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Shop.Products.Application.Messaging;
using Shop.Products.Infrastructure.Configurations;

namespace Shop.Products.Infrastructure.Messages;

/// <summary>
///     Ensures that the configured Kafka topic exists, creating it if necessary.
/// </summary>
public class TopicInitializer
{
    private readonly IAdminClientFactory _factory;
    private readonly KafkaSettings _settings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TopicInitializer" /> class.
    /// </summary>
    public TopicInitializer(IOptions<KafkaSettings> options, IAdminClientFactory factory)
    {
        _settings = options.Value;
        _factory = factory;
    }

    public async Task EnsureTopicExistsAsync()
    {
        var config = new AdminClientConfig { BootstrapServers = _settings.BootstrapServers };
        using var adminClient = _factory.Create(config);

        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
        var topicExists = metadata.Topics.Any(t => t.Topic == _settings.Topic);

        if (!topicExists)
            await adminClient.CreateTopicsAsync([
                new TopicSpecification
                {
                    Name = _settings.Topic,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                }
            ]);
    }
}