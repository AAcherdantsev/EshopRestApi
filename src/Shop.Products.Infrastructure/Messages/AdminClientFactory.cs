using Confluent.Kafka;
using Shop.Products.Application.Messaging;

namespace Shop.Products.Infrastructure.Messages;

/// <summary>
///     Admin client factory.
/// </summary>
internal class AdminClientFactory : IAdminClientFactory
{
    /// <inheritdoc />
    public IAdminClient Create(AdminClientConfig config)
    {
        return new AdminClientBuilder(config).Build();
    }
}