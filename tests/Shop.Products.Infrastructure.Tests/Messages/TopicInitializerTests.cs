using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;
using Moq;
using Shop.Products.Application.Messaging;
using Shop.Products.Infrastructure.Configurations;
using Shop.Products.Infrastructure.Messages;

namespace Shop.Products.Infrastructure.Tests.Messages;

public class TopicInitializerTests
{
    private KafkaSettings _settings = new()
    {
        BootstrapServers = "localhost:9092",
        Topic = "test-topic"
    };

    [Test]
    public async Task EnsureTopicExistsAsync_WhenTopicExists_DoesNotCreate()
    {
        var metadata = new Metadata([], [new TopicMetadata("test-topic", [], null)], 1, "broker");

        var mockAdmin = new Mock<IAdminClient>();
        mockAdmin.Setup(a => a.GetMetadata(It.IsAny<TimeSpan>())).Returns(metadata);

        var mockFactory = new Mock<IAdminClientFactory>();
        mockFactory.Setup(f => f.Create(It.IsAny<AdminClientConfig>())).Returns(mockAdmin.Object);

        var initializer = new TopicInitializer(Options.Create(_settings), mockFactory.Object);

        await initializer.EnsureTopicExistsAsync();

        mockAdmin.Verify(a => a.CreateTopicsAsync(It.IsAny<IEnumerable<TopicSpecification>>(), It.IsAny<CreateTopicsOptions>()),
            Times.Never);
    }

    [Test]
    public async Task EnsureTopicExistsAsync_WhenTopicDoesNotExist_CreatesTopic()
    {
        var metadata = new Metadata(
            new List<BrokerMetadata>(),
            new List<TopicMetadata>(), 1, "broker"
        );

        var mockAdmin = new Mock<IAdminClient>();
        mockAdmin.Setup(a => a.GetMetadata(It.IsAny<TimeSpan>())).Returns(metadata);

        mockAdmin.Setup(a => a.CreateTopicsAsync(It.IsAny<IEnumerable<TopicSpecification>>(), null))
            .Returns(Task.CompletedTask);

        var mockFactory = new Mock<IAdminClientFactory>();
        mockFactory.Setup(f => f.Create(It.IsAny<AdminClientConfig>())).Returns(mockAdmin.Object);

        var initializer = new TopicInitializer(Options.Create(_settings), mockFactory.Object);

        await initializer.EnsureTopicExistsAsync();

        mockAdmin.Verify(a => a.CreateTopicsAsync(
            It.Is<IEnumerable<TopicSpecification>>(t => t.First().Name == _settings.Topic), null), Times.Once);
    }
}
