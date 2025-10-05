using Shop.Products.Application.Dto.Messages;

namespace Shop.Products.Application.Messaging;

/// <summary>
///     Defines a producer that sends product stock update messages.
/// </summary>
public interface IProductStockProducer
{
    /// <summary>
    ///     Sends a patch product message asynchronously.
    /// </summary>
    /// <param name="request">The message containing product ID and new quantity.</param>
    Task SendAsync(PatchProductMessage request);
}