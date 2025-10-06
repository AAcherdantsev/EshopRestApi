using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Messaging;
using Shop.Products.Infrastructure.Configurations;
using Shop.Products.Infrastructure.Extensions;
using Shop.Products.Infrastructure.Messages;
using Shop.Products.Infrastructure.Persistence;
using Shop.Products.Infrastructure.Persistence.Repositories;

namespace Shop.Products.Infrastructure.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    [SetUp]
    public void Setup()
    {
        var inMemoryConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ConnectionStrings:ProductsDb",
                    "Server=(localdb)\\MSSQLLocalDB;Database=TestDb;Trusted_Connection=True;"),
                new KeyValuePair<string, string?>("ConnectionStrings:kafka", "localhost:9092")
            })
            .Build();

        _builder = Host.CreateApplicationBuilder();
        _builder.Configuration.AddConfiguration(inMemoryConfig);
    }

    private IHostApplicationBuilder _builder;

    [Test]
    public void AddMessageServices_RegistersKafkaRelatedServices()
    {
        // Act
        _builder.AddMessageServices();

        // Assert
        var provider = _builder.Services.BuildServiceProvider();

        Assert.That(provider.GetRequiredService<TopicInitializer>(), Is.Not.Null);
        Assert.That(provider.GetRequiredService<IProductStockProducer>(), Is.InstanceOf<ProductStockProducer>());

        var options = provider.GetRequiredService<IOptions<KafkaSettings>>().Value;
        Assert.That(options.BootstrapServers, Is.EqualTo("localhost:9092"));
        Assert.That(options.Topic, Is.EqualTo("product-stock-topic"));
    }

    [Test]
    public void AddMessageServices_Throws_WhenKafkaConnectionStringMissing()
    {
        // Arrange
        var badBuilder = Host.CreateApplicationBuilder();
        badBuilder.Configuration.AddInMemoryCollection(new[]
        {
            new KeyValuePair<string, string?>("ConnectionStrings:ProductsDb", "fake")
        });

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            badBuilder.AddMessageServices();
            badBuilder.Services.BuildServiceProvider().GetRequiredService<TopicInitializer>();
        });
    }

    [Test]
    public void AddRepository_RegistersDatabaseContextAndRepository()
    {
        // Act
        _builder.AddRepository();
        var provider = _builder.Services.BuildServiceProvider();

        // Act + Assert
        var dbContext = provider.GetRequiredService<DatabaseContext>();
        Assert.That(dbContext, Is.Not.Null);

        var repo = provider.GetRequiredService<IProductRepository>();
        Assert.That(repo, Is.InstanceOf<ProductRepository>());
    }

    [Test]
    public void AddRepository_ConfiguresDbContextWithSqlServer()
    {
        // Act
        _builder.AddRepository();
        var provider = _builder.Services.BuildServiceProvider();

        // Assert
        var options = provider.GetRequiredService<DbContextOptions<DatabaseContext>>();

        var sqlServerExtension = options.Extensions
            .OfType<SqlServerOptionsExtension>()
            .FirstOrDefault();

        Assert.That(sqlServerExtension, Is.Not.Null, "DbContext must be configured with SqlServer");
    }
}