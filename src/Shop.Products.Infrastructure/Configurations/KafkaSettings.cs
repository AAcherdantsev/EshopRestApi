namespace Shop.Products.Infrastructure.Configurations;

/// <summary>
///     The kafka settings.
/// </summary>
public class KafkaSettings
{
    /// <summary>
    ///     The bootstrap servers.
    /// </summary>
    public required string BootstrapServers { get; set; }

    /// <summary>
    ///     The topic.
    /// </summary>
    public required string Topic { get; set; }
}