using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Messaging;
using Shop.Products.Infrastructure.Configurations;
using Shop.Products.Infrastructure.Messages;
using Shop.Products.Infrastructure.Persistence;
using Shop.Products.Infrastructure.Persistence.Repositories;
using Shop.Products.Infrastructure.Services;

namespace Shop.Products.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMessageServices(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<KafkaSettings>(options =>
        {
            var bootstrapServers = builder.Configuration.GetConnectionString("kafka");
            if (string.IsNullOrEmpty(bootstrapServers))
                throw new InvalidOperationException("Kafka connection string is not configured!");

            options.BootstrapServers = bootstrapServers;
            options.Topic = "product-stock-topic";
        });
        builder.Services.AddSingleton<TopicInitializer>();
        builder.Services.AddHostedService<StartupInitializer>();
        builder.Services.AddSingleton<IProductStockProducer, ProductStockProducer>();
        builder.Services.AddHostedService<ProductStockConsumer>();
    }
    
    public static void AddRepository(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("ProductsDb");
            options
                .UseSqlServer(connectionString, o =>
                {
                    o.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        });
        
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
    }
}