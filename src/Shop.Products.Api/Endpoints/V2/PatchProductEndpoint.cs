using FastEndpoints;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Dto.Messages;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Application.Messaging;

namespace Shop.Products.Api.Endpoints.V2;

/// <summary>
///     Endpoint for patching a product's fields by ID.
/// </summary>
public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    private readonly IProductStockProducer _productStockProducer;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PatchProductEndpoint" /> class.
    /// </summary>
    /// <param name="productStockProducer">The product stock producer.</param>
    public PatchProductEndpoint(IProductStockProducer productStockProducer)
    {
        _productStockProducer = productStockProducer;
    }

    /// <summary>
    ///     Configures the endpoint route, version, Swagger documentation, pre-processors, and access.
    /// </summary>
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

    /// <summary>
    ///     Handles the PATCH request by sending a product update message.
    /// </summary>
    /// <param name="req">The patch product request containing the new quantity.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(PatchProductRequest req, CancellationToken ct)
    {
        var productId = Route<int>("id");
        var message = new PatchProductMessage
        {
            ProductId = productId,
            NewQuantity = req.NewQuantity
        };

        await _productStockProducer.SendAsync(message);
        await Send.ResponseAsync(null!, StatusCodes.Status202Accepted, ct);
    }
}