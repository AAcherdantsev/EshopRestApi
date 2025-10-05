using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Products.Infrastructure.Messages;

namespace Shop.Products.Infrastructure.Services;

/// <summary>
/// Hosted service that initializes the database and messaging topics during application startup.
/// </summary>
public class StartupInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of the <see cref="StartupInitializer"/> class.  
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public StartupInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Starts the hosted service, initializing the database and ensuring Kafka topics exist.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to stop the initialization process.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var topicInitializer = scope.ServiceProvider.GetRequiredService<TopicInitializer>();
        var sqlTask = DatabaseInitializer.InitializeAsync(scope.ServiceProvider, addProductExamples: true);
        var topicTask = topicInitializer.EnsureTopicExistsAsync();
        await Task.WhenAll(sqlTask, topicTask);
    }

    /// <summary>
    /// Stops the hosted service.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}