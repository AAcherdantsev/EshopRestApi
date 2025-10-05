using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Shop.Products.Infrastructure.Configurations;

namespace Shop.Products.Infrastructure.Messages;

/// <summary>
/// Ensures that the configured Kafka topic exists, creating it if necessary.
/// </summary>
public class TopicInitializer
{
    private readonly KafkaSettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicInitializer"/> class.
    /// </summary>
    /// <param name="options">Kafka settings containing bootstrap servers and topic name.</param>
    public TopicInitializer(IOptions<KafkaSettings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Ensures the Kafka topic exists by checking metadata and creating the topic if it does not exist.
    /// </summary>
    public async Task EnsureTopicExistsAsync()
    {
        var config = new AdminClientConfig { BootstrapServers = _settings.BootstrapServers };
        using var adminClient = new AdminClientBuilder(config).Build();

        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
        var topicExists = metadata.Topics.Any(t => t.Topic == _settings.Topic);

        if (!topicExists)
        {
            await adminClient.CreateTopicsAsync([
                new TopicSpecification()
                {
                    Name = _settings.Topic,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                }
            ]);
        }
    }
}