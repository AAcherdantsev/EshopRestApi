using FastEndpoints;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Application.Messaging;

namespace Shop.Products.Api.Endpoints.V2;

public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    private readonly IProductStockProducer _productStockProducer;
    
    public PatchProductEndpoint(IProductStockProducer productStockProducer)
    {
        _productStockProducer = productStockProducer;
    }
    
    /// <inheritdoc/>
    public override void Configure()
    {
        Patch("/products/{id:int}");
        Version(2);
        
        Summary(s =>
        {
            s.Summary = "Update product by ID";
            s.Description = "Updates specific product fields by its ID.";
            s.Params["id"] = "Product ID";
            s.Response(StatusCodes.Status202Accepted, "Request accepted");
            s.Response(StatusCodes.Status400BadRequest, "Invalid request data");
            s.Response(StatusCodes.Status500InternalServerError, "Internal server error");
        });
        
        PreProcessors(new PatchProductProcessor());

        AllowAnonymous();
    }
    
    /// <inheritdoc/>
    public override async Task HandleAsync(PatchProductRequest req, CancellationToken ct)
    {
        var productId = Route<int>("id");
        var message = new PatchProductMessage()
        {
            ProductId = productId,
            NewQuantity = req.NewQuantity
        };
        
        await _productStockProducer.SendAsync(message);
        await Send.ResponseAsync(null!, StatusCodes.Status202Accepted, ct);
    }
}