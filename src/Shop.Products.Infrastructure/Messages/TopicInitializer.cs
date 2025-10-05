using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Shop.Products.Infrastructure.Configurations;

namespace Shop.Products.Infrastructure.Messages;

public class TopicInitializer
{
    private readonly KafkaSettings _settings;

    public TopicInitializer(IOptions<KafkaSettings> options)
    {
        _settings = options.Value;
    }

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