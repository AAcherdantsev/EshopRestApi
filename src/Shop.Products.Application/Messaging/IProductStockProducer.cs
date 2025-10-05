using Shop.Products.Application.Dto.Messages;

namespace Shop.Products.Application.Messaging;

public interface IProductStockProducer
{
    Task SendAsync(PatchProductMessage request);
}