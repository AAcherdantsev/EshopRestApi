using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Messaging;

namespace Shop.Products.Api.Tests.IntegrationTests;

public class IntegrationTestBase : IDisposable
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IProductStockProducer> ProducerMock;
    protected readonly Mock<IProductRepository> RepositoryMock;

    public IntegrationTestBase()
    {
        RepositoryMock = new Mock<IProductRepository>();
        ProducerMock = new Mock<IProductStockProducer>();
        MapperMock = new Mock<IMapper>();

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    // Add test configuration to override Kafka settings
                    var testConfig = new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:kafka"] = "localhost:9092", // This is what the extension method looks for
                        ["ConnectionStrings:ProductsDb"] =
                            "Server=(localdb)\\mssqllocaldb;Database=TestProductsDb;Trusted_Connection=true;MultipleActiveResultSets=true"
                    };
                    config.AddInMemoryCollection(testConfig);
                });

                builder.ConfigureServices(services =>
                {
                    // Remove all hosted services (including Kafka consumer) to prevent background processing in tests
                    var hostedServices = services.Where(d => d.ServiceType == typeof(IHostedService)).ToList();
                    foreach (var service in hostedServices) services.Remove(service);

                    // Remove the existing services
                    services.RemoveAll(typeof(IProductRepository));
                    services.RemoveAll(typeof(IProductStockProducer));
                    services.RemoveAll(typeof(IMapper));

                    // Add mocked services
                    services.AddSingleton(RepositoryMock.Object);
                    services.AddSingleton(ProducerMock.Object);
                    services.AddSingleton(MapperMock.Object);
                });
            });

        Client = Factory.CreateClient();
    }

    public void Dispose()
    {
        Client?.Dispose();
        Factory?.Dispose();
    }
}