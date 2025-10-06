using Confluent.Kafka;

namespace Shop.Products.Application.Messaging;

public interface IAdminClientFactory
{
    IAdminClient Create(AdminClientConfig config);
}