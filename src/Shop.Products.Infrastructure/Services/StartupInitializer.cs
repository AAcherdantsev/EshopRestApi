using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shop.Products.Infrastructure.Messages;

namespace Shop.Products.Infrastructure.Services;

public class StartupInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public StartupInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var topicInitializer = scope.ServiceProvider.GetRequiredService<TopicInitializer>();
        var sqlTask = DatabaseInitializer.InitializeAsync(scope.ServiceProvider, addProductExamples: true);
        var topicTask = topicInitializer.EnsureTopicExistsAsync();
        await Task.WhenAll(sqlTask, topicTask);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}